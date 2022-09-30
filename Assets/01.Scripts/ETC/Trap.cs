using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    protected TrapDataSO _trapData;
    [SerializeField]
    protected LayerMask _whatIsEnemy;

    private AudioSource _audioSource;
    private Animator _animator;
    private int _hashActive = Animator.StringToHash("activate");

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ActivateTrap();
    }

    public void ActivateTrap()
    {
        StartCoroutine(ActiveLoop());
    }

    IEnumerator ActiveLoop()
    {
        WaitForSeconds wsActive = new WaitForSeconds(_trapData.activeTime);
        WaitForSeconds wsDeactive = new WaitForSeconds(_trapData.deactiveTime);
        _audioSource.clip = _trapData.activeClip;

        while(true)
        {
            yield return wsDeactive;
            _animator.SetBool(_hashActive, true);
            _audioSource.Play();
            yield return wsActive;
            _animator.SetBool(_hashActive, false);
        }
    }

    public void DisableTrap()
    {
        StopAllCoroutines(); //��� �ڷ�ƾ ����
        _animator.SetBool(_hashActive, false);
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layerMask = 1 << collision.gameObject.layer;

        if((_whatIsEnemy & layerMask) > 0 )
        {
            //Ʈ���� �����Ʈ �ö��̴��� �ε����� ������ �θ� �ִ°� ã�ƿ;� �Ѵ�.
            IHittable hittable = collision.GetComponentInParent<IHittable>();
            hittable?.GetHit(_trapData.damage, gameObject);
        }
    }
}
