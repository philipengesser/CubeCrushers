using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TransformFollow : NetworkBehaviour
{
    public Transform toFollow;

    private Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner == false)
            return;

        toFollow = Camera.main.transform;
        myTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner == false)
            return;

        myTransform.position = toFollow.position;
        myTransform.rotation = toFollow.rotation;

    }
}
