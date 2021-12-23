using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum MatchValue : int
{
    Beaver = 0,
    Cat = 1,
    Dog = 2,
    Donkey = 3,
    Elephant = 4,
    Gorilla = 5,
    Monkey = 6,
    Orca = 7,
    Panda = 8,
    Penguin = 9,
    Pig = 10,
    Rhino = 11,
    Snake = 12,
    Zebra = 13
}

public class Ball : MonoBehaviour, IPoolAble
{
    public Sprite[] BallSprites;
    public Sprite[] BallPopSprites;
    public MatchValue MatchColor { get; private set; }
    public Vector2Int CurrentPos { get; private set; }
    public bool IsPopSprite = false;
    public bool IsPlayAfraid = false;

    private SpriteRenderer m_SpriteRenderer;
    private int m_PoolIndex;
    private Sequence m_AfraidSequence;

    public const float BallAfraidAnimStrength = 0.04f;
    public const float BallAfraidAnimDurTime = 0.07f;

    public void SetColor(MatchValue color)
    {
        if (ReferenceEquals(m_SpriteRenderer, null))
            m_SpriteRenderer = (SpriteRenderer)GetComponent("SpriteRenderer");

        MatchColor = color;

        SetNormalSprite();
    }

    public void SetPopSprite()
    {
        m_SpriteRenderer.sprite = BallPopSprites[(int)MatchColor];
        IsPopSprite = true;
    }

    public void StartAfraidAnim()
    {
        m_AfraidSequence = DOTween.Sequence()
            .Append(transform.DOLocalMove(new Vector3(CurrentPos.x + BallAfraidAnimStrength, CurrentPos.y, 0), BallAfraidAnimDurTime))
            .Append(transform.DOLocalMove(new Vector3(CurrentPos.x - BallAfraidAnimStrength, CurrentPos.y, 0), BallAfraidAnimDurTime))
            .SetLoops(-1);

        IsPlayAfraid = true;
    }

    private void OnDestroy()
    {
        if (!ReferenceEquals(m_AfraidSequence, null))
            m_AfraidSequence.Kill();
    }

    public void StopAfraidAnim()
    {
        m_AfraidSequence.Kill();
        transform.localPosition = new Vector3(CurrentPos.x, CurrentPos.y, 0);

        IsPlayAfraid = false;
    }

    public void SetNormalSprite()
    {
        m_SpriteRenderer.sprite = BallSprites[(int)MatchColor];
        IsPopSprite = false;
    }


    public void SetPos(Vector2Int pos)
    {
        CurrentPos = pos;
    }

    public int GetIndex()
    {
        return m_PoolIndex;
    }

    public void SetIndex(int index)
    {
        m_PoolIndex = index;
    }
}
