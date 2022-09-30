using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class DemonAttackAction : DemonBossAIAction
{
    [SerializeField]
    private DemonBossAIBrain.AttackType attackType;

    private FieldInfo _info = null;

    protected override void Awake()
    {
        base.Awake();
        _info = typeof(AIDemonBossPhaseData).GetField(attackType.ToString(), BindingFlags.Public | BindingFlags.Instance);
    }

    public override void TakeAction()
    {
        bool check = (bool)_info.GetValue(_phaseData);

        if(check == false && _phaseData.idleTime <= 0)
        {
            _demonBrain.Attack(attackType);
        }
    }
}
