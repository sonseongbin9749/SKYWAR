using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBossBodyAnimation : MonoBehaviour
{
    protected Animator _bodyAnimator;

    protected readonly int _hashSummonPortal = Animator.StringToHash("SummonPortal");
    protected readonly int _hashFireball = Animator.StringToHash("Fireball");
    protected readonly int _hashGenerateArmBool = Animator.StringToHash("GenerateArm");
    protected readonly int _hashNeutralBool = Animator.StringToHash("Neutral");
    protected readonly int _hashDead = Animator.StringToHash("Dead");

    protected SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _bodyAnimator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayDeadAnimation()
    {
        _bodyAnimator.SetTrigger(_hashDead);
    }

    public void PlaySummonPortalAnimation()
    {
        _bodyAnimator.SetTrigger(_hashSummonPortal);
    }

    public void PlayFireballAnimation()
    {
        _bodyAnimator.SetTrigger(_hashFireball);
    }

    public void EnterGenerateArm(bool value)
    {
        _bodyAnimator.SetBool(_hashGenerateArmBool, value);
    }

    public void EnterNeutral(bool value)
    {
        _bodyAnimator.SetBool(_hashNeutralBool, value);
    }

    public void SetInvincible(bool value)
    {
        _spriteRenderer.material.SetInt("_MakeNeutralColor", value ? 1 : 0);
    }
}
