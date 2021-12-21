using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [HideInInspector]
    public BoardFiller BoardFiller;
    [HideInInspector]
    public BallController BallController;
    [HideInInspector]
    public BoardInput BoardInput;
    [HideInInspector]
    public BoardQuery BoardQuery;

    public GameObject TilePrefab;
    public Vector2Int BoardSize;
    public Ball[,] CurrentBalls;

    public NumToImageString ScoreUI;
    public NumToImageString RecordUI;
    public int Score = 0;
    public int Record = 0;

    private void Start()
    {
        CurrentBalls = new Ball[BoardSize.x, BoardSize.y];
        CreateTiles();
        //Screen.SetResolution(1080, 1920, false);

        BoardFiller.FillRow();
    }

    public void AddScore()
    {
        ScoreUI.SetString(++Score);
    }

    public void AddRecord()
    {
        RecordUI.SetString(++Record);
    }


    public Ball GetBall(Vector2Int pos)
    {
        return CurrentBalls[pos.x, pos.y];
    }

    private void CreateTiles()
    {
        for (int x = 0; x < BoardSize.x; x++)
        {
            for (int y = 0; y < BoardSize.y; y++)
            {
                var _createdTile = Instantiate(TilePrefab, transform);

                var _tile = _createdTile.GetComponent<Tile>();
                _tile.Init(this, new Vector2Int(x, y));
            }
        }
    }
}
