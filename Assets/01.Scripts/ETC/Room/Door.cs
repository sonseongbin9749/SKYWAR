using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Door : MonoBehaviour
{
    private Transform _openTrm;
    private Transform _closeTrm;

    [SerializeField]
    private bool _isOpen = false;

    private AudioSource _audioSource;

    [SerializeField]
    private RoomIconSO _iconSo;
    private SpriteRenderer _iconRenderer;

    [SerializeField]
    private RoomType _nextRoom;
    public RoomType NextRoomType
    {
        get => _nextRoom;
        set
        {
            _nextRoom = value;
            if(_iconRenderer == null )
            {
                _iconRenderer = transform.Find("Category/Icon").GetComponent<SpriteRenderer>();
            }
            _iconRenderer.sprite = _iconSo.sprites[(int)value];
        }
    }

    private void Awake()
    {
        _openTrm = transform.Find("Open");
        _closeTrm = transform.Find("Closed");
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isOpen)
        {
            GameManager.Instance.LoadNextRoom(NextRoomType);
        }
    }

    public void OpenDoor(bool isOpen)
    {
        _isOpen = isOpen;
        _openTrm.gameObject.SetActive(_isOpen);
        _closeTrm.gameObject.SetActive(!_isOpen);
        if(_isOpen == true)
            _audioSource.Play();
    }
}
