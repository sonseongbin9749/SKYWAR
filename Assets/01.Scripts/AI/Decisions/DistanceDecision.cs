using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDecision : AIDecision
{
    [Range(0.1f, 30f)] public float distance = 5f;
    
    public override bool MakeADecision()
    {
        //여기를 니네가 만들꺼야
        float calc = 0f;

        //여기서 주의사항. EnemyBrain에 BasePosition이라는걸 만들어야 해
        calc = Vector2.Distance(_enemyBrain.target.position, transform.position);

        if(calc < distance)
        {
            if(_aiActionData.targetSpotted == false)
            {
                _aiActionData.targetSpotted = true;
            }
        }
        else
        {
            _aiActionData.targetSpotted = false;
        }

        return _aiActionData.targetSpotted;
    }

    //빌드하면 에러난다. => 전처리기
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(UnityEditor.Selection.activeGameObject == gameObject)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, distance);
            Gizmos.color = Color.white;
        }
    }
#endif

}
