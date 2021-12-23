using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class CheckSaveOnStart : MonoBehaviour
{
    public static bool IsLoginGPGS = false;

    private void Awake()
    {
#if UNITY_ANDROID 
        InitGPGS();
#endif

#if UNITY_EDITOR_WIN
        CheckData();
#endif
    }

    private void InitGPGS()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false)
            .Build();
        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.Activate();
        LoginGPGS();
    }

    private void LoginGPGS()
    {

        try
        {

            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                CheckData();
            }
            else
            {
                PlayGamesPlatform.Instance.Authenticate((bool Success) =>
                {
                    if (Success)
                    {
                        CheckData();
                        IsLoginGPGS = Success;
                    }
                    else
                    {
                        //로그인 실패
                        IsLoginGPGS = Success;
                    }
                });
            }
        }
        catch (System.Exception)
        {
            throw;
        } 
    }

    private void CheckData()
    {
        SetEncryptedPlayerPrefs();

        if (EncryptedPlayerPrefs.GetInt(Board.IsPlayingKey) == 1)
            SceneManager.LoadScene(2);
        else
            SceneManager.LoadScene(1);
    }

    public void SetEncryptedPlayerPrefs()
    {
        if (ReferenceEquals(EncryptedPlayerPrefs.keys, null))
        {
            EncryptedPlayerPrefs.keys = new string[5];

            EncryptedPlayerPrefs.keys[0] = "7z3DBe69";
            EncryptedPlayerPrefs.keys[1] = "0GDczs34";
            EncryptedPlayerPrefs.keys[2] = "GvbsI116";
            EncryptedPlayerPrefs.keys[3] = "UIVoziv4";
            EncryptedPlayerPrefs.keys[4] = "jaw3eDAs";

        }
        if (!EncryptedPlayerPrefs.HasKey(Board.RecordKey))
        {
            EncryptedPlayerPrefs.SetInt(Board.RecordKey, 0);
            EncryptedPlayerPrefs.SetInt(Board.IsPlayingKey, 0);
            EncryptedPlayerPrefs.SetInt(Board.PresetIndexKey, 0);
            EncryptedPlayerPrefs.SetInt(Board.LastScoreKey, 0);
            EncryptedPlayerPrefs.SetString(Board.BallsDataKey, "Null");
        }
    }
}
