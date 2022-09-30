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
                //나중에 플레이어 스크립트 만들면 타입으로 변경할께
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

    #region 코인 데이터 관련부분
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

    #region 스테이지 로딩 관련 부분들
    [Header("스테이지 데이터들")]
    public List<RoomListSO> stages;
    private Room _currentRoom = null; //현재 있는 방
    #endregion

    public Action OnClearAllDropItems = null;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple GameManager is running");
        Instance = this;

        PoolManager.Instance =  new PoolManager(transform); //풀매니저 생성

        //여기다 각종 매니저 로직을 넣을꺼야
        GameObject timeController = new GameObject("TimeController");
        timeController.transform.parent = transform.parent;
        TimeController.instance = timeController.AddComponent<TimeController>();

        Instantiate(_textureParticleManagerPrefab, transform.parent);

        UIManager.Instance = new UIManager();

        RoomManager.Instance = new RoomManager(); //룸매니저 생성
        int bossCnt = Random.Range(8, 12);  // 총 방을 8개에서 10개 사이를 지나면 보스가 등장하도록 한다.

        RoomManager.Instance.InitStage(stages[0], bossCnt);

        _currentRoom = GameObject.FindObjectOfType<Room>(); //최초 생성되어 있는 룸이 있는지를 체크
        if(_currentRoom == null)
        {
            Room room = RoomManager.Instance.LoadStartRoom();
            room.LoadRoomData();
            ChangeRoom(room); //방전환 실시
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

    #region 룸 변경 관련 로직들
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
