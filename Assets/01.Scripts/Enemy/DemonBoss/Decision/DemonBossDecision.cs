using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DemonBossDecision : AIDecision
{
    protected AIDemonBossPhaseData _phaseData;
    protected DemonBossAIBrain _demonBrain;

   

    protected override void Awake()
    {
        base.Awake();
        _demonBrain = _enemyBrain as DemonBossAIBrain;
        _phaseData = _enemyBrain.transform.Find("AI").GetComponent<AIDemonBossPhaseData>();
    }

}
