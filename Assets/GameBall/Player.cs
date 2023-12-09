using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// This class is attached to the NetworkedPlayer and thus gets spawned in for each client when they connect to the match. It handles everything related to the local player hitting the ball and all remote players updating the balls position/velocity on their clients
/// </summary>
public class Player : NetworkBehaviour
{
    // This will be the Owned player on each client. So on client1 it will be the player client1 controls etc
    public static Player LocalPlayer;

    public Transform playerTransform;

    // The layer that the ball is on so that we can easily detect the ball and know it isn't something else
    [SerializeField]
    public LayerMask BallMask;

    // Used by the host to spawn the ball when the match starts
    public GameObject GameBallPrefab;
    public AudioSource PlayerSource;
    public AudioClip PlayerHitClip;

    // When the player taps on the screen, how long to check to see if the ball is in range. This is important because without it the player feels like they need to tap multiple times to make sure they hit the ball
    public float SwingDuration;
    private float swingTimeLeft;
    // Visual for where the player is, used for debugging
    public Renderer PlayerCapsule;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner == true)
            LocalPlayer = this;

        if (IsServer == false || IsOwner == false)
            return;
    }

    private void Start()
    {
        print("Player Created!");
        PlayerManager.s.Players.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        // Sets the visibility of the player capsule based on the GlobalVariables.ShowPlayerCapsule bool. default to disabled but can be useful for debugging
        PlayerCapsule.enabled = GlobalVariables.ShowPlayerCapsule;

        if (IsOwner == false)
            return;

        swingTimeLeft -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            swingTimeLeft = SwingDuration;
        }

        if (swingTimeLeft > 0)
        {
            TryToHitBall();
        }
        
    }
    /// <summary>
    /// Checks to see if the ball is in range of the player and if so hits it
    /// </summary>
    private void TryToHitBall()
    {
        Collider ballCollider = DetectBall();
        if (ballCollider != null)
        {
            LaunchBall();
        }
    }


    /// <summary>
    /// Returns the ball collider if the ball is withen raduis(GlobalVariables.sphereCastRadius) of the player
    /// </summary>
    /// <returns></returns>
    private Collider DetectBall()
    {
        Collider ballColliderToReturn = null;
        var colliders = Physics.OverlapSphere(playerTransform.position, GlobalVariables.sphereCastRadius, BallMask);
        if (colliders != null && colliders.Length > 0)
            ballColliderToReturn = colliders[0];

        if (ballColliderToReturn == null)
        {
            Ray ray = new Ray(playerTransform.position, playerTransform.forward);
            if (Physics.SphereCast(ray, GlobalVariables.sphereCastRadius,
                out RaycastHit hit, GlobalVariables.sphereCastDistance,
                BallMask))
            {
                ballColliderToReturn = hit.collider;
            }

        }

        return ballColliderToReturn;
    }
    

    /// <summary>
    /// This function calls both the local code and ServerRpc launch the ball in the direction the player is pointing
    /// </summary>
    private void LaunchBall()
    {
        swingTimeLeft = 0;
        Vector3 launchDir = transform.forward;

        // angle the launch up a little based on GlobalVariables.upwardAngle
        launchDir += Vector3.up * GlobalVariables.upwardAngle;
        launchDir = launchDir.normalized;

        Vector3 ballPosition = GameBall.s.transform.localPosition;
        Vector3 ballVelocity = launchDir * GlobalVariables.launchForce;
        // transform the ball velocity to be relative to the SharedAROrigin since different players will have different global cordinate spaces
        ballVelocity = GameBall.s.transform.parent.InverseTransformVector(ballVelocity);

        // This line applies the physics locally. It will happen immeadatly so that the can see that they hit the ball instantly
        ApplyBallPhysics(ballPosition, ballVelocity);
        // This line calls the ServerRpc which calls the ClientRpc which calls the above line on all the other clients. This is what synchronizes the balls position
        ApplyBallPhysicsServerRpc(ballPosition, ballVelocity, OwnerClientId);
    }

    /// <summary>
    /// This server RPC is just a 'pass through' function which calls the ApplyBallPhysicsClientRpc. All the parameters are just passed into ApplyBallPhysicsClientRpc
    /// </summary>
    /// <param name="ballPosition">The position to put this ball on all clients</param>
    /// <param name="ballVelocity">The velocity to apply to this ball on all clients</param>
    /// <param name="originatingClientID">The Id of the client who called this ServerRpc, this is used to prevent code that has already been run locally on the calling client from being ran again on that client</param>
    [ServerRpc]
    private void ApplyBallPhysicsServerRpc(Vector3 ballPosition, Vector3 ballVelocity, ulong originatingClientID)
    {
        print("ServerRpc called");
        ApplyBallPhysicsClientRpc(ballPosition, ballVelocity, originatingClientID);
    }

    /// <summary>
    /// This client RPC is just a 'pass through' function which calls the ApplyBallPhysics.
    /// </summary>
    /// <param name="ballPosition">The position to put this ball on all clients</param>
    /// <param name="ballVelocity">The velocity to apply to this ball on all clients</param>
    /// <param name="originatingClientID">The Id of the client or called the Server RPC that called this Client RPC. this is used to prevent code that has already been run locally on the calling client from being ran again on that client</param>
    [ClientRpc]
    private void ApplyBallPhysicsClientRpc(Vector3 ballPosition, Vector3 ballVelocity, ulong originatingClientID)
    {
        print("id : " + originatingClientID);
        // If this is the client that actually sent this message then don't apply the ball physics again
        if (IsOwner)
            return;
        print("ClientRpc called");
        ApplyBallPhysics(ballPosition, ballVelocity);
    }

    /// <summary>
    /// This is the function that actually handles apply the players interactions with the ball. It simply sets the balls position and velocity based on passed in parameters. It is not a networked function
    /// </summary>
    /// <param name="ballPosition">The position to put this ball on this instance</param>
    /// <param name="ballVelocity"></param>
    private void ApplyBallPhysics(Vector3 ballPosition, Vector3 ballVelocity)
    {
        print("ApplyBallPhysicsCalled");
        // Note : I'm setting the balls local position because everything needs to be relative to the SharedAR Origin
        GameBall.s.transform.localPosition = ballPosition;
        // This code slows the ball down if the force is being applied on the client who inituated the interaction. The reason for this is to help compensate for the latency time it will take for the message to reach the other player.
        if (IsOwner && 
            (IsServer == false || NetworkManager.Singleton.ConnectedClientsIds.Count > 1))
        {
            ballVelocity *= GlobalVariables.localLaunchForceMultiplier;
        }
        // Note : I'm converting the ball velocity to be in the cordinate space of the SharedAR Origin since different players will have different global cordinate spaces     
        ballVelocity = GameBall.s.transform.parent.TransformVector(ballVelocity);
        GameBall.s.MyRigidbody.velocity = ballVelocity;
        // This changes the balls color for the specified duration so that I can see that the ball really was hit
        GameBall.s.JustHitByPlayer = .4f;
        PlayerSource.PlayOneShot(PlayerHitClip);
    }
}
