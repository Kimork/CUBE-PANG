using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LetterBox : SingletonManager<LetterBox>
{
    private Camera m_Cam;

    public override void Awake()
    {
        base.Awake();
        m_Cam = (Camera)GetComponent("Camera");
    }
    public void RefreshCam()
    {
        m_Cam.Render();
    }

}
