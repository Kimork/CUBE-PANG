using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DownTween : MonoBehaviour
{
    public float InitDelay;
    public float OffsetY;
    public float DurTime;
    public AnimationCurve AnimationCurve;

    private void Awake()
    {
        StartCoroutine(DownTweenRoutine());
    }

    IEnumerator DownTweenRoutine()
    {
        yield return new WaitForSeconds(InitDelay);
        var _targetPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        transform.localPosition = transform.localPosition + new Vector3(0, OffsetY, 0);
        GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

        transform.DOMove(_targetPos, DurTime).SetEase(AnimationCurve);
    }
}
