using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    public Renderer MyRenderer;
    public Material InvisibleMat;
    public Material VisibleMat;

    private void Update()
    {
        if (GlobalVariables.MeshingIsVisible)
            MyRenderer.material = VisibleMat;
        else
            MyRenderer.material = InvisibleMat;
    }
}
