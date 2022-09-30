using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    private List<EnemySpawner> _spawnList;
    protected List<Door> _doorList;
    public List<Door> DoorList => _doorList;
    protected List<Enemy> _enemyList; //�ش� �뿡 �����ϴ� ���� ����
    protected List<Trap> _trapList; //��ȿ� �ִ� Ʈ����

    [SerializeField]
    protected bool _roomCleared = false, _isBossRoom = false; //��� ��Ż�� ������� Ŭ����, �������ΰ�?
    public bool IsBossRoom => _isBossRoom;
    //private Boss _roomBoss;
    [SerializeField]
    private Vector3 _camOffset = new Vector3(0, 3.5f, 0); //�����濡 ���� ī�޶� �̵�

    private Transform _startPosTrm;
    public Vector3 StartPosition => _startPosTrm.position;

    private int _closedPortalCount; //���� ���� ��Ż�� ����
    private int _deadEnemyCount;

    private bool _loadComplete = false; //���� �ε������� �� �ε������

    public void LoadRoomData()
    {
        if (_loadComplete == true) return;

        _spawnList = new List<EnemySpawner>();

        transform.Find("Portals").GetComponentsInChildren<EnemySpawner>(_spawnList);
        _closedPortalCount = 0;
        foreach (EnemySpawner es in _spawnList)
        {
            UnityAction closeAction = null;
            closeAction = () =>
            {
                _closedPortalCount++;
                CheckClear();
                es.OnClosePortal.RemoveListener(closeAction);
            };
            es.OnClosePortal.AddListener(closeAction);
        }

        _enemyList = new List<Enemy>();
        transform.Find("Enemies").GetComponents<Enemy>(_enemyList);
        _deadEnemyCount = 0;

        foreach(Enemy e in _enemyList)
        {
            UnityAction dieAction = null;
            dieAction = () =>
            {
                _deadEnemyCount++;
                CheckClear();
                e.OnDie.RemoveListener(dieAction);
            };
            e.OnDie.AddListener(dieAction);
        }

        _doorList = new List<Door>();
        transform.Find("Doors").GetComponentsInChildren<Door>(_doorList);
        _startPosTrm = transform.Find("StartPosition");

        _trapList = new List<Trap>();
        transform.Find("Traps").GetComponentsInChildren<Trap>(_trapList);

        _loadComplete = true;

        if(_isBossRoom)
        {
            //���⿡ ���� ã�Ƽ� �����ϴ� ���� �ʿ�
        }
    }

    public void CheckClear()
    {
        if(_deadEnemyCount >= _enemyList.Count && _closedPortalCount >= _spawnList.Count)
        {
            ClearRoom();
        }
    }

    protected virtual void ClearRoom()
    {
        OpenAllDoors();
        _roomCleared = true;
    }
    
    public void ActiveRoom()
    {
        _spawnList.ForEach(x => x.ActivePortalSensor()); //��� ��Ż�� ���� Ȱ��ȭ
        
        if(_isBossRoom)
        {
            //�����濡 ���缭 ī�޶� ���� �� ���� ���°� ���⼭ ���ش�.
        }
    }

    private void OpenAllDoors()
    {
        _doorList.ForEach(x => x.OpenDoor(true));
    }
}
