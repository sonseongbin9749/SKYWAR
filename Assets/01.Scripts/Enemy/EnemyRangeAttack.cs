using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyRangeAttack : EnemyAttack
{
    [SerializeField] private BulletDataSO _bulletData;
    [SerializeField] private Transform _firePos;


    public override void Attack(int damage)
    {
        
        if(_waitBeforeNextAttack == false)
        {
            _enemyBrain.SetAttackState(true); //공격시작으로 셋팅
            AttackFeedback?.Invoke(); 

            Transform target = GetTarget();
            
            Vector2 aimDirection = target.position - _firePos.position;
            float desireAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            Quaternion rot = Quaternion.AngleAxis(desireAngle, Vector3.forward);

            SpawnBullet(_firePos.position, rot, true, damage);
            StartCoroutine(WaitBeforeAttackCoroutine());
        }
    }

    private void SpawnBullet(Vector3 pos, Quaternion rot, bool isEnemyBullet, int damage)
    {
        Bullet b = PoolManager.Instance.Pop(_bulletData.prefab.name) as Bullet;
        b.SetPositionAndRotation(pos, rot);
        b.IsEnemy = isEnemyBullet;
        b.BulletData = _bulletData;
        b.damageFactor = damage;
    }
}
