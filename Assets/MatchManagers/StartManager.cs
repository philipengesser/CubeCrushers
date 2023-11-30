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

    public void StartMatch()
    {
        //if (NetworkManager.IsClient == false)
        //    NetworkManager.StartHost();

        //var obj = Instantiate(GameBallPrefab);
        //obj.GetComponent<NetworkObject>().Spawn(true);
    }
}
