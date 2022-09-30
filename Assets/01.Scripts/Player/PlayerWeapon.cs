using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeapon : AgentWeapon
{
    //나중에 플레이어만을 위한 무기교체, 무기드랍, 무기얻기 코드가 여기 들어옵니다.
    #region 무기드랍 및 교체 로직
    public DroppedWeapon dropWeapon = null;

    private List<Weapon> _weaponList = new List<Weapon>();
    private Player _player;
    private int _currentWeaponIndex = 0;

    public UnityEvent<List<Weapon>, int> UpdateWeaponUI;
    public UnityEvent<bool, Action> ChangeWeaponUI;
    private bool _isChangeWeapon = false;
    #endregion

    [field : SerializeField]
    public UnityEvent<int, int> OnChangeTotalAmmo { get; set; }  //현재값, 최대값

    [SerializeField] private ReloadGaugeUI _reloadUI = null;
    [SerializeField] private AudioClip _cannotSound = null; //재장전이 안될때 


    [SerializeField] private int _maxTotalAmmo = 2000; //최대 2000발까지 가질 수 있어
    [SerializeField] private int _totalAmmo = 200; //처음 시작시에 2000발 가지고 시작

    public bool AmmoFull { get => _totalAmmo == _maxTotalAmmo; }
    public int TotalAmmo
    {
        get => _totalAmmo;
        set
        {
            _totalAmmo = Mathf.Clamp(value, 0, _maxTotalAmmo);
            OnChangeTotalAmmo?.Invoke(_totalAmmo, _maxTotalAmmo);
        }
    }

    private AudioSource _audioSource;

    private Weapon _currentWeapon = null; //현재 무기

    private bool _isReloading = false;
    public bool IsReloading { get => _isReloading; }


    public override void AssignWeapon()
    {
        _weapon = _currentWeapon;
        _weaponRenderer = _weapon?.Renderer;
    }

    protected override void AwakeChild()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = transform.parent.GetComponent<Player>();
    }

    protected void Start()
    {
        Weapon[] weapons = GetComponentsInChildren<Weapon>(); //자식으로 가진 모든 웨폰을 가져온다.

        for(int idx = 0; idx < _player.PlayerStatus.maxWeapon; idx++)
        {
            if(weapons.Length <= idx)
            {
                _weaponList.Add(null);
            }else
            {
                _weaponList.Add(weapons[idx]);
                weapons[idx].gameObject.SetActive(false);
            }
        }

        if(_weaponList.Count > 0)
        {
            _currentWeapon = _weaponList[0];
            _currentWeapon.gameObject.SetActive(true);
            AssignWeapon();
            OnChangeTotalAmmo?.Invoke(_totalAmmo, _maxTotalAmmo);
        }

        UpdateWeaponUI?.Invoke(_weaponList, _currentWeaponIndex);
    }

    public void ReloadGun()
    {
        if(_weapon != null && !_isReloading && _totalAmmo > 0 && !_weapon.AmmoFull)
        {
            _isReloading = true;
            _weapon.StopShooting();
            //코루틴
            StartCoroutine(ReloadCoroutine());
        }
        else
        {
            PlayClip(_cannotSound);
        }
    }

    private void PlayClip(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    IEnumerator ReloadCoroutine()
    {
        _reloadUI.gameObject.SetActive(true);
        float time = 0;
        while(time <= _weapon.WeaponData.reloadTime)
        {
            _reloadUI.ReloadGaugeNormal(time / _weapon.WeaponData.reloadTime);
            time += Time.deltaTime;
            yield return null;
        }
        _reloadUI.gameObject.SetActive(false);
        PlayClip(_weapon.WeaponData.reloadClip);

        int reloadedAmmo = Mathf.Min(TotalAmmo, _weapon.EmptyBulletCnt);
        //현재 총에 부족한 분량과 남은 총탄중에 작은분량으로 선택해서
        TotalAmmo -= reloadedAmmo;
        _weapon.Ammo += reloadedAmmo;

        _isReloading = false;
    }

    public override void Shoot()
    {
        if(_weapon == null)
        {
            PlayClip(_cannotSound);
            return;
        }
        if(_isReloading)
        {
            PlayClip(_weapon.WeaponData.outOfAmmoClip);
            return;
        }
        base.Shoot();
    }

    //이 함수는 x키를 눌렀을 때 실행됩니다. 
    public void AddWeapon()
    {
        if (_isReloading) return;
        /*3가지 경우
        1 . 땅에 떨어진 무기가 있고, 그 위에 플레이어가 있고, 내가지금 무기를 안들고 있어.
        => 이러면 줍는다.
        2. 땅에 떨어진 무기가 있고, 내가 지금 무기를 들고 있어 
        => 내가 들고 있는 건 버리고, 땅에 떨어진 건 줍는다.
        3. 땅에 떨어진 무기도 없고 내가 지금 무기를 들고 있다면 
        => 버린다.
        */

        //3번 케이스
        if(_currentWeapon != null)
        {
            DropWeapon(_currentWeapon);
            
        }

        //1,2번 케이스
        if(dropWeapon != null)
        {
            Vector3 offset = new Vector3(0.5f, 0, 0);

            dropWeapon.transform.parent = transform;
            dropWeapon.transform.localPosition = offset;
            dropWeapon.transform.localRotation = Quaternion.identity;

            _currentWeapon = _weapon = dropWeapon.weapon;

            _weaponList[_currentWeaponIndex] = _currentWeapon; //주운 무기를 리스트에 넣는다.

            dropWeapon.PickUpWeapon();
            dropWeapon = null;
        }

        UpdateWeaponUI?.Invoke(_weaponList, _currentWeaponIndex);
    }

    private void DropWeapon(Weapon weapon)
    {
        _weaponList[_currentWeaponIndex] = null; //버려야해
        _weapon = null;
        _currentWeapon = null;
        weapon.StopShooting();
        weapon.transform.parent = null; //월드에다가 던져버린다.

        //총을 던질때는 총구방향으로 던지도록 코드를 작성할께
        Vector3 targetPosition = weapon.GetRightDirection() * 0.3f 
                                            + weapon.transform.position;
        weapon.transform.rotation = Quaternion.identity;
        weapon.transform.localScale = Vector3.one;

        weapon.transform.DOMove(targetPosition, 0.5f).OnComplete(()=>
        {
            weapon.droppedWeapon.IsActive = true;
        });
    }

    public void ChangeToNextWeapon(bool isPrev)
    {
        if(_isReloading || _weaponList.Count <= 1 || _isChangeWeapon == true)
        {
            PlayClip(_cannotSound);
            return;
        }

        _isChangeWeapon = true;
        _currentWeapon?.gameObject.SetActive(false); //현재 들고 있는 무기 비활성화 해주고

        ChangeWeaponUI?.Invoke(isPrev, () =>
        {
            int nextIdx = 0;
            if (isPrev)
            {
                nextIdx = _currentWeaponIndex - 1 < 0 ? _weaponList.Count - 1 : _currentWeaponIndex - 1;
            }
            else
            {
                nextIdx = (_currentWeaponIndex + 1) % _weaponList.Count;
            }

            ChangeWeapon(_weaponList[nextIdx]);
            _currentWeaponIndex = nextIdx;

            _isChangeWeapon = false;
        });
        
    }

    private void ChangeWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;
        if(weapon != null)
        {
            weapon.gameObject.SetActive(true);
            weapon.ResetWeapon();
        }
        AssignWeapon();
    }
}
