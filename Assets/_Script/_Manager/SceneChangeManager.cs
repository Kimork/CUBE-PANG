using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : SingletonManager<SceneChangeManager>
{
    public void SceneLoad(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
