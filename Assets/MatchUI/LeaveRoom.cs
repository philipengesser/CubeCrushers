using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveRoom : MonoBehaviour
{
    public NetworkManager NetworkManager;

    public void Leave()
    {
        NetworkManager.Shutdown();
        SceneManager.LoadScene("MainMenu");
    }


}
