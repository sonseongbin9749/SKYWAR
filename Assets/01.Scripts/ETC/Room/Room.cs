using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    private List<EnemySpawner> _spawnList;
    protected List<Door> _doorList;
    public List<Door> DoorList => _doorList;
    protected List<Enemy> _enemyList; //해당 룸에 존재하는 적의 갯수
    protected List<Trap> _trapList; //방안에 있는 트랩들

    [SerializeField]
    protected bool _roomCleared = false, _isBossRoom = false; //모든 포탈이 사라지면 클리어, 보스방인가?
    public bool IsBossRoom => _isBossRoom;
    //private Boss _roomBoss;
    [SerializeField]
    private Vector3 _camOffset = new Vector3(0, 3.5f, 0); //보스방에 들어가면 카메라를 이동

    private Transform _startPosTrm;
    public Vector3 StartPosition => _startPosTrm.position;

    private int _closedPortalCount; //현재 폐쇄된 포탈의 갯수
    private int _deadEnemyCount;

    private bool _loadComplete = false; //방의 로드정보가 다 로딩됬는지

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
            //여기에 보스 찾아서 저장하는 로직 필요
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
        _spawnList.ForEach(x => x.ActivePortalSensor()); //모든 포탈의 센서 활성화
        
        if(_isBossRoom)
        {
            //보스방에 맞춰서 카메라 셋팅 및 연출 들어가는걸 여기서 해준다.
        }
    }

    private void OpenAllDoors()
    {
        _doorList.ForEach(x => x.OpenDoor(true));
    }
}
