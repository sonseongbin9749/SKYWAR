using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Room/RoomList")]
public class RoomListSO : ScriptableObject
{
    public List<Room> startRooms; //������ġ ��
    public List<Room> monsterRooms; // ���ͷ� 
    public List<Room> trapRooms; //Ʈ�� + ���� (��带 �� �����ִ� ��)
    public List<Room> healRooms; //������ + ���� (���� ������ �ִ�ü������ �������� �ִ� ��)
    public List<Room> storeRooms; //����, �׵��� ���� ���� ������, ü��ȸ��, ��
    public List<Room> bossRooms; //������
}
