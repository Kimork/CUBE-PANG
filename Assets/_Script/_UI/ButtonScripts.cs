using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class ButtonScripts : MonoBehaviour
{
    public void SceneLoad(int sceneNum)
    {
        SceneChangeManager.Instance.SceneLoad(sceneNum);
    }

    public void ShowRanking()
    {
#if UNITY_ANDROID 
        try
        {
            if (OnGameStart.IsLoginGPGS)
                Social.ShowLeaderboardUI();
        }
        catch (System.Exception)
        {

            throw;
        }
#endif
    }

    public void ShowAchieve()
    {
#if UNITY_ANDROID 
        try
        {
            if (OnGameStart.IsLoginGPGS)
                Social.ShowAchievementsUI();
        }
        catch (System.Exception)
        {

            throw;
        }
#endif
    }
}
