using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInput : MonoBehaviour
{
    public Board Board;
    private void Awake()
    {
        Board = (Board)GetComponent("Board");
        Board.BoardInput = this;
    }

    public void Pop(Vector2Int clickedPos)
    {
        if (!ReferenceEquals(Board.GetBall(clickedPos), null))
        {
            Board.BoardFiller.DestroyBall(Board.BoardQuery.GetPopBalls(clickedPos));
        }
    }
}
