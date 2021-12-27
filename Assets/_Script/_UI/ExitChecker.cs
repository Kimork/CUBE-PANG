using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitChecker : MonoBehaviour
{
    public GameObject ExitUI;
    public Board Board;
    private bool m_IsUIEnabled = false;
    public void ShowExitUI()
    {
        LetterBox.Instance.EnablePanel();
        ExitUI.SetActive(true);
        m_IsUIEnabled = true;
    }

    public void HideExitUI()
    {
        if (Board != null)
            Board.BoardInput.InputEnable();

        LetterBox.Instance.DisablePanel();
        ExitUI.SetActive(false);
        m_IsUIEnabled = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void toggleUI()
    {
        if (!m_IsUIEnabled)
        {
            if (Board != null && Board.BoardInput.IsInputableNow())
            {
                Board.BoardInput.InputDisable();
                ShowExitUI();
            }
            else if (Board == null)
                ShowExitUI();
        }
        else
        {
            HideExitUI();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggleUI();
        }
    }
}
