using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeFeedback : FeedBack
{
    [SerializeField] private GameObject _objectToShake;
    [SerializeField] private float _duration = 0.2f, _strength = 1f, _randomness = 90;
    [SerializeField] private int _vibrato = 10;
    [SerializeField] private bool _snapping = false, _fadeOut = true;
    //snapping : 격자단위로 떨리게 하는게, fadeOut은 진동후에 원래자리로 돌아가냐?

    public override void CompletePrevFeedBack()
    {
        _objectToShake.transform.DOComplete();
        // 모든트윈을 즉시 완료시키고 완료된 트윈의 갯수를 반환해
    }

    public override void CreateFeedBack()
    {
        CompletePrevFeedBack();
        _objectToShake.transform.DOShakePosition(
            _duration, _strength, _vibrato, _randomness, _snapping, _fadeOut);
    }
}
