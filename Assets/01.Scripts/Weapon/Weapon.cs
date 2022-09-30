using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour, IRangeWeapon
{
    #region 총 데이터
    [SerializeField] protected WeaponDataSO _weaponData;
    [SerializeField] protected GameObject _muzzle; //총구의 위치
    [SerializeField] protected Transform _shellEjectPos; //탄피 생성 지점

    [SerializeField] protected bool _isEnemyWeapon = false;

    public WeaponDataSO WeaponData { get => _weaponData; }
    #endregion

    #region 웨폰 렌더러 관련
    private WeaponRenderer _weaponRenderer;
    public WeaponRenderer Renderer { get => _weaponRenderer; }
    #endregion

    #region Ammo관련 코드
    public UnityEvent<int> OnAmmoChange; 
    [SerializeField] protected int _ammo; //현재 총알 수
    public int Ammo
    {
        get
        {
            return _weaponData.infiniteAmmo ? -1 : _ammo; //남은 탄환수를 무한탄약이면 -1아니면 남은 수 리턴
        }
        set
        {
            _ammo = Mathf.Clamp(value, 0, _weaponData.ammoCapacity);
            OnAmmoChange?.Invoke(_ammo);
        }
    }
    public bool AmmoFull { get => Ammo == _weaponData.ammoCapacity || _weaponData.infiniteAmmo; }
    public int EmptyBulletCnt { get => _weaponData.ammoCapacity - _ammo; } //부족한 탄환수 반환
    #endregion

    #region 발사로직
    public UnityEvent OnShoot;
    public UnityEvent OnShootNoAmmo;
    public UnityEvent OnStopShooting;

    protected bool _isShooting = false;
    [SerializeField] protected bool _delayCoroutine = false;
    #endregion

    #region 웨폰 드랍관련 로직
    private DroppedWeapon _droppedWeapon;
    public DroppedWeapon droppedWeapon { get => _droppedWeapon; }
    #endregion

    private void Awake()
    {
        _ammo = _weaponData.ammoCapacity;
        WeaponAudio audio = transform.Find("WeaponAudio").GetComponent<WeaponAudio>();
        audio.SetAudioClip(_weaponData.shootClip, _weaponData.outOfAmmoClip, _weaponData.reloadClip);

        //무기 드랍관련한 정보를 가져오고 false로 설정한다.
        _droppedWeapon = GetComponent<DroppedWeapon>();
        _droppedWeapon.IsActive = false;

        _weaponRenderer = GetComponent<WeaponRenderer>();
    }

    private void Update()
    {
        UseWeapon();
    }

    private void UseWeapon()
    {
        //현재 사격중이고 재장전중이 아니라면
        if(_isShooting && _delayCoroutine == false)
        {
            if(Ammo > 0 || _weaponData.infiniteAmmo)
            {
                if(!_weaponData.infiniteAmmo)
                {
                    Ammo -= _weaponData.GetBulletCountToSpawn(); //무한 탄약 총이 아니면 탄약 감소
                }

                OnShoot?.Invoke(); //실제 슈팅
                for(int i = 0; i < _weaponData.GetBulletCountToSpawn(); i++)
                {
                    ShootBullet(); //차후 구현합니다. 아마도 다음주?
                }
            }
            else
            {
                _isShooting = false;
                OnShootNoAmmo?.Invoke();
                return;
            }
            FinishShooting(); //한발 사격을 성공적으로 끝냈다면 해줘야 할일
        }
    }

    protected void FinishShooting()
    {
        StartCoroutine(DelayNextShootCoroutine());
        if(_weaponData.automaticFire == false)
        {
            _isShooting = false;
        }
    }

    protected IEnumerator DelayNextShootCoroutine()
    {
        _delayCoroutine = true;
        yield return new WaitForSeconds(_weaponData.weaponDelay);
        _delayCoroutine = false;
    }

    private void ShootBullet()
    {
        //Debug.Log("발사");
        SpawnBullet(_muzzle.transform.position, CalculateAngle(_muzzle), _isEnemyWeapon);
    }

    //발사시에 총알의 랜덤떨림에 따라서 발사각을 계산해주는 함수
    private Quaternion CalculateAngle(GameObject muzzle)
    {
        float spread = Random.Range(-_weaponData.spreadAngle, _weaponData.spreadAngle);
        //Quaternion.AngleAxis(spread, Vector3.forward);
        Quaternion spreadRot = Quaternion.Euler(new Vector3(0, 0, spread));
        return muzzle.transform.rotation * spreadRot;
    }

    private void SpawnBullet(Vector3 pos, Quaternion rot, bool isEnemyBullet)
    {
        Bullet b = PoolManager.Instance.Pop(_weaponData.bulletData.prefab.name) as Bullet;
        b.SetPositionAndRotation(pos, rot);
        b.IsEnemy = isEnemyBullet;
        b.BulletData = _weaponData.bulletData;
    }


    //사격 가능하다면 사격 시작
    public void TryShooting()
    {
        _isShooting = true;
    }

    public void StopShooting()
    {
        _isShooting = false;
        OnStopShooting?.Invoke();
    }

    public Vector3 GetRightDirection()
    {
        return transform.right;
    }

    public Vector3 GetFirePosition()
    {
        return _muzzle.transform.position;
    }

    public Vector3 GetShellEjectPosition()
    {
        return _shellEjectPos.position;
    }

    public Vector3 GetEjectDirection()
    {
        if(transform.localScale.y < 0)
        {
            return (transform.right * -0.5f + transform.up).normalized;
        }else
        {
            return (transform.right * 0.5f + transform.up).normalized * -1;
        }
    }

    public void ResetWeapon()
    {
        _isShooting = false;
        _delayCoroutine = false;
    }
}
