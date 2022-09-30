using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Resource : PoolableMono
{
    [field: SerializeField]
    public ResourceDataSO ResourceData { get; set; }

    private AudioSource _audioSource;
    private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = ResourceData.useSound;
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //�ش� ���ҽ��� �־��� ��
    public void PickUpResource()
    {
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        _collider2D.enabled = false;
        _spriteRenderer.enabled = false;
        _audioSource.Play();
        yield return new WaitForSeconds(_audioSource.clip.length + 0.3f); //������� �߰��� �Ծ������ �ʰ�
        PoolManager.Instance.Push(this);
    }

    public void DestroyResource()
    {
        gameObject.SetActive(false);
        PoolManager.Instance.Push(this);
    }

    public override void Reset()
    {
        _spriteRenderer.enabled = true;
        _collider2D.enabled = true;
    }

}
