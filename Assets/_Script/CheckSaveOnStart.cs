using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckSaveOnStart : MonoBehaviour
{
    private void Awake()
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
