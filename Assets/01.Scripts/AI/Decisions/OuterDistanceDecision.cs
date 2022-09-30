using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterDistanceDecision : AIDecision
{
    [SerializeField]
    [Range(0.1f, 15f)]
    private float _distance = 5f;

    public override bool MakeADecision()
    {
        if(_enemyBrain.basePosition != null)
        {
            return Vector3.Distance(_enemyBrain.target.transform.position, _enemyBrain.basePosition.position)
                                > _distance;
        }
        return Vector3.Distance(_enemyBrain.target.transform.position, transform.position)
                                > _distance;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _distance);
            Gizmos.color = Color.white;
        }
    }
#endif

}
