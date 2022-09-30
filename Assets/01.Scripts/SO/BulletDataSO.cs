using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon/BulletData")]
public class BulletDataSO : ScriptableObject
{
    public GameObject prefab;
    [Range(1, 10)] public int damage = 1;
    [Range(1, 100f)] public float bulletSpeed = 1;

    [Range(0, 5f)] public float explosionRadius = 3f; //폭발반경

    public Sprite sprite;
    public RuntimeAnimatorController animatorController;

    public float friction = 0f;

    //미구현이지만 아마 강의때 할 수 있을 듯
    public bool bounce = false; //총알이 벽에 부딛혔을 때 튕길 것인가.
    public bool goThroughHit = false; //관통할 것인가.
    public bool isRayCast = false; //레이캐스트 총알인가 
    public bool isCharging = false; //차징 불렛이냐?

    public GameObject impactObstaclePrefab; //장애물에 부딛혔을때 나올 이펙트
    public GameObject impactEnemyPrefab; //적에 부딛혔을때 나올 이펙트

    [Range(1, 20)] public float knockBackPower = 5f;
    [Range(0.01f, 1f)] public float knockBackDelay = 0.1f;

    public Material bulletMat;
    
    public float lifeTime = 2f; //총알의 생존 시간

}
