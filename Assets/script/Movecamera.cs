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

    void LateUpdate()//�÷��̾ ������ �Ŀ� ���� �������� �ϱ� ����
    {
        transform.position = new Vector2(target.position.x, target.position.y);
    }
}
