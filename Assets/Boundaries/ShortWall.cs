using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ShortWall : MonoBehaviour
{
    public bool LosePointsOnHit = false;
    public NetworkManager NetworkManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (NetworkManager.Singleton != null &&
                NetworkManager.Singleton.ConnectedClients.Count > 1)
            {
                GlobalData.s.Score -= 2;
                collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                collision.gameObject.transform.position = transform.position +
                    (transform.up * 4);
            }
            else
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
}
