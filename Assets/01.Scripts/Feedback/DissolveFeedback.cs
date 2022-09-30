using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class DissolveFeedback : FeedBack
{
    private SpriteRenderer _spriteRenderer = null;
    [SerializeField]
    private float _duration = 0.05f;

    public UnityEvent DeathCallback;

    private void Awake()
    {
        _spriteRenderer = transform.parent.Find("VisualSprite").GetComponent<SpriteRenderer>();
    }

    public override void CompletePrevFeedBack()
    {
        _spriteRenderer.DOComplete();
        _spriteRenderer.material.DOComplete();
        _spriteRenderer.material.SetFloat("_Dissolve", 1);
    }

    public override void CreateFeedBack()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_spriteRenderer.material.DOFloat(0, "_Dissolve", _duration));
        if(DeathCallback != null)
        {
            seq.AppendCallback(() => DeathCallback.Invoke());
        }
    }
}
