using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFreezeFeedback : FeedBack
{
    [SerializeField]
    private float _freezeTimeDelay = 0.05f, _unFreezeTimeDelay = 0.02f;

    [SerializeField]
    [Range(0, 1f)]
    private float _timeFreezeValue = 0.2f;

    public override void CompletePrevFeedBack()
    {
        if(TimeController.instance != null)
            TimeController.instance.ResetTimeScale();
    }

    public override void CreateFeedBack()
    {
        TimeController.instance.ModifyTimeScale(_timeFreezeValue, _freezeTimeDelay, () =>
        {
            TimeController.instance.ModifyTimeScale(1, _unFreezeTimeDelay);
        }); 
    }
}
