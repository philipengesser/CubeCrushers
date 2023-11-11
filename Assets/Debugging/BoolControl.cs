using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BoolControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var toggle = GetComponentInChildren<Toggle>();
        toggle.onValueChanged.AddListener((newValue) =>
        {
            SetValue(newValue);
        });
        toggle.isOn = GetValue();
    }

    public abstract void SetValue(bool newValue);
    public abstract bool GetValue();
}
