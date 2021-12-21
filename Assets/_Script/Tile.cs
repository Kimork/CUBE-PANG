using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Board Board;
    public BoardInput BoardInput;
    public Vector2Int PosInfo;

    public void Init(Board board, Vector2Int pos)
    {
        Board = board;
        BoardInput = Board.BoardInput;
        PosInfo = pos;
        transform.localPosition = new Vector3(pos.x, pos.y, 0);
    }

    private void OnMouseDown()
    {
        Board.BoardInput.Pop(PosInfo);
    }
}
