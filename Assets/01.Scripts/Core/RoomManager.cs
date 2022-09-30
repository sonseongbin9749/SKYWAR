using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Define;

public class RoomManager
{
    public static RoomManager Instance;

    private int _roomCnt = 0;  //���� ������ ���� ����
    private int _bossCnt = 0;  //������ ��Ÿ�� ���� ����
    private RoomListSO _roomList;

    //1���� ������������ �ϳ��� RoomList�� ������, ���ӸŴ����� ��Ŵ����� ������������ �ʱ�ȭ �Ѵ�.
    public void InitStage(RoomListSO listSo, int bossCnt)
    {
        _roomCnt = 0;
        _roomList = listSo;
        _bossCnt = bossCnt; 
    }

    public Room LoadStartRoom()
    {
        return LoadNextRoom(RoomType.Start);
    }

    public Room LoadNextRoom(RoomType type)
    {
        Room roomPrefab = LoadRandomRoom(type);
        Room room = GameObject.Instantiate(roomPrefab, null);
        SetRoomDoorDestination(room); //�ش� ���� ���鿡�ٰ� ���������� ����ǵ��� ��.
        _roomCnt++;
        return room;
    }

    public void SetRoomDoorDestination(Room room)
    {
        if(room.DoorList.Count == 1)
        {
            room.DoorList[0].NextRoomType = IsABossRoom();
        }
        else if(room.DoorList.Count >= 2)
        {
            room.DoorList[0].NextRoomType = IsABossRoom();
            for(int i = 1; i < room.DoorList.Count; i++)
            {
                room.DoorList[i].NextRoomType = GetSpecialRoom();
            }
        }
    }

    private RoomType IsABossRoom()
    {
        if(_roomCnt == _bossCnt - 1)
            return RoomType.Store; //���� ������ �׻� ����������
        
        if (_roomCnt == _bossCnt)
            return RoomType.Boss;

        return RoomType.Monster;
    }

    private RoomType GetSpecialRoom()
    {
        if (_roomCnt == _bossCnt - 1)
            return RoomType.Store; //���� ������ �׻� ����������

        float dice = Random.Range(0, 1f);
        if(dice < 0.5f)
        {
            return RoomType.Trap;
        }
        else
        {
            return RoomType.Heal;
        }
    }


    private Room LoadRandomRoom(RoomType type)
    {
        FieldInfo field = typeof(RoomListSO).GetField($"{type.ToString().ToLower()}Rooms", 
                                                    BindingFlags.Instance | BindingFlags.Public);
        List<Room> list = field.GetValue(_roomList) as List<Room>;

        int randIdx = Random.Range(0, list.Count);
        return list[randIdx];
    }
}
