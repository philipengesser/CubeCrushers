using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public Transform playerTransform;

    [SerializeField]
    public LayerMask BallMask;

    public GameObject LocalGameBallPrefab;
    public NetworkVariable<NetworkObjectReference> localGameBall = new NetworkVariable<NetworkObjectReference>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner == false)
            return;

        
        SpawnLocalGameBallServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnLocalGameBallServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        var obj = Instantiate(LocalGameBallPrefab);

        obj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        localGameBall.Value = obj;
        StartCoroutine(DelaySetLocalGameBall(obj));
    }

    IEnumerator DelaySetLocalGameBall(GameObject obj)
    {
        yield return new WaitForSeconds(3);
        // Disable the ball if the player is not the server so that we only start with 1 ball
        SetLocalGameBallClientRpc(obj, IsOwnedByServer);
        
    }

    [ClientRpc]
    public void SetLocalGameBallClientRpc(NetworkObjectReference gameBall, bool active)
    {
        ((GameObject)localGameBall.Value).SetActive(active);
    }

    private void Start()
    {
        print("Player Created!");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner == false)
            return;

        if (HeldBallRigidbody != null)
        {
            HeldBallRigidbody.transform.position = PlayerHand.transform.position;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (GlobalVariables.CatchMode == true)
            {
                CatchAndThrow();
            }
            else
            {
                TryToHitBall();
            }

        }
    }

    #region Catch And Throw stuff

    public Transform PlayerHand;
    public Rigidbody HeldBallRigidbody;

    private void CatchAndThrow()
    {
        if (HeldBallRigidbody == null)
        {
            GrabBallLocal();
        }
        else
        {
            ThrowBallLocal();
        }
    }

    private void GrabBallLocal()
    {
        Collider ballCollider = DetectBall();
        if (ballCollider == null)
            return;

        GrabBall(ballCollider.gameObject);
        GrabBallServerRpc(ballCollider.gameObject);
    }

    [ServerRpc]
    private void GrabBallServerRpc(NetworkObjectReference gameBall)
    {
        GrabBallClientRpc(gameBall);
    }

    [ClientRpc]
    private void GrabBallClientRpc(NetworkObjectReference gameBall)
    {
        GrabBall(gameBall);
    }

    private void GrabBall(GameObject ballObject)
    {
        ballObject.SetActive(false);

        ((GameObject)localGameBall.Value).SetActive(true);
        HeldBallRigidbody = ((GameObject)localGameBall.Value).GetComponent<Rigidbody>();
        HeldBallRigidbody.isKinematic = true;
        HeldBallRigidbody.useGravity = false;
        HeldBallRigidbody.velocity = Vector3.zero;
    }

    private void ThrowBallLocal()
    {
        ThrowBall();
        ThrowBallServerRpc();
    }

    [ServerRpc]
    private void ThrowBallServerRpc()
    {
        ThrowBallClientRpc();
    }

    [ClientRpc]
    private void ThrowBallClientRpc()
    {
        ThrowBall();
    }
    private void ThrowBall()
    {
        if (HeldBallRigidbody == null)
            return;

        HeldBallRigidbody.transform.parent = null;
        HeldBallRigidbody.useGravity = true;
        HeldBallRigidbody.isKinematic = false;
        LaunchBall(HeldBallRigidbody);
        HeldBallRigidbody = null;
    }

    #endregion

    private void TryToHitBall()
    {
        Collider ballCollider = DetectBall();
        if (ballCollider != null)
        {
            EnableLocalBallServerRpc(ballCollider.GetComponent<NetworkObject>());
            LaunchBall(ballCollider.GetComponent<Rigidbody>());
        }
    }

    [ServerRpc]
    public void EnableLocalBallServerRpc(NetworkObjectReference networkObjectReference)
    {
        EnableLocalBallClientRpc(networkObjectReference);
    }

    [ClientRpc]
    public void EnableLocalBallClientRpc(NetworkObjectReference networkObjectReference)
    {
        ((GameObject)networkObjectReference).SetActive(false);
        ((GameObject)localGameBall.Value).SetActive(true);
    }

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
    private void LaunchBall(Rigidbody ballRigidbody)
    {
        Vector3 launchDir = transform.forward;

        // angle the launch up a little
        launchDir += Vector3.up * GlobalVariables.upwardAngle;
        launchDir = launchDir.normalized;


        ballRigidbody.velocity = launchDir * GlobalVariables.launchForce;
        ballRigidbody.GetComponent<GameBall>().JustHitByPlayer = .2f;
    }
}
