using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    public Board Board;
    public AnimationCurve DumpSizeCurve;
    public int IsMoving = 0;


    public const float BallDumpAnimTargetHeight = 0.8f;
    public const float BallDumpAnimDurTime_DownScale = 0.12f;
    public const float BallDumpAnimDurTime_UpScale = 0.12f;

    private List<DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>> m_tweens
        = new List<DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>>();

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
        m_tweens.Add(_moveTween);
        await _moveTween.AsyncWaitForCompletion();
        m_tweens.Remove(_moveTween);
        if (playAnim)
        {
            Sequence _dumpAnim = DOTween.Sequence()
                .Append(_target.DOScaleY(BallDumpAnimTargetHeight, BallDumpAnimDurTime_DownScale).SetEase(DumpSizeCurve))
                .Append(_target.DOScaleY(1, BallDumpAnimDurTime_UpScale));
        }

        IsMoving--;
    }

    private void OnDestroy()
    {
        foreach (var _tween in m_tweens)
        {
            if (!ReferenceEquals(_tween, null))
                _tween.Kill();
        }
    }

    private void refreshBallPosInfo(Ball target, Vector2Int dest)
    {
        if (ReferenceEquals(Board.GetBall(target.CurrentPos), target))
            Board.CurrentBalls[target.CurrentPos.x, target.CurrentPos.y] = null;

        target.SetPos(dest);
        Board.CurrentBalls[dest.x, dest.y] = target;
    }
}
