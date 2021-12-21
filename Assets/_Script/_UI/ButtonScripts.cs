using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScripts : MonoBehaviour
{
    public void SceneLoad(int sceneNum)
    {
        SceneChangeManager.Instance.SceneLoad(sceneNum);
    }
}
