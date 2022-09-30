using System;
using UnityEngine;

public class UIManager
{
    public static UIManager Instance = null;

    private RectTransform _tooltipCanvasTrm = null;
    private MessageTooltip _messageTooltip = null;

    private int _weaponTooltipCnt = 0;
    private int _weaponTooltipOrder = 0;

    public UIManager()
    {
        try
        {
            _tooltipCanvasTrm = GameObject.Find("TooltipCanvas").GetComponent<RectTransform>();
            _messageTooltip = _tooltipCanvasTrm.Find("MessageTooltip").GetComponent<MessageTooltip>();

            _messageTooltip.CloseImmediatly(); //시작하면 바로 닫아버리고
        }catch(NullReferenceException ne)
        {
            Debug.LogError("툴팁용 캔버스가 존재하지 않습니다. 확인요망");
            Debug.Log(ne.Message);
        }
        
    }

    public void OpenMessageTooltip(string msg, float time = 0)
    {
        _messageTooltip.ShowText(msg, time);
    }

    public void CloseMessageTooltip()
    {
        _messageTooltip.CloseText();
    }

    public WeaponTooltip OpenWeaponTooltip(WeaponDataSO weaponData, Vector3 worldPos)
    {
        WeaponTooltip tooltip = PoolManager.Instance.Pop("WeaponTooltip") as WeaponTooltip;

        tooltip.SetText(weaponData);

        tooltip.PopupTooltip(worldPos, _weaponTooltipOrder);
        _weaponTooltipCnt++;
        _weaponTooltipOrder++;
        return tooltip;
    }

    public void CloseWeaponTooltip(WeaponTooltip tooltip)
    {
        tooltip?.CloseTooltip();
        _weaponTooltipCnt--;
        if(_weaponTooltipCnt <= 0)
        {
            _weaponTooltipOrder = 0;
        }
    }

}
