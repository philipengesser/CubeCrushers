using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : NetworkBehaviour
{
    public static TimerManager s;
    private void Awake()
    {
        s = this;
        TimeLeft.Value = GlobalVariables.MatchTime;
    }

    public NetworkVariable<float> TimeLeft;

    private void Update()
    {
        if (IsServer == false || StartManager.s.GameStarted == false)
            return;

        TimeLeft.Value -= Time.deltaTime;
        if (TimeLeft.Value <= 0)
            EndMatchClientRpc();

    }

    [ClientRpc]
    public void EndMatchClientRpc()
    {
        if (IsServer)
        {
            StartCoroutine(DelayedShutdown(2));
        }
        else
        {
            StartCoroutine(DelayedShutdown(0));
        }
    }

    public IEnumerator DelayedShutdown(float delay)
    {
        yield return new WaitForSeconds(delay);
        GlobalData.s.LastScore = ScoreManager.s.Score.Value;
        NetworkManager.Shutdown();
        SceneManager.LoadScene("MainMenu");
    }

    [ServerRpc(RequireOwnership = false)]
    public void DecreaseTimeServerRpc(int amountToDecrease)
    {
        TimeLeft.Value -= amountToDecrease;
    }
}
