using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : PoolableMono, IAgent, IHittable, IKnockback
{
    [SerializeField] private EnemyDataSO _enemytData;
    public EnemyDataSO EnemyData => _enemytData;

    protected bool _isDead = false;
    protected AgentMovement _agentMovement; //���� �˹�ó���Ϸ��� �̸� �����´�.
    protected EnemyAnimation _enemyAnimation;
    protected EnemyAttack _enemyAttack;
    protected CapsuleCollider2D _collider;

    protected EnemyAIBrain _enemyBrain;

    //�׾����� ó���� �Ͱ�
    //��Ƽ�� ���¸� ������ �ְ�

    #region �������̽� ������
    public int Health { get; private set;}

    [field : SerializeField] public UnityEvent OnDie { get; set; }
    [field : SerializeField] public UnityEvent OnGetHit { get; set; }

    public bool IsEnemy => true;
    public Vector3 HitPoint { get; private set; }

    public virtual void GetHit(int damage, GameObject damageDealer)
    {
        if (_isDead) return;
        //���׾����� ����ٰ� �ǰ� ���� ���� �ۼ�
        float critical = Random.value; // 0 ~ 1 
        bool isCritical = false;

        if(critical <= GameManager.Instance.CriticalChance)
        {
            
            float ratio = Random.Range(GameManager.Instance.CriticalMinDamage, 
                GameManager.Instance.CriticalMaxDamage);
            damage = Mathf.CeilToInt((float)damage * ratio);
            isCritical = true;
        }

        Health -= damage;
        HitPoint = damageDealer.transform.position; //���� ���ȴ°�? 
        //�̰� �˾ƾ� normal�� ����ؼ� �ǰ� Ƣ���� �� �� �ִ�.
        OnGetHit?.Invoke(); //�ǰ� �ǵ�� ���

        //���⿡ ������ ���� ����ִ� ������ ���� �Ѵ�.
        DamagePopup popup = PoolManager.Instance.Pop("DamagePopup") as DamagePopup;
        popup.Setup(damage, transform.position + new Vector3(0,0.5f,0), isCritical, Color.white);


        if(Health <= 0)
        {
            _isDead = true;
            _agentMovement.StopImmediatelly(); //��� ����
            _agentMovement.enabled = false; //�̵��ߴ�
            OnDie?.Invoke(); //��� �̺�Ʈ �κ�ũ
        }
    }
    #endregion

    [SerializeField]
    protected bool _isActive = false;

    private void Awake()
    {
        _agentMovement = GetComponent<AgentMovement>();
        _enemyAnimation = transform.Find("VisualSprite").GetComponent<EnemyAnimation>();
        _enemyAttack = GetComponent<EnemyAttack>();
        _collider = GetComponent<CapsuleCollider2D>();
        _enemyAttack.attackDelay = _enemytData.attackDelay;
        _enemyBrain = GetComponent<EnemyAIBrain>();
    }

    public virtual void PerformAttack()
    {
        if (_isDead == false && _isActive == true)
        {
            //���⿡ �������� ������ ������ �Ŵ�.
            _enemyAttack.Attack(_enemytData.damage);
        }
    }

    public override void Reset()
    {
        Health = _enemytData.maxHealth;
        _collider.enabled = true;
        _isActive = false;
        _isDead = false;
        _agentMovement.enabled = true;
        _enemyAttack.Reset(); //ó�� �����ÿ� ��Ÿ�� �ٽ� ���ư��� 
        //��Ƽ�� ���� �ʱ�ȭ
        //Reset�� ���� �̺�Ʈ ����
        _agentMovement.ResetKnockbackParam(); 
    }

    private void Start()
    {
        Health = _enemytData.maxHealth;
    }

    public void Die()
    {
        //��� �̺�Ʈ �κ�ũ �����ְ�
        //Ǯ�Ŵ����� �־��ְ�
        PoolManager.Instance.Push(this);
    }

    public void SpawnInPortal(Vector3 pos, float power, float time)
    {
        _isActive = false;
        transform.DOJump(pos, power, 1, time).OnComplete(() => _isActive = true);
    }

    public void Knockback(Vector2 direction, float power, float duration)
    {
        if(_isDead == false && _isActive == true)
        {
            if(power > _enemytData.knockbackRegist)
            {
                _agentMovement.Knockback(direction, power, duration);
            }
        }
    }
}
