using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortWall : MonoBehaviour
{
    public bool LosePointsOnHit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (LosePointsOnHit)
                GlobalData.s.Score -= 3;

            Rigidbody ballRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            float absoluteX = Mathf.Abs(ballRigidbody.velocity.x);
            float maxX = 3;
            if (absoluteX > maxX)
            {
                ballRigidbody.velocity = new Vector3(
                    Mathf.Sign(ballRigidbody.velocity.x) * maxX,
                    ballRigidbody.velocity.y,
                    ballRigidbody.velocity.z
                    );
            }
        }
    }
}
