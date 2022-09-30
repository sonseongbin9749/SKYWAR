using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackBlood : FeedBack
{
    [SerializeField] private float _randomRange = 0.3f;

    private IHittable _agent;

    private void Awake()
    {
        _agent = GetComponentInParent<IHittable>();
    }

    public override void CompletePrevFeedBack()
    {
        //do nothing;
    }

    public override void CreateFeedBack()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(-_randomRange, +_randomRange), 
            Random.Range(-_randomRange, +_randomRange), 0);

        Vector3 pos = transform.position + randomPos;

        Vector3 dir = (transform.position - _agent.HitPoint).normalized;
        TextureParticleManager.Instance.SpawnBlood(pos, dir);

    }
}
