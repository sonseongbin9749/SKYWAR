using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : AIAction
{
    public override void TakeAction()
    {
        _aiMovementData.direction = Vector2.zero; //���缭

        if(_aiActionData.attack == false)
        {
            _aiMovementData.pointOfInterest = _enemyBrain.target.position;
            _enemyBrain.Attack(); //����Ű�� ������ ����°�    
        }

        _enemyBrain.Move(_aiMovementData.direction, _aiMovementData.pointOfInterest);
    }
}