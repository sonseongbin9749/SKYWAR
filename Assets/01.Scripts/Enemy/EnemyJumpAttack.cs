using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyJumpAttack : EnemyAttack
{
    #region ������Ŀ�� ���� ����
    [SerializeField]
    private int _bezierResolution = 30; //������ Ŀ�� ���� ���� ���� ǥ��
    private Vector3[] _bezierPoints; //������ ����� ����
    #endregion

    #region ���� ���� ������
    [SerializeField]
    private float _jumpSpeed = 0.9f, _jumpDelay = 0.4f, _impactRadius = 2f;
    //���� �Ϸ������ �ɸ��� �ð�, ������ �����ϱ��������� �����̽ð�, ������ ó�´� ������
    private float _frameSpeed = 0; //�������� �����Ӵ� �ɸ��� �ð�, ����ؼ� �־��ٰ���.
    #endregion

    public UnityEvent PlayJumpAnimation; //���� ���� �ִϸ��̼� ��� ��ȣ
    public UnityEvent PlayLandingAnimation;  //���� �ִϸ��̼� ��� ��ȣ

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Attack(2);
        }
    }

    public override void Attack(int damage)
    {
        if(_waitBeforeNextAttack == false)
        {
            _enemyBrain.SetAttackState(true);

            Jump();
        }
    }

    private void Jump()
    {
        _waitBeforeNextAttack = true;
        Vector3 deltaPos = transform.position - _enemyBrain.basePosition.position;
        Vector3 targetPos = GetTarget().position + deltaPos;
        Vector3 startControl = (targetPos - transform.position) / 4;

        float angle = targetPos.x - transform.position.x < 0 ? -45f : 45f;

        Vector3 cp1 = Quaternion.Euler(0, 0, angle) * startControl;
        Vector3 cp2 = Quaternion.Euler(0, 0, angle) * (startControl * 3);

        _bezierPoints = DOCurve.CubicBezier.GetSegmentPointCloud(
            transform.position, transform.position + cp1, 
            targetPos, transform.position + cp2, _bezierResolution);
        _frameSpeed = _jumpSpeed / _bezierResolution;
        StartCoroutine(JumpCoroutine());
    }

    IEnumerator JumpCoroutine()
    {
        AttackFeedback?.Invoke(); //���ݻ��带 ����ϰ�
        yield return new WaitForSeconds(_jumpDelay);
        PlayJumpAnimation?.Invoke(); //���� �ִϸ��̼� ���

        for(int i = 0; i < _bezierPoints.Length; i++)
        {
            yield return new WaitForSeconds(_frameSpeed);
            transform.position = _bezierPoints[i];

            if(i == _bezierPoints.Length - 5)  //�ٴ� ���� 5������ ���̸�
            {
                EdgeOfEndAnimation();
            }
        }

        JumpEnd();
    }

    private void EdgeOfEndAnimation()
    {
        PlayLandingAnimation?.Invoke();
    }

    private void JumpEnd()
    {
        Vector3 basePos = _enemyBrain.basePosition.position;
        //���� ����Ʈ�� ��������  �ϰ�

        //����Ĺ����� �÷��̰� �ִ��� �˻��ؼ� �������ְ� �˹��
        Vector3 dir = GetTarget().position - basePos;
        
        if(dir.sqrMagnitude <= _impactRadius * _impactRadius)  //����� �ݰ�ȿ� �ִ�
        {
            IHittable targetHit = GetTarget().GetComponent<IHittable>();
            targetHit?.GetHit(_enemy.EnemyData.damage, gameObject);

            if(dir.sqrMagnitude == 0) //�̰�츦 �����ؾ���
            {
                dir = Random.insideUnitCircle;
            }
            IKnockback targetKnockback = GetTarget().GetComponent<IKnockback>();
            targetKnockback?.Knockback(dir.normalized, _enemy.EnemyData.knockbackPower, 1f);
        }

        // ���ʹ� �극�ο� ���ݻ��� �����ؼ� �������� ���ư����� �ؾ��ϰ�
        _enemyBrain.SetAttackState(false);

        StartCoroutine(WaitBeforeAttackCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
