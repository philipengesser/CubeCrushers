using Niantic.Lightship.SharedAR.Colocalization;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ImageTrackingColocalizationDemo_Simple : MonoBehaviour
{
    public SharedSpaceManager _sharedSpaceManager;

    [SerializeField]
    private Texture2D _targetImage;

    [SerializeField]
    private float _targetImageSize;

    public void StartSharedSpace(string roomName)
    {
        var imageTrackingOptions = ISharedSpaceTrackingOptions.CreateImageTrackingOptions(
                    _targetImage, _targetImageSize);
        var roomOptions = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
            roomName,
            10,
            "image tracking colocalization demo"
        );

        _sharedSpaceManager.StartSharedSpace(imageTrackingOptions, roomOptions);

        // start as host
        NetworkManager.Singleton.StartHost();
        // Or start as client
        // NetworkManager.Singleton.StartClient();
    }
}
