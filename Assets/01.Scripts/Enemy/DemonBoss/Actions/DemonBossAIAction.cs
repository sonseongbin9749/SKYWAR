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
        _demonBrain = _enemyBrain as DemonBossAIBrain; //�θ� ã���ִϱ� Ÿ�Ը� ĳ��Ʈ�ϸ� �ȴ�.
        _phaseData = _enemyBrain.transform.Find("AI").GetComponent<AIDemonBossPhaseData>();
    }
}
