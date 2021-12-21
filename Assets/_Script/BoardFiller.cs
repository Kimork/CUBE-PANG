using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BoardFiller : MonoBehaviour
{
    public Board Board;
    public GameObject BallPrefab;

    public ObjectPoolManager<Ball> BallPool;
    public AnimationCurve BallPopCurve;

    private Vector2Int m_Size;
    private int m_SponeHeight;
    private List<List<int[]>> m_LinePreset;
    private MatchValue[] m_BallPreset;
    private int m_DestroyingBallCount = 0;

    private void Awake()
    {
        Board = (Board)GetComponent("Board");
        Board.BoardFiller = this;

        m_Size = Board.BoardSize;
        m_SponeHeight = m_Size.y + 1;

        BallPool = new ObjectPoolManager<Ball>(BallPrefab, Board.transform, m_Size.x * m_Size.y + 10, new Vector3(-5, -5, 0));
        SetPreset();
    }

    private void SetPreset()
    {
        m_LinePreset = new List<List<int[]>>();

        m_LinePreset.Add(new List<int[]>() { new int[] { 0, 1, 2, 3, 4, 5 } });
        m_LinePreset.Add(new List<int[]>() { new int[] { 0, 1, 2 }, new int[] { 3, 4, 5 } });
        m_LinePreset.Add(new List<int[]>() { new int[] { 0, 1 }, new int[] { 2, 3 }, new int[] { 4, 5 } });
        m_LinePreset.Add(new List<int[]>() { new int[] { 0, 1 }, new int[] { 2, 3, 4, 5 } });
        m_LinePreset.Add(new List<int[]>() { new int[] { 0, 1, 2, 3 }, new int[] { 4, 5 } });

        List<MatchValue[]>  _BallPreset = new List<MatchValue[]>();

        _BallPreset.Add(new MatchValue[] { MatchValue.Zebra, MatchValue.Cat, MatchValue.Orca, MatchValue.Pig });
        _BallPreset.Add(new MatchValue[] { MatchValue.Penguin, MatchValue.Beaver, MatchValue.Snake, MatchValue.Elephant });
        _BallPreset.Add(new MatchValue[] { MatchValue.Rhino, MatchValue.Gorilla, MatchValue.Donkey, MatchValue.Monkey });
        _BallPreset.Add(new MatchValue[] { MatchValue.Dog, MatchValue.Orca, MatchValue.Panda, MatchValue.Donkey });
        _BallPreset.Add(new MatchValue[] { MatchValue.Panda, MatchValue.Snake, MatchValue.Donkey, MatchValue.Pig });

        var _index = Random.Range(0, _BallPreset.Count);
        m_BallPreset = _BallPreset[_index];
    }

    public async void DestroyBall(Ball target)
    {
        m_DestroyingBallCount++;

        var _target = target.transform;

        target.SetPopSprite();
        var _popTween = _target.DOScale(new Vector3(0.6f, 0.6f, 1), 0.25f).SetEase(BallPopCurve);
        await _popTween.AsyncWaitForCompletion();

        if (target.IsPlayAfraid)
            target.StopAfraidAnim();

        BallPool.PushObject(target);
        Board.CurrentBalls[target.CurrentPos.x, target.CurrentPos.y] = null;
        m_DestroyingBallCount--;
    }

    public void DestroyBall(List<Ball> targets)
    {
        StartCoroutine(DestroyBallRoutine(targets));
    }

    private IEnumerator DestroyBallRoutine(List<Ball> targets)
    {
        foreach (var _ball in targets)
        {
            DestroyBall(_ball);
        }

        yield return new WaitUntil(() => m_DestroyingBallCount == 0);
        FillRow();
    }

    private void FillNullSpace()
    {
        for (int y = 1; y < m_Size.y; y++)
        {
            for (int x = 0; x < m_Size.x; x++)
            {
                if (Board.BoardQuery.IsWithinBouns(x, y))
                {
                    var _nullCheck = Board.BoardQuery.GetNullSpaceInColumn(new Vector2Int(x, y));

                    if (_nullCheck.x != -1)
                        Board.BallController.MoveBall(Board.GetBall(new Vector2Int(x, y)), _nullCheck, 0.12f);
                }
            }
        }
    }
    public void FillRow()
    {
        StartCoroutine(FillRowRoutine());
    }

    private IEnumerator FillRowRoutine()
    {
        foreach (var _ball in Board.CurrentBalls)
        {
            if (!ReferenceEquals(_ball, null) && _ball.IsPopSprite)
            {
                _ball.SetNormalSprite();
                _ball.StopAfraidAnim();
            }
        }

        FillNullSpace();

        var _presetIndex = Random.Range(0, m_LinePreset.Count - 1);

        MatchValue[] _value = new MatchValue[m_LinePreset[_presetIndex].Count];

        var _valueList = m_BallPreset.ToList();

        for (int i = 0; i < _value.Length; i++)
        {
            var _index = Random.Range(0, _valueList.Count);
            _value[i] = _valueList[_index];
            _valueList.RemoveAt(_index);
        }

        bool _isGameOver = false;
        bool _isPinch = false;

        for (int x = 0; x < m_Size.x; x++)
        {
            for (int y = 0; y < m_Size.y; y++)
            {
                if (ReferenceEquals(Board.CurrentBalls[x, y], null))
                {

                    for (int i = 0; i < m_LinePreset[_presetIndex].Count; i++)
                    {
                        if (m_LinePreset[_presetIndex][i].Contains(x))
                        {
                            var _newBall = BallPool.GetObject();
                            _newBall.transform.localPosition = new Vector3(x, m_SponeHeight, 0);
                            _newBall.transform.localScale = new Vector3(1, 1, 1);
                            _newBall.SetColor(_value[i]);

                            _newBall.gameObject.SetActive(true);
                            Board.BallController.MoveBall(_newBall, new Vector2Int(x, y), 0.12f, true);
                        }
                    }

                    if (y == m_Size.y - 1)
                        _isPinch = true;

                    break;
                }

                if (y == m_Size.y - 1)
                    _isGameOver = true;
            }
        }

        yield return new WaitUntil(() => Board.BallController.IsMoving == 0);


        if (_isPinch)
        {
            foreach (var _ball in Board.CurrentBalls)
            {
                if (!ReferenceEquals(_ball, null))
                {
                    _ball.SetPopSprite();
                    _ball.StartAfraidAnim();
                }
            }
        }

        if (_isGameOver)
            SceneManager.LoadScene(0);
    }
}
