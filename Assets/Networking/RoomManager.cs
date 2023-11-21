using Niantic.Lightship.SharedAR.Colocalization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Texture2D ImageForLocalization;
    public float ImageSizeInMeters;
    public SharedSpaceManager SharedSpaceManager;
    public GameObject HowToLocalizeInstructionPanel;
    public GameObject SharedRootMarkerPrefab;
    public GameObject SharedSceneSetupPrefab;
    public GameObject SharedSceneSetupInstance;

    // Start is called before the first frame update
    void Start()
    {
        
        SharedSpaceManager.sharedSpaceManagerStateChanged += OnColocalizationTrackingStateChanged;


        var imageTrackingOptions = ISharedSpaceTrackingOptions.CreateImageTrackingOptions(
                ImageForLocalization, ImageSizeInMeters);

        //set room name from text box
        var roomOptions = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                GlobalData.s.CurrentRoomName, 32, "Cube Crusher Demo!");

        SharedSpaceManager.StartSharedSpace(imageTrackingOptions, roomOptions);

    }

    private void OnDestroy()
    {
        SharedSpaceManager.sharedSpaceManagerStateChanged -= OnColocalizationTrackingStateChanged;
        if (NetworkManager.Singleton != null)
        {
            // shutdown and destroy NetworkManager when switching the scene
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
        }
    }



    private void OnColocalizationTrackingStateChanged(SharedSpaceManager.SharedSpaceManagerStateChangeEventArgs args)
    {
        if (args.Tracking)
        {
            Debug.Log("Colocalized.");
            // Hide the target image instruction panel
            HowToLocalizeInstructionPanel.SetActive(false);

            // create an origin marker object and set under the sharedAR origin
            Instantiate(SharedRootMarkerPrefab,
                SharedSpaceManager.SharedArOriginObject.transform, false);
            // SharedScene code to make sure the walls are in the same place on all devices
            Destroy(SharedSceneSetupInstance);
            SharedSceneSetupInstance =
                Instantiate(SharedSceneSetupPrefab,
                SharedSpaceManager.SharedArOriginObject.transform, false);
            // Start networking
            if (GlobalData.s.IsHost)
            {
                NetworkManager.Singleton.StartHost();
                print("Starting Host");
            }
            else
            {
                NetworkManager.Singleton.StartClient();
                print("Starting Client");
            }
        }
        else
        {
            Debug.Log($"Image tracking not tracking?");
        }
    }
}
