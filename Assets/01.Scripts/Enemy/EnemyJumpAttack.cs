using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyJumpAttack : EnemyAttack
{
    #region 베지어커브 관련 값들
    [SerializeField]
    private int _bezierResolution = 30; //베지어 커브 상의 점의 갯수 표현
    private Vector3[] _bezierPoints; //베지어 곡선상의 점들
    #endregion

    #region 점프 관련 변수들
    [SerializeField]
    private float _jumpSpeed = 0.9f, _jumpDelay = 0.4f, _impactRadius = 2f;
    //점프 완료까지의 걸리는 시간, 점프를 시작하기전까지의 딜레이시간, 점프로 처맞는 반지름
    private float _frameSpeed = 0; //점프에서 프레임당 걸리는 시간, 계산해서 넣어줄거임.
    #endregion

    public UnityEvent PlayJumpAnimation; //점프 시작 애니메이션 재생 신호
    public UnityEvent PlayLandingAnimation;  //착지 애니메이션 재생 신호

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
        AttackFeedback?.Invoke(); //공격사운드를 재생하고
        yield return new WaitForSeconds(_jumpDelay);
        PlayJumpAnimation?.Invoke(); //점프 애니메이션 재생

        for(int i = 0; i < _bezierPoints.Length; i++)
        {
            yield return new WaitForSeconds(_frameSpeed);
            transform.position = _bezierPoints[i];

            if(i == _bezierPoints.Length - 5)  //바닥 착지 5프레임 전이면
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
        //착지 이펙트를 만들어줘야  하고

        //충격파범위로 플레이거 있는지 검사해서 데미지주고 넉백시
        Vector3 dir = GetTarget().position - basePos;
        
        if(dir.sqrMagnitude <= _impactRadius * _impactRadius)  //충격파 반경안에 있다
        {
            IHittable targetHit = GetTarget().GetComponent<IHittable>();
            targetHit?.GetHit(_enemy.EnemyData.damage, gameObject);

            if(dir.sqrMagnitude == 0) //이경우를 생각해야해
            {
                dir = Random.insideUnitCircle;
            }
            IKnockback targetKnockback = GetTarget().GetComponent<IKnockback>();
            targetKnockback?.Knockback(dir.normalized, _enemy.EnemyData.knockbackPower, 1f);
        }

        // 에너미 브레인에 공격상태 해제해서 공격쿨이 돌아가도록 해야하고
        _enemyBrain.SetAttackState(false);

        StartCoroutine(WaitBeforeAttackCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
