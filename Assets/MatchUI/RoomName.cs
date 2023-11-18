using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomName : MonoBehaviour
{
    private void Start()
    {
        print("Check 2");
        print(GlobalData.s.CurrentRoomName);
        GetComponent<TextMeshProUGUI>().text = GlobalData.s.CurrentRoomName;
    }
}
