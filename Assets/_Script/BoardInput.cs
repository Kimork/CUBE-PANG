using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInput : MonoBehaviour
{
    public Board Board;
    private bool m_IsInputable = true;
    private void Awake()
    {
        Board = (Board)GetComponent("Board");
        Board.BoardInput = this;
    }

    public void InputEnable()
    {
        m_IsInputable = true;
    }

    public void InputDisable()
    {
        m_IsInputable = false;
    }

    public void Pop(Vector2Int clickedPos)
    {
        if (m_IsInputable)
        {
            InputDisable();

            if (!ReferenceEquals(Board.GetBall(clickedPos), null))
            {
                Board.BoardFiller.DestroyBall(Board.BoardQuery.GetPopBalls(clickedPos));
            }
        }
    }
}
