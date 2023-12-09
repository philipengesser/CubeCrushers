using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MatchSceneLoadedManager : MonoBehaviour
{

    private void Awake()
    {
        var xrManagerSettings = UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager;
        xrManagerSettings.DeinitializeLoader();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex); // reload current scene
        xrManagerSettings.InitializeLoaderSync();
    }
}
