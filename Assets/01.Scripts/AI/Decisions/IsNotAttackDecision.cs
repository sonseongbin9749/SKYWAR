using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsNotAttackDecision : AIDecision
{
    public override bool MakeADecision()
    {
        return !_aiActionData.attack;
    }
}
