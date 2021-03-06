﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    string score;

    public void ButtonSurvivalPressed ()
    {
        SceneManager.LoadScene(Scenes.Survival);
    }

    public void ButtonQuitPressed ()
    {
        Application.Quit();
    }

    public void OnButtonShowLeaderboard()
    {
        Debug.Log("Showing Leaderboard");
        GPGS.ShowLeaderboardUI();
    }
}