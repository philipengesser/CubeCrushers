using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreText : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        text.text = $"High Score : {GlobalData.s.HighScore}";
    }
}
