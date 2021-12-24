using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LetterBox : SingletonManager<LetterBox>
{
    private Camera m_Cam;
    public GameObject GameOverPanel;

    public override void Awake()
    {
        base.Awake();
        m_Cam = (Camera)GetComponent("Camera");

        if (Screen.height >= Screen.width)
        {
            float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16);
            scaleheight *= Screen.height;
            m_Cam.orthographicSize = Screen.height / scaleheight * 6f;
        }
        else
        {
            m_Cam.orthographicSize = 6f;
        }

    }

    public void EnablePanel()
    {
        GameOverPanel.SetActive(true);
    }

    public void DisablePanel()
    {
        GameOverPanel.SetActive(false);
    }

    public void RefreshCam()
    {
        m_Cam.Render();
    }

}
