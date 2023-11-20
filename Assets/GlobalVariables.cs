using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static float YPosition = 1.6f;
    public static float XPosition = 0;
    public static float ZPosition = 0;
    public static int GroundHeight = -2;
    public static bool FakeGround = true;
    public static bool MeshingIsVisible = false;
    public static float BallSize = .125f;
    public static float CubeSize = 1;
    public static float CubeHeight = -.5f;
    public static bool CatchMode = true;
    public static float SpawnInterval = 5;
    public static float AttackWallSpeed = 7;
    public static bool SpawnAttackWall = false;
    public static bool CreateCubes;
    public static float sphereCastRadius = 2;
    public static float sphereCastDistance = 0;
    public static float launchForce = 10;
    public static float upwardAngle = 0;
}
