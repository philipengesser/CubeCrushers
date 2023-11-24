using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager s;

    private void Awake()
    {
        s = this;
    }

    public List<Player> Players;
}
