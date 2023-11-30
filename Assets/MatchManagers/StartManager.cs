using Niantic.Lightship.SharedAR.Colocalization;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    public static StartManager s;
    private void Awake()
    {
        s = this;
    }

    public NetworkManager NetworkManager;
    public GameObject CubeSpawnerPrefab;
    public SharedSpaceManager SharedSpaceManager;
    public bool GameStarted;

    // gameobjects to disable when the match starts
    public GameObject PreMatchUI;

    // prefabs to spawn in when the game starts
    public GameObject GameBallPrefab;
    

    public void StartMatch()
    {
        if (NetworkManager.IsClient == false)
            NetworkManager.StartHost();

        var obj = Instantiate(GameBallPrefab);
        obj.GetComponent<NetworkObject>().Spawn(true);

        var obj2 = Instantiate(
            CubeSpawnerPrefab, SharedSpaceManager.SharedArOriginObject.transform, false);
        obj2.GetComponent<NetworkObject>().Spawn(true);

        PreMatchUI.SetActive(false);
        GameStarted = true;
    }
}
