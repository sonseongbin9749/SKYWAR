using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunPanel : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    public Weapon Weapon { get => _weapon; }

    private Image _weaponImage;
    private TextMeshProUGUI _ammoText;

    private RectTransform _rectTrm = null;
    private float _initFontSize;
    private Color _trasparentColor = new Color(0, 0, 0, 0);

    public RectTransform RectTrm
    {
        get
        {
            if (_rectTrm == null)
                _rectTrm = GetComponent<RectTransform>();
            return _rectTrm;
        }
    }

    private void Awake()
    {
        _weaponImage = transform.Find("GunImage").GetComponent<Image>();
        _ammoText = transform.Find("RemainBulletText").GetComponent<TextMeshProUGUI>();
        _rectTrm = GetComponent<RectTransform>();
    }

    public void Init(Weapon weapon)
    {
        _weapon = weapon;
        if(_weapon != null)
        {
            _weaponImage.sprite = weapon.WeaponData.sprite;
            _weaponImage.color = Color.white;
        }
        else
        {
            _weaponImage.sprite = null;
            _weaponImage.color = _trasparentColor;
        }
        _initFontSize = _ammoText.fontSize;
    }

    public void UpdateBullet(int amount)
    {
        if(amount < 0) //무한탄창일 경우
        {
            _ammoText.color = Color.blue;
            _ammoText.fontSize = _initFontSize + 8;
            _ammoText.SetText("∞");
            return;
        }
        _ammoText.fontSize = _initFontSize;
        if(amount == 0)
        {
            _ammoText.color = Color.red;
        }
        else
        {
            _ammoText.color = Color.white;
        }
        _ammoText.SetText(amount.ToString());
    }
}
