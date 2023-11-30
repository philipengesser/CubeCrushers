using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CubeSpawner : NetworkBehaviour
{
    //public GameObject CubePrefab;
    public GameObject CurrentCube;
    public float SpawnMaxX;
    public float SpawnMaxZ;

    public List<GameObject> CubePrefabs;

    // Update is called once per frame
    void Update()
    {
        if (IsServer == false)
            return;


        if (CurrentCube == null)
        {
            SpawnCube();
        }
    }

    public void SpawnCube()
    {
        float spawnX = Random.Range(-SpawnMaxX, SpawnMaxX);
        float spawnZ = Random.Range(-SpawnMaxZ, SpawnMaxZ);
        Vector3 spawnPos = transform.position +
            (transform.right * spawnX) +
            (transform.forward * spawnZ);
        spawnPos.y = GlobalVariables.CubeHeight;

        var cubePrefab = GetCubeToSpawn();
        if (cubePrefab == null)
            return;

        CurrentCube = 
            Instantiate(cubePrefab, spawnPos, Quaternion.identity);
        CurrentCube.transform.localScale = new Vector3(1, 1, 1) * GlobalVariables.CubeSize;
        CurrentCube.GetComponent<NetworkObject>().Spawn(true);
    }

    public GameObject GetCubeToSpawn()
    {
        if (LevelManager.s.MoreCubes())
        {
            CubeType nextCubeType = LevelManager.s.GetNextCube();
            return CubePrefabs[(int)nextCubeType];
        }
        else
        {
            LevelManager.s.WinLevel();
            return null;
        }
    }
}
