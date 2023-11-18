using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LastScoreText : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        text.text = $"Last Score : {GlobalData.s.Score}";
    }
}
