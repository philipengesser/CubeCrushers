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
    public int CurrentLevelIndex;
    // The first int represents the levels index, the second int represents the number of stars obtained on that level
    // using a dictionary instead of a list because it is a bit more dynamic for adding new levels
    public Dictionary<int, int> LevelsCleared = new Dictionary<int, int>();
}
