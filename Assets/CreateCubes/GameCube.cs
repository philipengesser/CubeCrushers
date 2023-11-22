using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameCube : NetworkBehaviour
{
    public Renderer MyRenderer;
    public AudioSource CubeHitSource;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GlobalData.s.Score += 1;
            DestroyCubeServerRpc(collision.GetContact(0).point);
            //Destroy(this.gameObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyCubeServerRpc(Vector3 collisionPoint)
    {
        DestroyCubeClientRpc(collisionPoint);
        StartCoroutine(DespawnCubeAfterDelay());
    }

    public IEnumerator DespawnCubeAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<NetworkObject>().Despawn();
    }

    [ClientRpc]
    public void DestroyCubeClientRpc(Vector3 collisionPoint)
    {
        StartCoroutine(DestoryCubeVisuals(collisionPoint));
    }

    public IEnumerator DestoryCubeVisuals(Vector3 hitPoint)
    {
        CubeHitSource.Play();
        Material mat = MyRenderer.material;
        mat.SetVector("HitPoint", hitPoint);
        mat.SetFloat("HitSize", GlobalVariables.CubeSize);
        float t = .7f;
        while (t < 1)
        {
            t += Time.deltaTime / (t * t * t * t * t * t * t * t * t) / 7;
            mat.SetFloat("FadeSize", 1 - t);
            yield return null;
        }
    }
}
