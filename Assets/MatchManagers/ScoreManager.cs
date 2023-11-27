using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager s;

    public NetworkVariable<int> Score;

    private void Awake()
    {
        s = this;
    }

    [ServerRpc(RequireOwnership = false)]
    public void IncreaseScoreServerRpc(int amountToIncrease)
    {
        Score.Value += amountToIncrease;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DecreaseScoreServerRpc(int amountToDecrease)
    {
        Score.Value -= amountToDecrease;
    }
}
