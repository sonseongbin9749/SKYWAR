using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Hand : MonoBehaviour, IHittable
{

    private Animator _animator;
    private readonly int _hashFadeIn = Animator.StringToHash("FadeIn");
    private readonly int _hashShockAttack = Animator.StringToHash("ShockAttack");
    private readonly int _hashFlapperAttack = Animator.StringToHash("FlapperAttack");

    private Vector3 _initPosition; //맨 처음에 팔이 달려있었던 위치를 기억하게 한다.

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2d;

    public bool IsEnemy => true;

    public Vector3 HitPoint { get; set; }

    #region 공격관련 멤버변수들
    [SerializeField] private Transform _attackPosTrm;
    private LayerMask _whatIsEnemy;

    [SerializeField] private int _damage;
    [SerializeField] private float _atkRadius = 3f, _knockPower, _flapPower, _flapDistance;
    [SerializeField] private bool _isLeftHand, _isFlapping; //파리채 공격중일때 사용할 예정

    Sequence seq = null;
    public UnityEvent OnAttack = null;
    public UnityEvent OnFlap = null;
    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _attackPosTrm = transform.Find("AttackPoint");
        _initPosition = transform.position;
        _whatIsEnemy = 1 << LayerMask.NameToLayer("Player");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
    }

    #region 쇼크웨이브 공격
    public void AttackShockSequence(Vector3 targetPos, Action Callback)
    {
        Vector3 atkPos = targetPos - _attackPosTrm.localPosition;

        seq = DOTween.Sequence();
        //요기에 주먹 뻗기전에 주먹을 부르르 떤다든지 하는 전조증상을 넣어도 된다.
        seq.Append( transform.DOMove(_initPosition + new Vector3(0, 0.5f), 0.2f) );
        seq.Join(transform.DOScale(1.2f, 0.2f)); //크기도 증가하게
        seq.Append(transform.DOMove(atkPos + new Vector3(0, 0.5f), 0.7f));
        seq.AppendCallback(() =>
        {
            _animator.SetTrigger(_hashShockAttack);
        });
        seq.Join(transform.DOMove(atkPos, 0.2f));
        seq.AppendCallback(() =>
        {
            ActiveShock();
        });
        seq.AppendInterval(1f);
        seq.Append(transform.DOMove(_initPosition, 0.3f));
        seq.AppendCallback(() => Callback?.Invoke());
    }

    private void ActiveShock()
    {
        OnAttack?.Invoke(); //바닥을 찍을 때 피드백

        ImpactScript impact = PoolManager.Instance.Pop("ImpactShockwave") as ImpactScript;
        impact.SetPositionAndRotation(_attackPosTrm.position, Quaternion.identity);
        impact.SetLocalScale(Vector3.one * 1.4f);

        Collider2D col = Physics2D.OverlapCircle(_attackPosTrm.position, 
                                            _atkRadius, _whatIsEnemy);

        if(col != null)
        {
            IHittable iHit = col.GetComponent<IHittable>();
            iHit.GetHit(_damage, gameObject);

            Vector3 dir = col.transform.position - _attackPosTrm.position;
            IKnockback iKnock = col.GetComponent<IKnockback>();
            if(dir.sqrMagnitude == 0)
            {
                dir = Random.insideUnitCircle;
            }

            iKnock?.Knockback(dir.normalized, _knockPower, 1f);
        }
    }

    #endregion

    #region 파리채 공격

    public void AttackFlapperSequence(Vector3 targetPos, Action Callback)
    {
        Vector3 atkPos = targetPos - _attackPosTrm.localPosition; //델타만큼 빼준다.
        seq = DOTween.Sequence();

        seq.Append(transform.DOMoveY(atkPos.y, 0.4f));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() =>
        {
            _animator.SetTrigger(_hashFlapperAttack);//손바닥 애니메이션 재생
            _isFlapping = true; //휘두르기 시작
            OnFlap?.Invoke(); //휘두르는 사운드 등 피드백 재생
        });

        float x = _isLeftHand ? -1f : 1f;
        float targetX = transform.position.x + x * _flapDistance;
        seq.Join(transform.DOMoveX(targetX, 0.7f));
        seq.AppendInterval(0.3f);
        seq.AppendCallback(() =>
        {
            _isFlapping = false;  //휘두르는거 끝
        });
        seq.Join(transform.DOMove(_initPosition, 0.3f));
        seq.AppendCallback(() => Callback?.Invoke());
    }

    #endregion
    private void OnDisable()
    {
        seq?.Kill();
        SetDeadParam();
    }

    public void SetDeadParam()
    {
        gameObject.SetActive(false);
        transform.SetPositionAndRotation(_initPosition, Quaternion.identity);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            AttackShockSequence(Vector3.zero, null);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AttackFlapperSequence(Vector3.zero, null);
        }
    }


    public void GetHit(int damage, GameObject damageDealer)
    {
        
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_attackPosTrm.position, _atkRadius);
        Gizmos.color = Color.white;
    }
#endif
}
