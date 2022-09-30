using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : AIAction
{
    public override void TakeAction()
    {
        _aiMovementData.direction = Vector2.zero; //멈춰서

        if(_aiActionData.attack == false)
        {
            _aiMovementData.pointOfInterest = _enemyBrain.target.position;
            _enemyBrain.Attack(); //공격키가 눌리게 만드는거    
        }

        _enemyBrain.Move(_aiMovementData.direction, _aiMovementData.pointOfInterest);
    }
}