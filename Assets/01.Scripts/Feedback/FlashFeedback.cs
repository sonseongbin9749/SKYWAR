using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFeedback : FeedBack
{
    private SpriteRenderer _spriteRenderer = null;
    [SerializeField]
    private float _flashTime = 0.1f;
    [SerializeField]
    private Material _flashMat = null;

    private Shader _originalMatShader;

    private void Awake()
    {
        _spriteRenderer = transform.parent.Find("VisualSprite").GetComponent<SpriteRenderer>();
        _originalMatShader = _spriteRenderer.material.shader; //���� ���̴��� �����صΰ�
    }

    public override void CompletePrevFeedBack()
    {
        StopAllCoroutines();
        _spriteRenderer.material.SetInt("_MakeSolidColor", 0);
        _spriteRenderer.material.shader = _originalMatShader;
    }

    public override void CreateFeedBack()
    {
        if(_spriteRenderer.material.HasProperty("_MakeSolidColor") == false)
        {
            _spriteRenderer.material.shader = _flashMat.shader; //���� ��Ƽ����� ��ü
        }

        _spriteRenderer.material.SetInt("_MakeSolidColor", 1);
        StartCoroutine(WaitBeforeChangingBack());
    }

    IEnumerator WaitBeforeChangingBack()
    {
        yield return new WaitForSeconds(_flashTime);
        //������ �־��µ� �����׿�..
        if(_spriteRenderer.material.HasProperty("_MakeSolidColor") == true)
        {
            _spriteRenderer.material.SetInt("_MakeSolidColor", 0);
        }
        else
        {
            _spriteRenderer.material.shader = _originalMatShader;
        }
    }
}
