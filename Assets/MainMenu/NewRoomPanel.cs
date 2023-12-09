using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Niantic.Lightship.SharedAR.Colocalization;
using UnityEngine.SceneManagement;

/// <summary>
/// This allows players to enter the name for a new room and create it.
/// </summary>
public class NewRoomPanel : MonoBehaviour
{
    public TMP_InputField RoomNameInput;
    public Button CreateRoomButton;

    private void Start()
    {
        RoomNameInput.text = GenerateRandomAlphanumericString(6);
    }
    public void CreateRoom()
    {
        GlobalData.s.CurrentRoomName = RoomNameInput.text;
        GlobalData.s.IsHost = true;
        // When this scene loads a new room will be created if the entered room name(GlobalData.s.CurrentRoomName) is different from any other existing room names
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

    static string GenerateRandomAlphanumericString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // You can use a StringBuilder for better performance if needed
        char[] randomString = new char[length];
        for (int i = 0; i < length; i++)
        {
            randomString[i] = chars[Random.Range(0, chars.Length)];
        }

        return new string(randomString);
    }
}
