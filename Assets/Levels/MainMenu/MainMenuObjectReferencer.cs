using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuObjectReferencer : MonoBehaviour
{
    public static MainMenuObjectReferencer s;

    public GameObject LevelSelectionScreen;

    // Start is called before the first frame update
    void Start()
    {
        s = this;
    }
}
