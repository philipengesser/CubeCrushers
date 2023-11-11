using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBall : MonoBehaviour
{
    public Renderer MyRenderer;
    public Material NormalMat;
    public Material HitSomethingMat;
    public Material PlayerHitMat;
    public Rigidbody MyRigidbody;

    public float JustCollided = 0;
    public float JustHitByPlayer = 0;

    private void Update()
    {
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

        if (MyRigidbody.velocity.y > 5)
            MyRigidbody.velocity = new Vector3(MyRigidbody.velocity.x, 5, MyRigidbody.velocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        JustCollided = .2f;
    }
}
