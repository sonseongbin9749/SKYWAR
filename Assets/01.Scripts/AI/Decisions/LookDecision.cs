using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LookDecision : AIDecision
{
    [SerializeField]
    [Range(0.1f, 15f)]
    private float _distance = 5f;

    public UnityEvent OnPlayerSpotted;
    public LayerMask raycastMask;

    public override bool MakeADecision()
    {
        Vector3 dir = _enemyBrain.target.transform.position - transform.position;
        dir.z = 0;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, _distance, raycastMask);

        if(hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPlayerSpotted?.Invoke();
            return true;
        }
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeGameObject == gameObject 
            && _enemyBrain != null 
            && _enemyBrain.target != null)
        {
            Gizmos.color = Color.red;
            Vector3 dir = _enemyBrain.target.transform.position - transform.position;
            dir.z = 0;
            Gizmos.DrawRay(transform.position, dir.normalized * _distance);
            Gizmos.color = Color.white;
        }
    }
#endif

}
