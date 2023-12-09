using Niantic.Lightship.SharedAR.Rooms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the availble rooms. Rooms are handled by Niantic behind the scenes so you don't need to do any server side code and can just use the room functionality they built. The API key you get when you create a Lightship project is what ties the rooms tegether, so for example if you use the same API key in 2 different projects you will have the same set of rooms
/// </summary>
public class RoomsDisplay : MonoBehaviour
{
    // The Prefab to spawn in for each available room
    public GameObject RoomButtonPrefab;
    // A list of all the spawned room buttons, used to delete them before spawning in new ones on refresh
    public List<GameObject> CurrentRoomButtons;
    // The parent object to spawn the room buttons under
    public Transform RoomButtonHolder;

    // Start is called before the first frame update
    void Start()
    {
        RefreshDisplay();
    }
    /// <summary>
    /// Gets all the rooms associated with this game and displays them
    /// </summary>
    public void RefreshDisplay()
    {
        // This is the code that acutally gets the Rooms
        var getAllRoomsStatus = RoomManagementService.GetAllRooms(out List<IRoom> rooms);
        if (getAllRoomsStatus == RoomManagementServiceStatus.Ok ||
            getAllRoomsStatus == RoomManagementServiceStatus.NotFound)
        {
            RefreshDisplayUI(rooms);
        }
        else
        {
            print("Problem fetching rooms");
            print(getAllRoomsStatus.ToString());
        }
    }

    /// <summary>
    /// Deletes old room button gameobjects and then Instantiates new ones based on the passed in list
    /// </summary>
    /// <param name="rooms">The list of rooms to display</param>
    private void RefreshDisplayUI(List<IRoom> rooms)
    {
        // cleanup old room buttons
        for (int i = CurrentRoomButtons.Count - 1; i >= 0; i--)
        {
            Destroy(CurrentRoomButtons[i]);
        }
        CurrentRoomButtons.Clear();

        // Create new room buttons
        foreach (var room in rooms)
        {
            var obj = Instantiate(RoomButtonPrefab, RoomButtonHolder);
            obj.GetComponent<RoomButton>().RoomNameText.text = 
                $"{room.RoomParams.Name}";
            // : {room.Networking.PeerIDs.Count}
            CurrentRoomButtons.Add(obj);
        }
        
    }

    /// <summary>
    /// Used to delete all available rooms, this is a hacky way of cleanup rooms so you will definitely want a better solution for a production application
    /// </summary>
    public void DeleteRooms()
    {
        RoomManagementService.GetAllRooms(out List<IRoom> rooms);
        foreach (var room in rooms)
        {
            RoomManagementService.DeleteRoom(room.RoomParams.RoomID);
        }

        RefreshDisplay();
    }
}
