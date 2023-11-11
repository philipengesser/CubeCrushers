using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject CubePrefab;
    public GameObject CurrentCube;
    public float SpawnMaxX;
    public float SpawnMaxZ;

    // Update is called once per frame
    void Update()
    {
        if (CurrentCube == null)
        {
            SpawnCube();
        }
    }

    public void SpawnCube()
    {
        float spawnX = Random.Range(-SpawnMaxX, SpawnMaxX);
        float spawnZ = Random.Range(-SpawnMaxZ, SpawnMaxZ);
        Vector3 spawnPos = new Vector3(spawnX, GlobalVariables.CubeHeight, spawnZ);
        CurrentCube = 
            Instantiate(CubePrefab, spawnPos, Quaternion.identity);
        CurrentCube.transform.localScale = new Vector3(1, 1, 1) * GlobalVariables.CubeSize;
    }
}
