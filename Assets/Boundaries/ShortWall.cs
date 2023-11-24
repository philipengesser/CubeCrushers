using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ShortWall : MonoBehaviour
{
    public bool LosePointsOnHit = false;
    public ParticleSystem WallHitSystem;
    public AudioSource WallSource;
    public AudioClip WallHitClip;
    public AudioClip BackWallHitClip;
    public Transform BallSpawnPosition;
    public Vector3 BallResetLocalPosition;

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


            //var _startRotationX = WallHitSystem.main.startRotationX;
            //_startRotationX.constant = transform.rotation.eulerAngles.x;
            //var _startRotationY = WallHitSystem.main.startRotationY;
            //_startRotationY.constant = transform.rotation.eulerAngles.y;
            //var _startRotationZ = WallHitSystem.main.startRotationZ;
            //_startRotationZ.constant = transform.rotation.eulerAngles.z;


            //main.startRotationY = transform.rotation.eulerAngles.y;
            //main.startRotationZ = transform.rotation.eulerAngles.z;
            WallHitSystem.Emit(3);

            if (NetworkManager.Singleton != null &&
                (NetworkManager.Singleton.IsServer == false || 
                NetworkManager.Singleton.ConnectedClients.Count > 1))
            {
                Player closerPlayer = PlayerManager.s.Players.OrderBy(p =>
                    Vector3.Distance(p.transform.position, GameBall.s.transform.position))
                    .FirstOrDefault();
                // This makes it so that only the player closest to the ball can control it
                if (closerPlayer == null || closerPlayer.IsOwner == false)
                    return;

                Vector3 ballPosition = BallResetLocalPosition;
                GameBall.s.ResetBallPositionStart(ballPosition, Vector3.zero);
            }
            else
            {
                if (LosePointsOnHit)
                {
                    GlobalData.s.Score -= 3;
                    WallSource.PlayOneShot(BackWallHitClip);
                }
                else
                {
                    WallSource.PlayOneShot(WallHitClip);
                }
                    

                // if the ball is not moving down the court fast enough add some speed in the direction down the court
                Rigidbody ballRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                float minVelocity = 4;
                Vector3 direction = transform.up;
                float velocityInDirection = Vector3.Dot(ballRigidbody.velocity, direction);



                if (velocityInDirection < minVelocity)
                {
                    ballRigidbody.velocity = (ballRigidbody.velocity / 2) +
                        direction * minVelocity * Mathf.Sign(velocityInDirection);
                }
            }
        }
    }
}
