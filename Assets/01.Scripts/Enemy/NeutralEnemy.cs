using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeutralEnemy : Enemy
{
    protected bool _isNeutral = false; //���»��°� �Ǹ� true
    protected int _accumulateDamage = 0; //����������

    [SerializeField]
    private int _neutralDamage = 10; //���»��·� ������ ���� �־�� �ϴ� ������
    [SerializeField]
    private float _neutralTime = 3f; //����ȭ�� ���� �ð�

    public UnityEvent OnNeutralHit = null; //���»��¿��� ó���� �� �߻��ϴ� ��
    public UnityEvent OnEnterNeutral = null; //���»��·� �� �� �߻��ϴ� ��
    public UnityEvent OnExitNeutral = null; //���� ���¸� ����� �� �߻��ϴ� ��

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
        //���߿� ���������� ������ �ȹ޵��� ��.
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

