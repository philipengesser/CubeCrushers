using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        updateHighScore();
        cleanupNetworkManager();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void updateHighScore()
    {
        if (GlobalData.s.HighScore < GlobalData.s.LastScore)
        {
            GlobalData.s.HighScore = GlobalData.s.LastScore;
        }
    }

    void cleanupNetworkManager()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }
    }
}
