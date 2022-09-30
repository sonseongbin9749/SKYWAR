using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackStunAnimation : FeedBack
{
    [SerializeField]
    private float _framePerTime = 12;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Sprite[] _sprites;

    private void Awake()
    {
        _spriteRenderer = transform.Find("VisualIcon").GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }

    public override void CompletePrevFeedBack()
    {
        StopAllCoroutines();
        _spriteRenderer.enabled = false;
    }

    public override void CreateFeedBack()
    {
        _spriteRenderer.enabled = true;
        StartCoroutine(StunAnimationCoroutine());
    }

    IEnumerator StunAnimationCoroutine()
    {
        WaitForSeconds ws = new WaitForSeconds(1f / _framePerTime);

        int idx = 0;
        while(true)
        {
            yield return ws;
            _spriteRenderer.sprite = _sprites[idx];
            idx = (idx + 1) % _sprites.Length;
        }
    }
}
