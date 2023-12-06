using Niantic.Lightship.SharedAR.Colocalization;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the start of the game, the start will always be initialized by the host. ImageLocalization may or may not be used for SharedAR before the game starts
/// </summary>
public class StartManager : NetworkBehaviour
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

    /// <summary>
    /// This button should only be available for the host, after pressing it the match will start and new players will not be able to join the room
    /// </summary>
    public Button StartMatchButton;

    private void Start()
    {
#if !UNITY_EDITOR
        if (GlobalData.s.IsHost == false)
            StartMatchButton.gameObject.SetActive(false);
#endif
    }

    public void StartMatch()
    {
        if (NetworkManager.IsClient == false)
            NetworkManager.StartHost();

        var obj = Instantiate(GameBallPrefab);
        obj.GetComponent<NetworkObject>().Spawn(true);

        var obj2 = Instantiate(CubeSpawnerPrefab);
        obj2.GetComponent<NetworkObject>().Spawn(true);

        PreMatchUI.SetActive(false);
        GameStarted = true;
    }
}
