using UnityEngine;

[CreateAssetMenu(menuName = "SO/Agent/Status")]
public class AgentStatusSO : ScriptableObject
{
    [Range(0, 0.9f)] public float critical; //ĳ������ ũ��Ƽ�� Ȯ��
    [Range(1.5f, 3f)] public float criticalMinDmg; //ũ��Ƽ�� �̴ϸ� ������
    [Range(1.5f, 3f)] public float criticalMaxDmg; //ũ��Ƽ�� �ƽ� ������

    [Range(0, 0.7f)] public float dodge; //ĳ���� ȸ��Ȯ��
    [Range(3, 8)] public int maxHP; //�ִ� ü��

    [Range(1, 5)] public int maxWeapon = 3;
}
