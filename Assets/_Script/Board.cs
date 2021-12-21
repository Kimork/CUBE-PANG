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

    private void Start()
    {
        CurrentBalls = new Ball[BoardSize.x, BoardSize.y];
        CreateTiles();

        SetLetterBox();
        //Screen.SetResolution(1080, 1920, false);

        BoardFiller.FillRow();
    }

    private void SetLetterBox()
    {
        Camera camera = Camera.main;
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // (가로 / 세로)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
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
