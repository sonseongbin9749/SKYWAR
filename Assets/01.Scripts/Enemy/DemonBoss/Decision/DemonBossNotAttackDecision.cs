using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBossNotAttackDecision : DemonBossDecision
{
    public override bool MakeADecision()
    {
        return _phaseData.CanAttack;
    }
}
