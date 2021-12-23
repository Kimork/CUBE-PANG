using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : SingletonManager<SceneChangeManager>
{

    public override void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (LetterBox.Instance != null)
                LetterBox.Instance.RefreshCam();

            if (OnGameStart.Instance != null)
            {
                OnGameStart.Instance.InitBannerAD();

                OnGameStart.Instance.DestroyPopupAD();
                if (scene.buildIndex == 2)
                {
                    OnGameStart.Instance.LoadPopupAD();
                }
            }
        };
    }


    public void SceneLoad(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
