using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum CubeType { NormalCube, SpeedCube, SlowCube, LaunchCube}

public class GameCube : NetworkBehaviour
{
    public Renderer MyRenderer;
    public AudioSource CubeHitSource;
    public bool DestroyVisualsRan;
    public CubeType CubeType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            switch (CubeType)
            {
                case CubeType.NormalCube:
                    break;
                case CubeType.SpeedCube:
                    GameBall.s.BallSpeedUpTimeLeft = 3f;
                    break;
                case CubeType.SlowCube:
                    GameBall.s.BallSlowDownTimeLeft = .2f;
                    break;
                case CubeType.LaunchCube:
                    GameBall.s.LaunchBallUp();
                    break;
                default:
                    break;
            }

            ScoreManager.s.IncreaseScoreServerRpc(1);
            DestroyCubeServerRpc(other.transform.position);
            StartCoroutine(DestoryCubeVisuals(other.transform.position));
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
        if (DestroyVisualsRan == true)
            yield break;

        DestroyVisualsRan = true;
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
