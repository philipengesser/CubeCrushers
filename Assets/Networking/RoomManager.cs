using Niantic.Lightship.SharedAR.Colocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// The RoomManager uses ImageColocalization to connect players to the same SharedAR session and also starting them as hosts/clients.
/// </summary>
public class RoomManager : MonoBehaviour
{
    /// <summary>
    /// This is the image that we will have to look at to localize SharedAR sessions
    /// </summary>
    public Texture2D ImageForLocalization;
    /// <summary>
    /// Size in meters
    /// </summary>
    public float ImageSizeInMeters;
    public SharedSpaceManager SharedSpaceManager;
    public GameObject HowToLocalizeInstructionPanel;
    public GameObject SharedRootMarkerPrefab;
    public GameObject SharedSceneSetupPrefab;
    public GameObject SharedSceneSetupInstance;

    // Start is called before the first frame update
    void Start()
    {
        // Regester a callback for the  sharedSpaceManagerStateChanged event
        SharedSpaceManager.sharedSpaceManagerStateChanged += OnColocalizationTrackingStateChanged;

        // Create image tracking options
        var imageTrackingOptions = ISharedSpaceTrackingOptions.CreateImageTrackingOptions(
                ImageForLocalization, ImageSizeInMeters);

        // Create room options(the room options are what will connect two different players to the same Network match)
        var roomOptions = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                GlobalData.s.CurrentRoomName, 32, "Cube Crusher Demo!");
        // if this is the host then set the LevelIndex that we will be playing through using LightShips built in Room.Datastore functionality
        //if (GlobalData.s.IsHost)
        //    roomOptions.Room.Datastore.SetData(1, "LevelIndex", BitConverter.GetBytes(GlobalData.s.CurrentLevelIndex));
        //else // Otherwise set the current CurrentLevelIndex based on the value the host already set in the datastore 
        //    GlobalData.s.CurrentLevelIndex = BitConverter.ToInt32(roomOptions.Room.Datastore.GetData(1, "LevelIndex"));
        // Pass in the created imageTracking and room options to Lightships SharedSpaceManager to add the player to the room and start searching for the Colocalization image
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


    // This callback runs when Lightship recognizes the Image we are using for Colocolization for the first time
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
            // Create SharedScene object and parent it to the SharedAROrigin object to make sure the walls are in the same place on all devices
            Destroy(SharedSceneSetupInstance);
            SharedSceneSetupInstance =
                Instantiate(SharedSceneSetupPrefab,
                SharedSpaceManager.SharedArOriginObject.transform, false);
            //SharedSceneSetupInstance.transform.localPosition += new Vector3(0, -.5f, 0);
            // Start Unity Netcode for GameObjects as either a Host or Client based upon a global variable which is set in the main menu
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
