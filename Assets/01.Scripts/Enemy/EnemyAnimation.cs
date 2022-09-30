using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : AgentAnimation
{
    protected EnemyAIBrain _enemyAIBrain;
    protected readonly int _attackHashStr = Animator.StringToHash("Attack");
    protected readonly int _jumpdownHashStr = Animator.StringToHash("Jumpdown");

    //나중에 공격애니메이션이 있는 적들은 만들 예정

    protected override void ChildAwake()
    {
        _enemyAIBrain = transform.parent.GetComponent<EnemyAIBrain>();
    }

    public void SetEndOfAttackAnimation()
    {
        _enemyAIBrain.SetAttackState(false);
    }

    public void PlayAttackAnimation()
    {
        _agentAnimator.SetTrigger(_attackHashStr);
    }

    public void PlayJumpdownAnimation()
    {
        _agentAnimator.SetTrigger(_jumpdownHashStr);
    }
}
