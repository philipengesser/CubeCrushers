using Niantic.Lightship.SharedAR.Colocalization;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EndpointStartManager : MonoBehaviour
{
    public NetworkManager NetworkManager;
    public GameObject CubeSpawnerPrefab;
    public SharedSpaceManager SharedSpaceManager;

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.OnServerStarted += OnServerStarted;
    }

    public void OnServerStarted()
    {
        var obj = Instantiate(
            CubeSpawnerPrefab, SharedSpaceManager.SharedArOriginObject.transform, false);

        obj.GetComponent<NetworkObject>().Spawn(true);
    }
}
