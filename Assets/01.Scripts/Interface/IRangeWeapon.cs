using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangeWeapon
{
    Vector3 GetRightDirection(); //�Ѿ��� �߻� ����
    Vector3 GetFirePosition(); //�߻���ġ
    Vector3 GetShellEjectPosition(); //ź�ǰ� ���� ��ġ
    Vector3 GetEjectDirection(); //ź�ǰ� ������ ����
}
