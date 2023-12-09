using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// The CubeSpawner is responsible for spawning the cubes you have to destroy. The cubes are instantiated on the server and then Spawned(i.e. Instantiated on the clients)
/// </summary>
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
        // The cubes are Instantiated on the server then we use the NGO Spawn function to add them to clients. That is why we only want this code to run on the server
        if (IsServer == false)
            return;


        if (CurrentCube == null)
        {
            SpawnCube();
        }
    }

    public void SpawnCube()
    {
        // generate a semi random position for the cube
        float spawnX = Random.Range(-SpawnMaxX, SpawnMaxX);
        float spawnZ = Random.Range(-SpawnMaxZ, SpawnMaxZ);
        Vector3 spawnPos = transform.localPosition +
            (transform.right * spawnX) +
            (transform.forward * spawnZ);
        spawnPos.y = GlobalVariables.CubeHeight;
        // get the prefab for the next cube we want to spawn
        var cubePrefab = GetCubeToSpawn();
        if (cubePrefab == null)
            return;

        CurrentCube = 
            Instantiate(cubePrefab, spawnPos, Quaternion.identity);
        CurrentCube.transform.localScale = new Vector3(1, 1, 1) * GlobalVariables.CubeSize;
        // Spawn the cube on all clients
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
            LevelManager.s.WinLevelClientRpc();
            return null;
        }
    }
}
