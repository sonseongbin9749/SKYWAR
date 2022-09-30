using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class WeaponTooltip : PoolableMono
{
    private static int DefaultSortingOrder = 20;

    private TextMeshPro _atkText;
    private TextMeshPro _ammoText;
    private TextMeshPro _delayText;
    private TextMeshPro _consumeText;

    private SpriteRenderer _panelSprite;

    private List<SpriteRenderer> _childSprite = new List<SpriteRenderer>();
    private List<TextMeshPro> _childText = new List<TextMeshPro>();

    public override void Reset()
    {
        transform.localScale = new Vector3(1, 0, 1);
        SetSortingOrder(0);
    }

    private void Awake()
    {
        _atkText = transform.Find("ATKRow/ValueText").GetComponent<TextMeshPro>();
        _childText.Add(_atkText);
        _ammoText = transform.Find("AmmoRow/ValueText").GetComponent<TextMeshPro>();
        _childText.Add(_ammoText);
        _delayText = transform.Find("DelayRow/ValueText").GetComponent<TextMeshPro>();
        _childText.Add(_delayText);
        _consumeText = transform.Find("ConsumeRow/ValueText").GetComponent<TextMeshPro>();
        _childText.Add(_consumeText);

        _panelSprite = GetComponent<SpriteRenderer>();

        GetComponentsInChildren<SpriteRenderer>(_childSprite);
        _childSprite.RemoveAt(0);
    }

    public void SetText(WeaponDataSO data)
    {
        _atkText.SetText(data.damageFactor.ToString());
        _ammoText.SetText(data.ammoCapacity.ToString());
        _delayText.SetText(data.weaponDelay.ToString());
        _consumeText.SetText(data.GetBulletCountToSpawn().ToString());
    }

    public void PopupTooltip(Vector3 worldPos, int order)
    {
        transform.localScale = new Vector3(1, 0, 1);
        worldPos.y += 0.5f;
        transform.position = worldPos;
        SetSortingOrder(order);
        Open();
    }

    private void SetSortingOrder(int order)
    {
        _panelSprite.sortingOrder = DefaultSortingOrder + order;
        _childSprite.ForEach(x => x.sortingOrder = DefaultSortingOrder + order + 1);
        _childText.ForEach(x => x.sortingOrder = DefaultSortingOrder + order + 1);
    }
    
    private void Open()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleY(1.2f, 0.3f));
        seq.Append(transform.DOScaleY(0.9f, 0.1f));
        seq.Append(transform.DOScaleY(1f, 0.1f));
    }

    public void CloseTooltip()
    {
        Close();
    }

    private void Close()
    {
        DOTween.Kill(transform); //현재 transform에 있는 모든 트윈을 제거한다.
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleY(1.2f, 0.1f));
        seq.Append(transform.DOScaleY(0f, 0.3f));
        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(this);
        });
    }
    
}
