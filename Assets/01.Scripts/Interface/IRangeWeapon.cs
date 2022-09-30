using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangeWeapon
{
    Vector3 GetRightDirection(); //총알의 발사 방향
    Vector3 GetFirePosition(); //발사위치
    Vector3 GetShellEjectPosition(); //탄피가 나올 위치
    Vector3 GetEjectDirection(); //탄피가 나오는 방향
}
