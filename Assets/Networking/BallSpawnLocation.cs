using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnLocation : MonoBehaviour
{
    public static BallSpawnLocation s;

    private void Awake()
    {
        s = this;
    }
}
