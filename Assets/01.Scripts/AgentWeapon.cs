using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    protected Weapon _weapon;

    protected WeaponRenderer _weaponRenderer;

    protected float _desireAngle; //���⸦ �ٶ󺸴� ����

    private void Awake()
    {
        AssignWeapon();
        AwakeChild();
    }

    protected virtual void AwakeChild()
    {
        //do nothing;
    }

    public virtual void AssignWeapon()
    {
        _weaponRenderer = GetComponentInChildren<WeaponRenderer>();
        _weapon = GetComponentInChildren<Weapon>();
    }

    public virtual void AimWeapon(Vector2 pointerPos)
    {
        if (_weapon == null) return; //������ �ִ� ���Ⱑ ������ ����

        Vector3 aimDirection = (Vector3)pointerPos - transform.position;
        //360�� ������ ������������ ������ ����ϰ�
        _desireAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //�ﰢ������ 

        AdjustWeaponRendering();
        
        transform.rotation = Quaternion.AngleAxis(_desireAngle, Vector3.forward); //z�� ���� ȸ��

    }

    private void AdjustWeaponRendering()
    {
        if(_weaponRenderer != null)
        {
            _weaponRenderer.FlipSprite( _desireAngle > 90f || _desireAngle < -90f ); //���� true��?
            _weaponRenderer.RenderBehindHead( _desireAngle > 0 && _desireAngle < 180 );
        }
    }

    public virtual void Shoot()
    {
        if(_weapon != null)
        {
            _weapon.TryShooting();
        }
    }

    public virtual void StopShooting()
    {
        if(_weapon != null)
        {
            _weapon.StopShooting();
        }
    }
}
