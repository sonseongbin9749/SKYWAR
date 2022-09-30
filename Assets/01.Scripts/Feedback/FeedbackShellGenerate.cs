using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FeedbackShellGenerate : FeedBack
{
    public UnityEvent ShellGenerated = null;
    public IRangeWeapon _weapon;

    private void Awake()
    {
        _weapon = transform.parent.GetComponent<IRangeWeapon>();
    }

    public override void CompletePrevFeedBack()
    {
        //do nothing
    }

    public override void CreateFeedBack()
    {
        if (_weapon == null) return;
        Vector3 shellPos = _weapon.GetShellEjectPosition();
        Vector3 ejectDir = _weapon.GetEjectDirection();

        TextureParticleManager.Instance.SpawnShell(shellPos, ejectDir);
        ShellGenerated?.Invoke();
    }
} 
