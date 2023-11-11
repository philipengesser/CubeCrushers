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
            float absoluteZ = Mathf.Abs(ballRigidbody.velocity.z);
            float minZ = 6;
            if (absoluteZ < minZ)
            {
                ballRigidbody.velocity = new Vector3(
                    ballRigidbody.velocity.x,
                    ballRigidbody.velocity.y,
                    Mathf.Sign(ballRigidbody.velocity.z) * minZ
                    );
            }
        }
    }
}
