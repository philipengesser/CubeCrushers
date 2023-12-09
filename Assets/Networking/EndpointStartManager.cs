using Niantic.Lightship.SharedAR.Colocalization;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EndpointStartManager : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        EndpointConnectedServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndpointConnectedServerRpc()
    {
        SetLevelIndexClientRpc(GlobalData.s.CurrentLevelIndex);
    }

    [ClientRpc]
    public void SetLevelIndexClientRpc(int levelIndex)
    {
        GlobalData.s.CurrentLevelIndex = levelIndex;
    }
}
