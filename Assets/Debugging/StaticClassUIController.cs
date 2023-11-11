using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using TMPro;

public class StaticClassUIController : MonoBehaviour
{
    //public GlobalModifiable staticClassType; // Assign the static class type in the Inspector

    public GameObject IntInputField;
    public GameObject BoolInputField;

    private void Start()
    {
        FieldInfo[] fields = typeof(GlobalVariables).GetFields(BindingFlags.Public | BindingFlags.Static);
        foreach (FieldInfo field in fields)
        {
            GameObject controlObject = null;

            if (field.FieldType == typeof(bool))
            {
                controlObject = Instantiate(BoolInputField, transform);
                Toggle toggle = controlObject.transform.Find("Toggle")
                    .GetComponent<Toggle>();
                toggle.isOn = (bool)field.GetValue(null);
                toggle.onValueChanged.AddListener((value) => field.SetValue(null, value));
            }
            else if (field.FieldType == typeof(int))
            {
                controlObject = Instantiate(IntInputField, transform);
                TMP_InputField inputField = controlObject.transform.Find("InputField (TMP)")
                    .GetComponent<TMP_InputField>();
                inputField.text = ((int)field.GetValue(null)).ToString();
                inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
                inputField.onValueChanged.AddListener((value) =>
                {
                    if (int.TryParse(value, out int intValue))
                        field.SetValue(null, intValue);
                });
            }
            else if (field.FieldType == typeof(float))
            {
                controlObject = Instantiate(IntInputField, transform);
                TMP_InputField inputField = controlObject.transform.Find("InputField (TMP)")
                    .GetComponent<TMP_InputField>();
                inputField.text = ((float)field.GetValue(null)).ToString();
                inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
                inputField.onValueChanged.AddListener((value) =>
                {
                    if (float.TryParse(value, out float floatValue))
                        field.SetValue(null, floatValue);
                });
            }
            if (controlObject != null)
            {
                var labelText = controlObject.transform.Find("Name")
                    .GetComponent<TextMeshProUGUI>();
                labelText.text = field.Name;
            }
        }
    }
}

