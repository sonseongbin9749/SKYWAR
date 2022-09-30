using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNextAttackType : DemonBossDecision
{
    [SerializeField]
    private DemonBossAIBrain.AttackType attackType;

    public override bool MakeADecision()
    {
        return _phaseData.nextAttackType == attackType;
    }
}
