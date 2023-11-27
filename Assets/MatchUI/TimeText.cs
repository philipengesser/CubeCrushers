using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    public TextMeshProUGUI Text;
    private void Update()
    {
        Text.text = ((int)TimerManager.s.TimeLeft.Value).ToString();
    }
}
