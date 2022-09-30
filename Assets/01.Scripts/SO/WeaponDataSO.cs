using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon/WeaponData")]
public class WeaponDataSO : ScriptableObject
{    
    public GameObject prefab;

    public BulletDataSO bulletData;

    [Range(0, 999)] public int ammoCapacity = 100; //źâ ũ��
    public bool automaticFire; //���� �����ϳ�?
    
    [Range(0.1f, 2f)] public float weaponDelay = 0.1f; //�������
    [Range(0, 10f)] public float spreadAngle = 5f; //�̰� ź�� ������ ����

    [SerializeField] private bool _multiBulletShoot = false; //�̰Ŵ� �ѹ� Ŭ���� ������ �߻�
    [SerializeField] private int _bulletCount = 1;

    public int damageFactor = 1;

    [Range(0.1f, 2f)] public float reloadTime = 0.1f;

    public AudioClip shootClip;
    public AudioClip outOfAmmoClip;
    public AudioClip reloadClip;

    public Sprite sprite;

    public bool infiniteAmmo = false; //���� źâ

    public int GetBulletCountToSpawn()
    {
        return _multiBulletShoot ? _bulletCount : 1;
    }
}
