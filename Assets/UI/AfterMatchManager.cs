using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AfterMatchManager : MonoBehaviour
{

    public GameObject DefeatView;
    public GameObject VictoryView;
    public Image[] Stars;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Start is called before the first frame update
    void Start()
    {
        VictoryView.SetActive(GlobalData.s.WonMatch);
        DefeatView.SetActive(!GlobalData.s.WonMatch);

        for (int i = 0; i < 3; i++)
        {
            if (i < GlobalData.s.StarsEarned)
                Stars[i].color = Color.white;
            else
                Stars[i].color = Color.black;

        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
