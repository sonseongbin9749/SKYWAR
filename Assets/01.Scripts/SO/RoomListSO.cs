using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Room/RoomList")]
public class RoomListSO : ScriptableObject
{
    public List<Room> startRooms; //시작위치 룸
    public List<Room> monsterRooms; // 몬스터룸 
    public List<Room> trapRooms; //트랩 + 몬스터 (골드를 좀 많이주는 방)
    public List<Room> healRooms; //힐링룸 + 몬스터 (몹을 잡으면 최대체력증가 아이템을 주는 방)
    public List<Room> storeRooms; //상점, 그동안 모은 골드로 아이템, 체력회복, 총
    public List<Room> bossRooms; //보스방
}
