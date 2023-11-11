using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGroundContoller : MonoBehaviour
{
    public Collider Collider;

    private void Update()
    {
        if (GlobalVariables.FakeGround == true)
            Collider.enabled = true;
        else
            Collider.enabled = false;

        transform.position = new Vector3(
            transform.position.x,
            GlobalVariables.GroundHeight,
            transform.position.z
            );
    }
}
