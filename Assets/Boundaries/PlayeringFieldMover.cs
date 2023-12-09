using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeringFieldMover : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            GlobalVariables.YPosition,
            transform.localPosition.z
            );
    }
}
