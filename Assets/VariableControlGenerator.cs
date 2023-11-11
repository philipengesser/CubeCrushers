using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class VariableControlGenerator : MonoBehaviour
{
    public Transform controlPanel;
    public Toggle showControlsToggle;
    public GlobalVariables globalVariablesInstance; // Reference to your instance of GlobalVariables

    private bool showControls = true;
    private List<GameObject> controlObjects = new List<GameObject>();

    private void Start()
    {
        showControlsToggle.onValueChanged.AddListener(ShowHideControls);
        GenerateControls();
    }

    private void GenerateControls()
    {
        if (globalVariablesInstance == null)
        {
            Debug.LogError("GlobalVariables instance is not set. Please assign it in the Inspector.");
            return;
        }

        Type globalVariablesType = globalVariablesInstance.GetType();
        FieldInfo[] fields = globalVariablesType.GetFields();

        foreach (FieldInfo fieldInfo in fields)
        {
            if (fieldInfo.FieldType == typeof(int) || fieldInfo.FieldType == typeof(float) ||
                fieldInfo.FieldType == typeof(string))
            {
                CreateInputFieldControl(fieldInfo);
            }
            else if (fieldInfo.FieldType == typeof(bool))
            {
                CreateToggleControl(fieldInfo);
            }
        }
    }

    private void ShowHideControls(bool show)
    {
        showControls = show;
        foreach (var controlObject in controlObjects)
        {
            controlObject.SetActive(show);
        }
    }

    private void CreateInputFieldControl(FieldInfo fieldInfo)
    {
        GameObject controlObject = new GameObject(fieldInfo.Name);
        controlObject.transform.SetParent(controlPanel);

        Text label = controlObject.AddComponent<Text>();
        label.text = fieldInfo.Name;
        label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        InputField inputField = controlObject.AddComponent<InputField>();
        inputField.text = fieldInfo.GetValue(globalVariablesInstance).ToString();
        inputField.onEndEdit.AddListener((value) =>
        {
            object parsedValue = Convert.ChangeType(value, fieldInfo.FieldType);
            fieldInfo.SetValue(globalVariablesInstance, parsedValue);
        });

        controlObjects.Add(controlObject);
        controlObject.SetActive(showControls);
    }

    private void CreateToggleControl(FieldInfo fieldInfo)
    {
        GameObject controlObject = new GameObject(fieldInfo.Name);
        controlObject.transform.SetParent(controlPanel);

        Text label = controlObject.AddComponent<Text>();
        label.text = fieldInfo.Name;
        label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        Toggle toggle = controlObject.AddComponent<Toggle>();
        toggle.isOn = (bool)fieldInfo.GetValue(globalVariablesInstance);
        toggle.onValueChanged.AddListener((value) =>
        {
            fieldInfo.SetValue(globalVariablesInstance, value);
        });

        controlObjects.Add(controlObject);
        controlObject.SetActive(showControls);
    }
}
