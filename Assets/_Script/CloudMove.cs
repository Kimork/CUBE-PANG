using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CloudMove : MonoBehaviour
{
    public Transform[] Clouds;
    public int StartX;
    public int EndX;
    public float DurTime;
    public AnimationCurve MoveCurve;

    private DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>[] m_tweens;

    private void Start()
    {
        m_tweens = new DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>[Clouds.Length];
        for (int i = 0; i < Clouds.Length; i++)
        {
            MoveClouds(i);
        }
    }

    public void MoveClouds(int i)
    {
        var _cloud = Clouds[i];

        var _calRestTime = DurTime * ((EndX - _cloud.localPosition.x) / ((float)EndX - StartX));

        var _tween = _cloud.DOMoveX(EndX, _calRestTime).SetEase(MoveCurve);
        _tween.SetAutoKill();
        _tween.onComplete +=
        () =>
        {
            _cloud.localPosition = new Vector3(StartX, _cloud.localPosition.y, 0);
            MoveClouds(i);
        };

        m_tweens[i] = _tween;
    }

    private void OnDestroy()
    {
        foreach (var _tween in m_tweens)
        {
            if (!ReferenceEquals(_tween, null))
                _tween.Kill();
        }
    }
}
