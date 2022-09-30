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

    private Vector3 _initPosition; //�� ó���� ���� �޷��־��� ��ġ�� ����ϰ� �Ѵ�.

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2d;

    public bool IsEnemy => true;

    public Vector3 HitPoint { get; set; }

    #region ���ݰ��� ���������
    [SerializeField] private Transform _attackPosTrm;
    private LayerMask _whatIsEnemy;

    [SerializeField] private int _damage;
    [SerializeField] private float _atkRadius = 3f, _knockPower, _flapPower, _flapDistance;
    [SerializeField] private bool _isLeftHand, _isFlapping; //�ĸ�ä �������϶� ����� ����

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

    #region ��ũ���̺� ����
    public void AttackShockSequence(Vector3 targetPos, Action Callback)
    {
        Vector3 atkPos = targetPos - _attackPosTrm.localPosition;

        seq = DOTween.Sequence();
        //��⿡ �ָ� �������� �ָ��� �θ��� ���ٵ��� �ϴ� ���������� �־ �ȴ�.
        seq.Append( transform.DOMove(_initPosition + new Vector3(0, 0.5f), 0.2f) );
        seq.Join(transform.DOScale(1.2f, 0.2f)); //ũ�⵵ �����ϰ�
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
        OnAttack?.Invoke(); //�ٴ��� ���� �� �ǵ��

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

    #region �ĸ�ä ����

    public void AttackFlapperSequence(Vector3 targetPos, Action Callback)
    {
        Vector3 atkPos = targetPos - _attackPosTrm.localPosition; //��Ÿ��ŭ ���ش�.
        seq = DOTween.Sequence();

        seq.Append(transform.DOMoveY(atkPos.y, 0.4f));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() =>
        {
            _animator.SetTrigger(_hashFlapperAttack);//�չٴ� �ִϸ��̼� ���
            _isFlapping = true; //�ֵθ��� ����
            OnFlap?.Invoke(); //�ֵθ��� ���� �� �ǵ�� ���
        });

        float x = _isLeftHand ? -1f : 1f;
        float targetX = transform.position.x + x * _flapDistance;
        seq.Join(transform.DOMoveX(targetX, 0.7f));
        seq.AppendInterval(0.3f);
        seq.AppendCallback(() =>
        {
            _isFlapping = false;  //�ֵθ��°� ��
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
