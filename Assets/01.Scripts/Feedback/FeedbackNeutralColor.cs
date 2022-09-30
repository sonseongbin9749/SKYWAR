using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackNeutralColor : FeedBack
{
    private SpriteRenderer _spriteRenderer = null;
    [SerializeField]
    private Material _neutralMat = null;
    private Shader _originalMatShader = null;

    private void Awake()
    {
        _spriteRenderer = transform.parent.Find("VisualSprite").GetComponent<SpriteRenderer>();
        _originalMatShader = _spriteRenderer.material.shader;
    }

    public override void CompletePrevFeedBack()
    {
        StopAllCoroutines();
        _spriteRenderer.material.SetInt("_MakeNeutralColor", 0);
        _spriteRenderer.material.shader = _originalMatShader;
    }

    public override void CreateFeedBack()
    {
        if(_spriteRenderer.material.HasProperty("_MakeNeutralColor") == false)
        {
            _spriteRenderer.material.shader = _neutralMat.shader;
        }

        _spriteRenderer.material.SetInt("_MakeNeutralColor", 1);
    }
}
