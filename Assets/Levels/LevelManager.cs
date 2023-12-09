using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : NetworkBehaviour
{
    public List<Level> Levels;
    public float TwoStarTimeThreshold;
    public float ThreeStarTimeThreshold;
    public int NextCubeIndex;
    public Level CurrentLevel
    {
        get
        {
            return Levels[GlobalData.s.CurrentLevelIndex - 1];
        }
    }
    public static LevelManager s;
    private void Awake()
    {
        s = this;
    }

    private void Start()
    {
        TimerManager.s.TimeLeft.Value = CurrentLevel.MatchTime;
    }

    public bool MoreCubes()
    {
        return NextCubeIndex < CurrentLevel.CubesToSpawn.Count;
    }

    public CubeType GetNextCube()
    {
        // note that I'm using the postfix increment operator in this case because I want to increment the value after getting the cube, I think this is the first time I've ever had a use for this nuanced feature, I recall in college one of the test questions was about prefix/postfix operators, fun times :)
        return CurrentLevel.CubesToSpawn[NextCubeIndex++];
    }

    [ClientRpc]
    public void WinLevelClientRpc()
    {
        if (IsServer)
        {
            StartCoroutine(DelayedWinLevel(2));
        }
        else
        {
            StartCoroutine(DelayedWinLevel(0));
        }
    }

    public IEnumerator DelayedWinLevel(int delay)
    {
        yield return new WaitForSeconds(delay);
        WinLevel();
    }

    public void WinLevel()
    {
        int stars = 1;
        if (TimerManager.s.TimeLeft.Value > ThreeStarTimeThreshold)
            stars = 3;
        else if (TimerManager.s.TimeLeft.Value > TwoStarTimeThreshold)
            stars = 2;

        GlobalData.s.WonMatch = true;
        GlobalData.s.StarsEarned = stars;
        GlobalData.s.TimeLeft = TimerManager.s.TimeLeft.Value;

        if (GlobalData.s.LevelsCleared.ContainsKey(GlobalData.s.CurrentLevelIndex))
        {
            if (GlobalData.s.LevelsCleared[GlobalData.s.CurrentLevelIndex] < stars)
                GlobalData.s.LevelsCleared[GlobalData.s.CurrentLevelIndex] = stars;
        }
        else
        {
            GlobalData.s.LevelsCleared.Add(GlobalData.s.CurrentLevelIndex, stars);
        }

        SceneManager.LoadScene("AfterMatchScene");
    }
}
