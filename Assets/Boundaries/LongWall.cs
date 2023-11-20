using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            //float absoluteZ = Mathf.Abs(ballRigidbody.velocity.z);
            float minZ = 3;
            Vector3 direction = transform.right;
            float velocityInDirection = Vector3.Dot(ballRigidbody.velocity, direction);



            if (velocityInDirection < minZ)
            {
                ballRigidbody.velocity = (ballRigidbody.velocity / 2) +
                    direction * minZ * Mathf.Sign(velocityInDirection);
                //ballRigidbody.velocity = new Vector3(
                //    ballRigidbody.velocity.x,
                //    ballRigidbody.velocity.y,
                //    Mathf.Sign(ballRigidbody.velocity.z) * minZ
                //    );
            }
        }
    }
}
