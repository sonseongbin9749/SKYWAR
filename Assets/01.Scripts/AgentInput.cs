using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class AgentInput : MonoBehaviour, IAgentInput
{
    [field: SerializeField] public UnityEvent<Vector2> OnMovementKeyPress { get; set; }
    [field: SerializeField] public UnityEvent<Vector2> OnPointerPositionChanged { get; set; }

    //단발형 총, 
    [field: SerializeField] public UnityEvent OnFireButtonPress { get; set; }
    [field: SerializeField] public UnityEvent OnFireButtonRelease { get; set; }

    public UnityEvent OnReloadButtonPress = null;
    private bool _fireButtonDown = false;

    [field: SerializeField] public UnityEvent OnDropButtonPress { get; set; }

    public UnityEvent<bool> OnNextWeaponPress;

    private void Update()
    {
        GetMovementInput();
        GetPointerInput();
        GetFireInput();
        GetReloadInput();

        GetDropInput();
        GetChangeWeapon();
    }

    private void GetChangeWeapon()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            OnNextWeaponPress?.Invoke(false); //다음무기
        }else if(Input.GetKeyDown(KeyCode.Q))
        {
            OnNextWeaponPress?.Invoke(true); //이전 무기
        }
    }

    private void GetDropInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnDropButtonPress?.Invoke();
        }
    }

    private void GetReloadInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
            OnReloadButtonPress?.Invoke();
    }

    private void GetFireInput()
    {
        if(Input.GetAxisRaw("Fire1") > 0)
        {
            if(!_fireButtonDown)
            {
                _fireButtonDown = true;
                OnFireButtonPress?.Invoke();
            }
        }
        else
        {
            if(_fireButtonDown)
            {
                _fireButtonDown = false;
                OnFireButtonRelease?.Invoke();
            }
        }
    }

    private void GetPointerInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector2 mouseInWorldPos = MainCam.ScreenToWorldPoint(mousePos);
        OnPointerPositionChanged?.Invoke(mouseInWorldPos);
    }

    private void GetMovementInput()
    {
        OnMovementKeyPress?.Invoke(
            new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
        );
    }
}
