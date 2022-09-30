using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : RegularBullet
{
    protected Collider2D _collider;

    private bool _charging = false; //��¡�� �Ѿ����� 

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
            _collider.enabled = !_charging; //��¡�� �ƴ� �Ѿ��� �ٷ� Ȱ��ȭ,
        }
    }

    //�������� �߻�
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
            return; //�Ǿƽĺ�
        }

        //�Ǿư� �ٸ��ٸ� ���� ����
        
        //���� ����Ʈ ����ϰ�
        Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
        ImpactScript impact = PoolManager.Instance.Pop(_bulletData.impactEnemyPrefab.name) as ImpactScript;
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 359f)));
        impact.SetPositionAndRotation(transform.position + (Vector3)randomOffset, rot);

        //�ݰ泻�� ���� �ִٸ� �������� �ְ� �˹�
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
