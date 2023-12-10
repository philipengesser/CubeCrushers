using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedSceneSetup : MonoBehaviour
{
    public List<MeshRenderer> FieldRenderers;
    private List<Color> OriginalColors = new List<Color>();

    public static SharedSceneSetup s;
    public void Awake()
    {
        s = this;
    }

    private void Start()
    {
        foreach (var fieldRenderer in FieldRenderers)
        {
            OriginalColors.Add(fieldRenderer.material.GetColor("Color_e8c8cae3d71f4dbca367ce937d4107d0"));
        }
    }

    public void FlashRed()
    {
        StartCoroutine(FlashRedCo());
    }

    private IEnumerator FlashRedCo()
    {
        yield return StartCoroutine(FlashOnce());
        yield return StartCoroutine(FlashOnce());
        yield return StartCoroutine(FlashOnce());
        yield return StartCoroutine(FlashOnce());
        yield return StartCoroutine(FlashOnce());
    }

    private IEnumerator FlashOnce()
    {
        foreach (var fieldRenderer in FieldRenderers)
        {
            fieldRenderer.material.SetColor("Color_e8c8cae3d71f4dbca367ce937d4107d0", Color.red);
        }

        yield return new WaitForSeconds(.1f);

        for (int i = 0; i < FieldRenderers.Count; i++)
        {
            FieldRenderers[i].material.SetColor("Color_e8c8cae3d71f4dbca367ce937d4107d0", OriginalColors[i]);
        }

        yield return new WaitForSeconds(.1f);
    }

}
