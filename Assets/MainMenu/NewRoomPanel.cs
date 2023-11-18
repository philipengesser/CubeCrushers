using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Niantic.Lightship.SharedAR.Colocalization;
using UnityEngine.SceneManagement;

public class NewRoomPanel : MonoBehaviour
{
    public TMP_InputField RoomNameInput;
    public Button CreateRoomButton;

    public void CreateRoom()
    {
        GlobalData.s.CurrentRoomName = RoomNameInput.text;
        GlobalData.s.IsHost = true;
        SceneManager.LoadScene("Match");
    }

    // Update is called once per frame
    void Update()
    {
        if (RoomNameInput.text.Length <= 0)
        {
            CreateRoomButton.interactable = false;
        }
        else
        {
            CreateRoomButton.interactable = true;
        }
    }
}
