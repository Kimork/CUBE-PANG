using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AlertLine : MonoBehaviour
{
    public float DurTime;
    private SpriteRenderer m_SpriteRenderer;
    private Sequence m_Sequence;

    private void Awake()
    {
        m_SpriteRenderer = (SpriteRenderer)GetComponent("SpriteRenderer");
    }

    private void OnEnable()
    {
        m_SpriteRenderer.color = new Color(1, 1, 1, 0);
        m_Sequence = DOTween.Sequence()
            .Append(m_SpriteRenderer.DOFade(1, DurTime / 2f))
            .Append(m_SpriteRenderer.DOFade(0, DurTime / 2f))
            .SetLoops(-1);
    }

    private void OnDisable()
    {
        m_Sequence.Kill();
    }
}
