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
    protected AgentMovement _agentMovement; //차후 넉백처리하려고 미리 가져온다.
    protected EnemyAnimation _enemyAnimation;
    protected EnemyAttack _enemyAttack;
    protected CapsuleCollider2D _collider;

    protected EnemyAIBrain _enemyBrain;

    //죽었을때 처리할 것과
    //액티브 상태를 관리할 애가

    #region 인터페이스 구현부
    public int Health { get; private set;}

    [field : SerializeField] public UnityEvent OnDie { get; set; }
    [field : SerializeField] public UnityEvent OnGetHit { get; set; }

    public bool IsEnemy => true;
    public Vector3 HitPoint { get; private set; }

    public virtual void GetHit(int damage, GameObject damageDealer)
    {
        if (_isDead) return;
        //안죽었으면 여기다가 피격 관련 로직 작성
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
        HitPoint = damageDealer.transform.position; //누가 때렸는가? 
        //이걸 알아야 normal을 계산해서 피가 튀도록 할 수 있다.
        OnGetHit?.Invoke(); //피격 피드백 재생

        //여기에 데미지 숫자 띄워주는 로직이 들어가야 한다.
        DamagePopup popup = PoolManager.Instance.Pop("DamagePopup") as DamagePopup;
        popup.Setup(damage, transform.position + new Vector3(0,0.5f,0), isCritical, Color.white);


        if(Health <= 0)
        {
            _isDead = true;
            _agentMovement.StopImmediatelly(); //즉시 정지
            _agentMovement.enabled = false; //이동중단
            OnDie?.Invoke(); //사망 이벤트 인보크
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
            //여기에 실제적인 공격을 수행할 거다.
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
        _enemyAttack.Reset(); //처음 생성시에 쿨타임 다시 돌아가게 
        //액티브 상태 초기화
        //Reset에 대한 이벤트 발행
        _agentMovement.ResetKnockbackParam(); 
    }

    private void Start()
    {
        Health = _enemytData.maxHealth;
    }

    public void Die()
    {
        //사망 이벤트 인보크 시켜주고
        //풀매니저에 넣어주고
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
