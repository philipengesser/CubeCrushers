using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomButton : MonoBehaviour
{
    public TextMeshProUGUI RoomNameText;

    public void JoinRoom()
    {
        GlobalData.s.CurrentRoomName = RoomNameText.text;
        print("Check 1");
        print(GlobalData.s.CurrentRoomName);
        GlobalData.s.IsHost = false;
        SceneManager.LoadScene("Match");
    }
}
