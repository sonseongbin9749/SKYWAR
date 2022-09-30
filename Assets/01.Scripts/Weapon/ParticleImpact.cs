using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleImpact : ImpactScript
{
    private ParticleSystem[] particles;

    protected override void ChildAwake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    public override void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        base.SetPositionAndRotation(pos, rot);
        StartCoroutine(DisableCoroutine());
    }

    IEnumerator DisableCoroutine()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }

        yield return new WaitForSeconds(2f);
        DestroyAfterAnimation();
    }

}
