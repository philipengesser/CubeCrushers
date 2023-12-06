using Niantic.Lightship.SharedAR.Colocalization;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EndpointStartManager : NetworkBehaviour
{
    public NetworkManager NetworkManager;
    public NetworkVariable<int> currentLevelIndexNetworked = new NetworkVariable<int>();

    // Start is called before the first frame update
    void Start()
    {
        //NetworkManager.OnServerStarted += OnServerStarted;
        NetworkManager.OnClientStarted += OnClientStarted;
    }

    public override void OnNetworkSpawn()
    {
        print("Check A");
        EndpointConnectedServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndpointConnectedServerRpc()
    {
        print("Check B");
        SetLevelIndexClientRpc(GlobalData.s.CurrentLevelIndex);
    }

    [ClientRpc]
    public void SetLevelIndexClientRpc(int levelIndex)
    {
        print("Check C");
        GlobalData.s.CurrentLevelIndex = levelIndex;
    }

    public void OnClientStarted()
    {
        
    }

    public void OnServerStarted()
    {
        //currentLevelIndexNetworked.Value = GlobalData.s.CurrentLevelIndex;
    }
}
