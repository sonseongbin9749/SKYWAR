using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayTimeDecision : DemonBossDecision
{
    public override bool MakeADecision()
    {
        return _phaseData.idleTime <= 0;
    }
}
