using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardQuery : MonoBehaviour
{
    public Board Board;
    private void Awake()
    {
        Board = (Board)GetComponent("Board");
        Board.BoardQuery = this;
    }

    public Vector2Int GetNullSpaceInColumn(Vector2Int pos)
    {
        Vector2Int _result = new Vector2Int(-1, -1);

        for (int y = pos.y; y >= 0; y--)
        {
            if (ReferenceEquals(Board.CurrentBalls[pos.x, y], null))
                _result = new Vector2Int(pos.x, y);
        }

        return _result;

    }

    public List<Ball> GetPopBalls(Vector2Int clickedPos)
    {
        var _balls = GetPopBalls(clickedPos, Board.GetBall(clickedPos).MatchColor);

        return _balls;
    }

    private List<Ball> GetPopBalls(Vector2Int startPos, MatchValue color)
    {
        List<Ball> _result = new List<Ball>();

        Queue<Vector2Int> _node = new Queue<Vector2Int>();
        _node.Enqueue(startPos);

        while (_node.Count != 0)
        {
            var _startPos = _node.Dequeue();

            for (int x = _startPos.x; x < Board.BoardSize.x; x++)
            {
                if (IsWithinBouns(x, _startPos.y))
                {
                    var _b1 = Board.CurrentBalls[x, _startPos.y];

                    if (_b1.MatchColor == color)
                    {
                        _result.Add(_b1);

                        for (int y = _startPos.y + 1; y < Board.BoardSize.y; y++)
                        {
                            if (IsWithinBouns(x, y))
                            {
                                var _b2 = Board.CurrentBalls[x, y];

                                if (_b2.MatchColor == color && !_result.Contains(_b2))
                                {
                                    _result.Add(_b2);
                                    _node.Enqueue(_b2.CurrentPos);
                                }
                                else
                                    break;
                            }
                            else
                                break;
                        }

                        for (int y = _startPos.y - 1; y >= 0; y--)
                        {
                            if (IsWithinBouns(x, y))
                            {
                                var _b2 = Board.CurrentBalls[x, y];

                                if (_b2.MatchColor == color && !_result.Contains(_b2))
                                {
                                    _result.Add(_b2);
                                    _node.Enqueue(_b2.CurrentPos);
                                }
                                else
                                    break;
                            }
                            else
                                break;
                        }
                    }
                    else
                        break;
                }
                else
                    break;
            }

            for (int x = _startPos.x - 1; x >= 0; x--)
            {
                if (IsWithinBouns(x, _startPos.y))
                {
                    var _b1 = Board.CurrentBalls[x, _startPos.y];

                    if (_b1.MatchColor == color)
                    {
                        _result.Add(_b1);

                        for (int y = _startPos.y + 1; y < Board.BoardSize.y; y++)
                        {
                            if (IsWithinBouns(x, y))
                            {
                                var _b2 = Board.CurrentBalls[x, y];

                                if (_b2.MatchColor == color && !_result.Contains(_b2))
                                {
                                    _result.Add(_b2);
                                    _node.Enqueue(_b2.CurrentPos);
                                }
                                else
                                    break;
                            }
                            else
                                break;
                        }

                        for (int y = _startPos.y - 1; y >= 0; y--)
                        {
                            if (IsWithinBouns(x, y))
                            {
                                var _b2 = Board.CurrentBalls[x, y];

                                if (_b2.MatchColor == color && !_result.Contains(_b2))
                                {
                                    _result.Add(_b2);
                                    _node.Enqueue(_b2.CurrentPos);
                                }
                                else
                                    break;
                            }
                            else
                                break;
                        }
                    }
                    else
                        break;
                }
                else
                    break;
            }
        }

        return _result;
    }

    public bool IsWithinBouns(int x, int y)
    {
        if (x < 0 || y < 0)
            return false;

        if (x >= Board.BoardSize.x || y >= Board.BoardSize.y)
            return false;

        if (ReferenceEquals(Board.CurrentBalls[x, y], null))
            return false;

        return true;
    }
}
