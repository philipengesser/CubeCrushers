using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWallSpawner : MonoBehaviour
{
    public GameObject AttackWallPrefab;
    public float timeToSpawn;

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariables.SpawnAttackWall == false)
            return;

        timeToSpawn -= Time.deltaTime;
        if (timeToSpawn <= 0)
        {
            SpawnAttackWall();
            timeToSpawn = GlobalVariables.SpawnInterval;
        }
    }

    public void SpawnAttackWall()
    {
        int spawnRightSide = Random.Range(0, 2);
        Vector3 spawnPos = new Vector3();
        if (spawnRightSide == 1)
            spawnPos = new Vector3(2, 0, 0);
        else
            spawnPos = new Vector3(-2, 0, 0);

        Instantiate(AttackWallPrefab, spawnPos, Quaternion.identity);
    }
}
