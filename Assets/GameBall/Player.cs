using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform playerTransform;

    [SerializeField]
    public LayerMask BallMask;


    // Update is called once per frame
    void Update()
    {
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
            GrabBall();
        }
        else
        {
            ThrowBall();
        }
    }

    private void GrabBall()
    {
        Collider ballCollider = DetectBall();
        if (ballCollider == null)
            return;

        HeldBall = ballCollider;
        HeldBall.transform.parent = PlayerHand;
        HeldBall.transform.localPosition = Vector3.zero;
        HeldBall.attachedRigidbody.isKinematic = true;
        HeldBall.attachedRigidbody.useGravity = false;
        HeldBall.attachedRigidbody.velocity = Vector3.zero;
    }

    private void ThrowBall()
    {
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
        Vector3 launchDir = Camera.main.transform.forward;

        // angle the launch up a little
        launchDir += Vector3.up * GlobalVariables.upwardAngle;
        launchDir = launchDir.normalized;


        ballCollider.attachedRigidbody.velocity = launchDir * GlobalVariables.launchForce;
        ballCollider.GetComponent<GameBall>().JustHitByPlayer = .2f;
    }
}
