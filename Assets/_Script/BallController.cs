using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    public Board Board;
    public AnimationCurve DumpSizeCurve;
    public int IsMoving = 0;

    private void Awake()
    {
        Board = (Board)GetComponent("Board");
        Board.BallController = this;
    }

    public async void MoveBall(Ball target, Vector2Int dest, float speed, bool playAnim = false)
    {
        IsMoving++;

        refreshBallPosInfo(target, dest);

        Transform _target = target.transform;
        float _distance = _target.localPosition.y - dest.y;
        //float _calTime = _distance * speed;


        var _moveTween = _target.DOMove(new Vector3(dest.x, dest.y, 0), speed);
        await _moveTween.AsyncWaitForCompletion();

        if (playAnim)
        {
            Sequence _dumpAnim = DOTween.Sequence()
                .Append(_target.DOScaleY(0.8f, 0.12f).SetEase(DumpSizeCurve))
                .Append(_target.DOScaleY(1, 0.12f));
        }

        IsMoving--;
    }

    private void refreshBallPosInfo(Ball target, Vector2Int dest)
    {
        if (ReferenceEquals(Board.GetBall(target.CurrentPos), target))
            Board.CurrentBalls[target.CurrentPos.x, target.CurrentPos.y] = null;

        target.SetPos(dest);
        Board.CurrentBalls[dest.x, dest.y] = target;
    }
}
