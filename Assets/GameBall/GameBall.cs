using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameBall : NetworkBehaviour
{
    public static GameBall s;

    public Renderer MyRenderer;
    public Material NormalMat;
    public Material HitSomethingMat;
    public Material PlayerHitMat;
    public Rigidbody MyRigidbody;

    public float JustCollided = 0;
    public float JustHitByPlayer = 0;

    public AudioSource BallSource;
    public AudioClip BallBounceOffGround;
    public AudioClip BallBounceOffWall;
    public AudioClip BallHitCube;
    public AudioClip BallHitBackWall;
    public AudioClip BallHitByPlayer;

    private bool resetingBallPosition;

    public void Awake()
    {
        s = this;
    }

    private void Update()
    {
        print("Position : " + transform.position);
        transform.localScale = new Vector3(1,1,1) * GlobalVariables.BallSize;

        if (JustHitByPlayer > 0)
        {
            MyRenderer.material = PlayerHitMat;
            JustHitByPlayer -= Time.deltaTime;
        }
        else if (JustCollided > 0)
        {
            MyRenderer.material = HitSomethingMat;
            JustCollided -= Time.deltaTime;
        }
        else
        {
            MyRenderer.material = NormalMat;
        }

        if (MyRigidbody.velocity.y > 7)
            MyRigidbody.velocity = new Vector3(MyRigidbody.velocity.x, 5, MyRigidbody.velocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        JustCollided = .2f;

        if (collision.gameObject.CompareTag("LongWall"))
            BallSource.PlayOneShot(BallBounceOffWall);
        else 
            BallSource.PlayOneShot(BallBounceOffGround);
    }

    public void ResetBallPositionStart(Vector3 ballPosition, Vector3 ballVelocity)
    {
        resetingBallPosition = true;
        ResetBallPositionServerRpc(ballPosition, ballVelocity);
        ResetBallPosition(ballPosition, ballVelocity);
    }

    [ServerRpc(RequireOwnership =false)]
    private void ResetBallPositionServerRpc(Vector3 ballPosition, Vector3 ballVelocity)
    {
        //print("ServerRpc called");
        ResetBallPositionClientRpc(ballPosition, ballVelocity);
    }

    [ClientRpc]
    private void ResetBallPositionClientRpc(Vector3 ballPosition, Vector3 ballVelocity)
    {
        // If this is the client that actually sent this message then don't apply the ball physics again
        if (resetingBallPosition == true)
        {
            resetingBallPosition = false;
            return; 
        }
        ResetBallPosition(ballPosition, ballVelocity);
    }

    private void ResetBallPosition(Vector3 ballPosition, Vector3 ballVelocity)
    {
        GameBall.s.transform.localPosition = ballPosition;
        GameBall.s.MyRigidbody.velocity = ballVelocity;
        BallSource.PlayOneShot(BallHitBackWall);
    }
}
