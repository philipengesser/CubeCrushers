using Niantic.Lightship.SharedAR.Rooms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsDisplay : MonoBehaviour
{
    public float RoomRefreshSpeed;
    public GameObject RoomButtonPrefab;
    public List<GameObject> CurrentRoomButtons;
    public Transform RoomButtonHolder;

    // Start is called before the first frame update
    void Start()
    {
        //RoomManagementService.GetAllRooms(out List<IRoom> rooms);
        //foreach (var room in rooms)
        //{
        //    print(room.Networking.PeerIDs.Count);
        //    RoomManagementService.DeleteRoom(room.RoomParams.RoomID);
        //}
        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        var getAllRoomsStatus = RoomManagementService.GetAllRooms(out List<IRoom> rooms);
        if (getAllRoomsStatus == RoomManagementServiceStatus.Ok)
        {
            RefreshDisplayUI(rooms);
        }
        else
        {
            print("Problem fetching rooms");
            print(getAllRoomsStatus.ToString());
        }
    }
    private void RefreshDisplayUI(List<IRoom> rooms)
    {
        // cleanup room buttons
        for (int i = CurrentRoomButtons.Count - 1; i >= 0; i--)
        {
            Destroy(CurrentRoomButtons[i]);
        }
        CurrentRoomButtons.Clear();

        foreach (var room in rooms)
        {
            var obj = Instantiate(RoomButtonPrefab, RoomButtonHolder);
            obj.GetComponent<RoomButton>().RoomNameText.text = 
                $"{room.RoomParams.Name} : {room.Networking.PeerIDs.Count}";
        }
        
    }

}
