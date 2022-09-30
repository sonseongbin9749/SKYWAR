using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBossIdleAction : DemonBossAIAction
{
    public override void TakeAction()
    {
        if(_phaseData.idleTime > 0)
        {
            _phaseData.idleTime -= Time.deltaTime;
            if (_phaseData.idleTime < 0)
                _phaseData.idleTime = 0;
        }
    }
}
