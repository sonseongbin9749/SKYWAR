using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeutralEnemy : Enemy
{
    protected bool _isNeutral = false; //무력상태가 되면 true
    protected int _accumulateDamage = 0; //누적데미지

    [SerializeField]
    private int _neutralDamage = 10; //무력상태로 빠지기 위해 넣어야 하는 데미지
    [SerializeField]
    private float _neutralTime = 3f; //무력화에 빠진 시간

    public UnityEvent OnNeutralHit = null; //무력상태에서 처맞을 때 발생하는 일
    public UnityEvent OnEnterNeutral = null; //무력상태로 들어갈 때 발생하는 일
    public UnityEvent OnExitNeutral = null; //무력 상태를 벗어났을 때 발생하는 일

    public override void PerformAttack()
    {
        if (_isNeutral)
            return;
        base.PerformAttack();
    }

    public override void Reset()
    {
        base.Reset();
        _accumulateDamage = 0;
        _isNeutral = false;
    }

    public override void GetHit(int damage, GameObject damageDealer)
    {
        //공중에 떠있을때는 데미지 안받도록 함.
        if (_isDead || _enemyBrain.AIActionData.attack) return;

        if(_isNeutral)
        {
            OnNeutralHit?.Invoke();
        }
        else
        {
            base.GetHit(damage, damageDealer);
            _accumulateDamage += damage;
        }

        if(_isNeutral == false && _accumulateDamage >= _neutralDamage)
        {
            _accumulateDamage = 0;
            _isNeutral = true;
            OnEnterNeutral?.Invoke();
            StartCoroutine(NeutralCoroutine());
        }
    }

    IEnumerator NeutralCoroutine()
    {
        yield return new WaitForSeconds(_neutralTime);
        _isNeutral = false;
        OnExitNeutral?.Invoke();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

