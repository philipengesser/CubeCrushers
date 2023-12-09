using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Keeps track of the match time remaining, once the time reaches zero the player loses the match
/// </summary>
public class TimerManager : NetworkBehaviour
{
    public static TimerManager s;
    private void Awake()
    {
        s = this;
        TimeLeft.Value = GlobalVariables.MatchTime;
    }
    // Note : only the server can modify this value
    public NetworkVariable<float> TimeLeft;

    private void Update()
    {
        // Only move forward if the match is started and this is the server
        if (IsServer == false || StartManager.s.GameStarted == false)
            return;
        
        TimeLeft.Value -= Time.deltaTime;
        // Once we ran out of time we can call the EndMatchClientRpc, we know we are on the server because of the above check. Since we know we are on the server we can directly call the client Rpc
        if (TimeLeft.Value <= 0) 
            EndMatchClientRpc();

    }
    /// <summary>
    /// Calls DelayedLose Coroutine, delays it by 3 seconds on the Server to make sure the message reaches clients.
    /// </summary>
    [ClientRpc]
    public void EndMatchClientRpc()
    {
        if (IsServer)
        {
            StartCoroutine(DelayedLose(2));
        }
        else
        {
            StartCoroutine(DelayedLose(0));
        }
    }

    /// <summary>
    /// This Coroutine makes the player lose the match, the delay is added so that the server does not switch scenes before it can send the EndMatchClientRpc to all the clients
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    public IEnumerator DelayedLose(float delay)
    {
        yield return new WaitForSeconds(delay);
        GlobalData.s.LastScore = ScoreManager.s.Score.Value;
        NetworkManager.Shutdown();

        GlobalData.s.WonMatch = false;
        GlobalData.s.StarsEarned = 0;
        GlobalData.s.TimeLeft = 0;
        SceneManager.LoadScene("AfterMatchScene");
    }

    /// <summary>
    /// This Server Rpc is required since clients cannot directly change NetworkVariables. Using this Server Rpc we can have the client tell the server to change the TimeLeft
    /// </summary>
    /// <param name="amountToDecrease">The number of seconds to decrease the TimeLeft by</param>
    [ServerRpc(RequireOwnership = false)]
    public void DecreaseTimeServerRpc(int amountToDecrease)
    {
        TimeLeft.Value -= amountToDecrease;
    }
}
