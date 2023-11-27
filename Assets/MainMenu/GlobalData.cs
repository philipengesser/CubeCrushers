using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData s;

    private void Awake()
    {
        if (s == null)
        {
            s = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    public int LastScore;
    public int HighScore;
    public string CurrentRoomName;
    public bool IsHost;
}
