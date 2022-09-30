using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DemonBossAIAction : AIAction
{
    protected AIDemonBossPhaseData _phaseData;
    protected DemonBossAIBrain _demonBrain;

    protected override void Awake()
    {
        base.Awake();
        _demonBrain = _enemyBrain as DemonBossAIBrain; //부모가 찾아주니까 타입만 캐스트하면 된다.
        _phaseData = _enemyBrain.transform.Find("AI").GetComponent<AIDemonBossPhaseData>();
    }
}
