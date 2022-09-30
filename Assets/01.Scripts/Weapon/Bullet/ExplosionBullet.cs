using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : RegularBullet
{
    protected Collider2D _collider;

    private bool _charging = false; //차징형 총알인지 

    public override BulletDataSO BulletData { 
        get => _bulletData;
        set
        {
            base.BulletData = _bulletData;

            if(_collider == null)
            {
                _collider = GetComponent<Collider2D>();
            }
            _charging = _bulletData.isCharging;
            _collider.enabled = !_charging; //차징이 아닌 총알은 바로 활성화,
        }
    }

    //실제적인 발사
    public void StartFire()
    {
        _collider.enabled = true;
        _charging = false;
    }

    protected override void FixedUpdate()
    {
        if (_charging) return;

        base.FixedUpdate();
    }

    public override void Reset()
    {
        base.Reset();
        if (_collider != null)
            _collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isDead) return;

        IHittable hittable = collision.GetComponent<IHittable>();

        if(hittable != null && hittable.IsEnemy == IsEnemy)
        {
            return; //피아식별
        }

        //피아가 다르다면 폭발 시작
        
        //폭발 이펙트 재생하고
        Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
        ImpactScript impact = PoolManager.Instance.Pop(_bulletData.impactEnemyPrefab.name) as ImpactScript;
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 359f)));
        impact.SetPositionAndRotation(transform.position + (Vector3)randomOffset, rot);

        //반경내에 적이 있다면 데미지를 주고 넉백
        LayerMask enemyLayer = 1 << _enemyLayer;
        Collider2D[] enemyArr = Physics2D.OverlapCircleAll(
            transform.position, _bulletData.explosionRadius, enemyLayer);
        foreach(Collider2D e in enemyArr)
        {
            IHittable hit = e.GetComponent<IHittable>();
            hit?.GetHit(_bulletData.damage * damageFactor, gameObject);

            IKnockback ikb = e.GetComponent<IKnockback>();
            Vector3 kbDir = (e.transform.position - transform.position).normalized;
            ikb?.Knockback(kbDir, _bulletData.knockBackPower, _bulletData.knockBackDelay);
        }

        _isDead = true;
        PoolManager.Instance.Push(this);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject && _bulletData != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _bulletData.explosionRadius);
            Gizmos.color = Color.white;
        }
    }
#endif

}
