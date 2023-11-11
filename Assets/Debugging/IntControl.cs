using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class IntControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var input = GetComponentInChildren<TMP_InputField>();
        input.onValueChanged.AddListener((newValue) =>
        {
            SetValue(int.Parse(newValue));
        });
        input.text = GetValue().ToString();
    }

    public abstract void SetValue(int newValue);
    public abstract int GetValue();
}
