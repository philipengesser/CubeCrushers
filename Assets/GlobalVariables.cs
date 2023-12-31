using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static bool ShowPlayerCapsule;
    public static bool ShowOppositeShortWall = false;
    public static float MatchTime = 120;
    public static float XPosition;
    public static float YPosition = 0;
    public static float ZPosition;
    public static int GroundHeight = -2;
    public static bool FakeGround = true;
    public static bool MeshingIsVisible = false;
    public static float BallSize = .25f;
    public static float CubeSize = 1;
    public static float CubeHeight = .5f;
    public static bool CatchMode = false;
    public static float SpawnInterval = 5;
    public static float AttackWallSpeed = 7;
    public static bool SpawnAttackWall = false;
    public static bool CreateCubes;
    public static float sphereCastRadius = 2;
    public static float sphereCastDistance = 0;
    public static float launchForce = 8;
    public static float localLaunchForceMultiplier = .6f;
    public static float upwardAngle = .5f;
}
