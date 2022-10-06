using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movecamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    void Start()
    {
        
    }

    void LateUpdate()//플레이어가 움직인 후에 따라 움직여야 하기 때문
    {
        transform.position = new Vector2(target.position.x, target.position.y);
    }
}
