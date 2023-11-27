using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWall : MonoBehaviour
{
    private float SelfDestructTimer = 10;
    public float MoveSpeed;

    private void Update()
    {
        transform.Translate(Vector3.back * MoveSpeed * Time.deltaTime);

        SelfDestructTimer -= Time.deltaTime;
        if (SelfDestructTimer < 0)
            Destroy(this.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //GlobalData.s.LastScore -= 5;
            ScoreManager.s.DecreaseScoreServerRpc(5);
            Destroy(this.gameObject);
        }
    }
}
