using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadGaugeUI : MonoBehaviour
{

    [SerializeField]
    private Transform bar;

    //0 ~ 1������ ǥ��ȭ�� ���� ����
    public void ReloadGaugeNormal(float value)
    {
        bar.localScale = new Vector3(value, 1, 1);
    }
}
