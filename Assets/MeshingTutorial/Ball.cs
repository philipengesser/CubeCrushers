using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print("Check 1");
        print(collision.gameObject.tag);
        if (collision.gameObject.tag == "GameCube")
        {
            GlobalData.s.LastScore += 1;
            Destroy(this.gameObject);
        }
    }
}
