using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudio : AudioPlayer
{
    [SerializeField]
    private AudioClip _shootBulletClip = null, _outOfBulletClip = null, _reloadClip = null;

    public void SetAudioClip(AudioClip shootSound, AudioClip outOfBullet, AudioClip reload)
    {
        _shootBulletClip = shootSound;
        _outOfBulletClip = outOfBullet;
        _reloadClip = reload;
    }

    public void PlayShootSound()
    {
        PlayClip(_shootBulletClip);
    }
    public void PlayNoBulletSound()
    {
        PlayClip(_outOfBulletClip);
    }
    public void PlayReloadSound()
    {
        PlayClip(_reloadClip);
    }
}
