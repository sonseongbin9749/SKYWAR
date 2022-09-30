using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    private BoxCollider2D _boxCol;
    private Weapon _weapon = null;
    public Weapon weapon { get => _weapon; }
    private WeaponTooltip _weaponTooltip = null;

    [SerializeField]
    private bool _isActive = false;
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            BoxCol.enabled = _isActive;
        }
    }
    public BoxCollider2D BoxCol
    {
        get
        {
            if(_boxCol == null) _boxCol = GetComponent<BoxCollider2D>();
            return _boxCol;
        }
    }

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
        IsActive = false;
    }

    public void ShowInfoPanel()
    {
        UIManager.Instance.OpenMessageTooltip("무기 교체를 원하시면 X 키를 누르세요");
        _weaponTooltip = UIManager.Instance.OpenWeaponTooltip(
                            _weapon.WeaponData, transform.position);
    }

    public void HideInfoPanel()
    {
        UIManager.Instance.CloseMessageTooltip();
        UIManager.Instance.CloseWeaponTooltip(_weaponTooltip);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsActive == false) return;
        if(collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if(p != null)
            {
                p.playerWeapon.dropWeapon = this;
            }
            ShowInfoPanel();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsActive == false) return;
        if(collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.playerWeapon.dropWeapon = null;
            }
            HideInfoPanel();
        }
    }

    public void PickUpWeapon()
    {
        HideInfoPanel();
        IsActive = false;
    }
}
