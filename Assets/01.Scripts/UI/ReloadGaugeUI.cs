using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadGaugeUI : MonoBehaviour
{

    [SerializeField]
    private Transform bar;

    //0 ~ 1까지의 표준화된 값을 뜻해
    public void ReloadGaugeNormal(float value)
    {
        bar.localScale = new Vector3(value, 1, 1);
    }
}
