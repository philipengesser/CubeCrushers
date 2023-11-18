using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public Transform playerTransform;

    [SerializeField]
    public LayerMask BallMask;

    private void Start()
    {
        print("Player Created!");
    }

    // Update is called once per frame
    void Update()
    {
        if (HeldBall != null)
        {
            HeldBall.transform.position = PlayerHand.transform.position;
        }

        if (IsOwner == false)
            return;

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
    public Collider HeldBall;

    private void CatchAndThrow()
    {
        if (HeldBall == null)
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

        GrabBall();
        GrabBallServerRpc();
    }

    [ServerRpc]
    private void GrabBallServerRpc()
    {
        GrabBallClientRpc();
    }

    [ClientRpc]
    private void GrabBallClientRpc()
    {
        GrabBall();
    }

    private void GrabBall()
    {
        HeldBall = GameBall.s.GetComponent<Collider>();
        HeldBall.attachedRigidbody.isKinematic = true;
        HeldBall.attachedRigidbody.useGravity = false;
        HeldBall.attachedRigidbody.velocity = Vector3.zero;
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
        if (HeldBall == null)
            return;

        HeldBall.transform.parent = null;
        HeldBall.attachedRigidbody.useGravity = true;
        HeldBall.attachedRigidbody.isKinematic = false;
        LaunchBall(HeldBall);
        HeldBall = null;
    }

    #endregion

    private void TryToHitBall()
    {
        Collider ballCollider = DetectBall();
        if (ballCollider != null)
        {
            LaunchBall(ballCollider);
        }
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
    private void LaunchBall(Collider ballCollider)
    {
        Vector3 launchDir = transform.forward;

        // angle the launch up a little
        launchDir += Vector3.up * GlobalVariables.upwardAngle;
        launchDir = launchDir.normalized;


        ballCollider.attachedRigidbody.velocity = launchDir * GlobalVariables.launchForce;
        ballCollider.GetComponent<GameBall>().JustHitByPlayer = .2f;
    }
}
