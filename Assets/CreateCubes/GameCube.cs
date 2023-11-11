using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCube : MonoBehaviour
{
    public Renderer MyRenderer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GlobalVariables.Score += 1;
            Destroy(this.gameObject);
            //StartCoroutine(DestoryCube(collision.GetContact(0).point));
        }
    }

    public IEnumerator DestoryCube(Vector3 hitPoint)
    {
        Material mat = MyRenderer.material;
        mat.SetVector("HitPoint", hitPoint);
        mat.SetFloat("HitSize", 1);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            mat.SetFloat("FadeSize", 1 - t);
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
