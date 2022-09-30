using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeapon : AgentWeapon
{
    //���߿� �÷��̾�� ���� ���ⱳü, ������, ������ �ڵ尡 ���� ���ɴϴ�.
    #region ������ �� ��ü ����
    public DroppedWeapon dropWeapon = null;

    private List<Weapon> _weaponList = new List<Weapon>();
    private Player _player;
    private int _currentWeaponIndex = 0;

    public UnityEvent<List<Weapon>, int> UpdateWeaponUI;
    public UnityEvent<bool, Action> ChangeWeaponUI;
    private bool _isChangeWeapon = false;
    #endregion

    [field : SerializeField]
    public UnityEvent<int, int> OnChangeTotalAmmo { get; set; }  //���簪, �ִ밪

    [SerializeField] private ReloadGaugeUI _reloadUI = null;
    [SerializeField] private AudioClip _cannotSound = null; //�������� �ȵɶ� 


    [SerializeField] private int _maxTotalAmmo = 2000; //�ִ� 2000�߱��� ���� �� �־�
    [SerializeField] private int _totalAmmo = 200; //ó�� ���۽ÿ� 2000�� ������ ����

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

    private Weapon _currentWeapon = null; //���� ����

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
        Weapon[] weapons = GetComponentsInChildren<Weapon>(); //�ڽ����� ���� ��� ������ �����´�.

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
            //�ڷ�ƾ
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
        //���� �ѿ� ������ �з��� ���� ��ź�߿� �����з����� �����ؼ�
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

    //�� �Լ��� xŰ�� ������ �� ����˴ϴ�. 
    public void AddWeapon()
    {
        if (_isReloading) return;
        /*3���� ���
        1 . ���� ������ ���Ⱑ �ְ�, �� ���� �÷��̾ �ְ�, �������� ���⸦ �ȵ�� �־�.
        => �̷��� �ݴ´�.
        2. ���� ������ ���Ⱑ �ְ�, ���� ���� ���⸦ ��� �־� 
        => ���� ��� �ִ� �� ������, ���� ������ �� �ݴ´�.
        3. ���� ������ ���⵵ ���� ���� ���� ���⸦ ��� �ִٸ� 
        => ������.
        */

        //3�� ���̽�
        if(_currentWeapon != null)
        {
            DropWeapon(_currentWeapon);
            
        }

        //1,2�� ���̽�
        if(dropWeapon != null)
        {
            Vector3 offset = new Vector3(0.5f, 0, 0);

            dropWeapon.transform.parent = transform;
            dropWeapon.transform.localPosition = offset;
            dropWeapon.transform.localRotation = Quaternion.identity;

            _currentWeapon = _weapon = dropWeapon.weapon;

            _weaponList[_currentWeaponIndex] = _currentWeapon; //�ֿ� ���⸦ ����Ʈ�� �ִ´�.

            dropWeapon.PickUpWeapon();
            dropWeapon = null;
        }

        UpdateWeaponUI?.Invoke(_weaponList, _currentWeaponIndex);
    }

    private void DropWeapon(Weapon weapon)
    {
        _weaponList[_currentWeaponIndex] = null; //��������
        _weapon = null;
        _currentWeapon = null;
        weapon.StopShooting();
        weapon.transform.parent = null; //���忡�ٰ� ����������.

        //���� �������� �ѱ��������� �������� �ڵ带 �ۼ��Ҳ�
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
        _currentWeapon?.gameObject.SetActive(false); //���� ��� �ִ� ���� ��Ȱ��ȭ ���ְ�

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
