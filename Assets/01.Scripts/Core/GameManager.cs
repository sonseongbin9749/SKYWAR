using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Define;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Texture2D cursorTexture = null;
    [SerializeField] private PoolingListSO _initList = null;
    [SerializeField] private TextureParticleManager _textureParticleManagerPrefab;

    private Transform _playerTrm;

    public Transform PlayerTrm
    {
        get
        {
            if(_playerTrm == null)
            {
                //���߿� �÷��̾� ��ũ��Ʈ ����� Ÿ������ �����Ҳ�
                _playerTrm = GameObject.FindGameObjectWithTag("Player").transform;
            }
            return _playerTrm;
        }
    }
    public Player _player;
    public AgentStatusSO PlayerStatus
    {
        get
        {
            if (_player == null)
                _player = PlayerTrm.GetComponent<Player>();
            return _player.PlayerStatus;
        }
    }

    #region ���� ������ ���úκ�
    public UnityEvent<int> OnCoinUpdate = null;
    private int _coinCnt;
    public int Coin
    {
        get => _coinCnt;
        set
        {
            _coinCnt = value;
            OnCoinUpdate?.Invoke(_coinCnt);
        }
    }
    #endregion

    #region �������� �ε� ���� �κе�
    [Header("�������� �����͵�")]
    public List<RoomListSO> stages;
    private Room _currentRoom = null; //���� �ִ� ��
    #endregion

    public Action OnClearAllDropItems = null;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple GameManager is running");
        Instance = this;

        PoolManager.Instance =  new PoolManager(transform); //Ǯ�Ŵ��� ����

        //����� ���� �Ŵ��� ������ ��������
        GameObject timeController = new GameObject("TimeController");
        timeController.transform.parent = transform.parent;
        TimeController.instance = timeController.AddComponent<TimeController>();

        Instantiate(_textureParticleManagerPrefab, transform.parent);

        UIManager.Instance = new UIManager();

        RoomManager.Instance = new RoomManager(); //��Ŵ��� ����
        int bossCnt = Random.Range(8, 12);  // �� ���� 8������ 10�� ���̸� ������ ������ �����ϵ��� �Ѵ�.

        RoomManager.Instance.InitStage(stages[0], bossCnt);

        _currentRoom = GameObject.FindObjectOfType<Room>(); //���� �����Ǿ� �ִ� ���� �ִ����� üũ
        if(_currentRoom == null)
        {
            Room room = RoomManager.Instance.LoadStartRoom();
            room.LoadRoomData();
            ChangeRoom(room); //����ȯ �ǽ�
        }else
        {
            _currentRoom.LoadRoomData();
            PlayerTrm.position = _currentRoom.StartPosition;
            RoomManager.Instance.SetRoomDoorDestination(_currentRoom);
            _currentRoom.ActiveRoom();
        }


        SetCursorIcon();
        CreatePool();
    }

    private void CreatePool()
    {
        foreach (PoolingPair pair in _initList.list)
            PoolManager.Instance.CreatePool(pair.prefab, pair.poolCnt);
    }

    private void SetCursorIcon()
    {
        Cursor.SetCursor(cursorTexture,
            new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f),
            CursorMode.Auto);
    }

    public float CriticalChance { get => PlayerStatus.critical; }
    public float CriticalMinDamage { get => PlayerStatus.criticalMinDmg; }
    public float CriticalMaxDamage { get => PlayerStatus.criticalMaxDmg; }

    #region �� ���� ���� ������
    public void LoadNextRoom(RoomType type)
    {
        Room room = RoomManager.Instance.LoadNextRoom(type);
        ChangeRoom(room);
    }

    private void ChangeRoom(Room newRoom)
    {
        if(_currentRoom != null)
        {
            Destroy(_currentRoom.gameObject);
        }

        TextureParticleManager.Instance.ClearAllParticle();
        OnClearAllDropItems?.Invoke();

        newRoom.transform.position = Vector3.zero;
        PlayerTrm.position = newRoom.StartPosition;
        _currentRoom = newRoom;
        _currentRoom.ActiveRoom();
    }
    #endregion
}
