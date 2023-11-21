using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongWall : MonoBehaviour
{
    public ParticleSystem WallHitSystem;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Play wall hit visuals
            WallHitSystem.transform.position = collision.GetContact(0).point;
            var main = WallHitSystem.main;
            main.startRotationX = Mathf.Deg2Rad * transform.rotation.eulerAngles.x * 90;
            main.startRotationY = Mathf.Deg2Rad * transform.rotation.eulerAngles.y;
            main.startRotationZ = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;

            WallHitSystem.Emit(3);

            // if the ball is not moving down the court fast enough add some speed in the direction down the court
            Rigidbody ballRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            float minForward = 4;
            Vector3 direction = transform.right;
            float velocityInDirection = Vector3.Dot(ballRigidbody.velocity, direction);



            if (velocityInDirection < minForward)
            {
                ballRigidbody.velocity = (ballRigidbody.velocity / 2) +
                    direction * minForward * Mathf.Sign(velocityInDirection);
            }
        }
    }
}
