using UnityEngine;

[CreateAssetMenu(menuName = "SO/Agent/Status")]
public class AgentStatusSO : ScriptableObject
{
    [Range(0, 0.9f)] public float critical; //캐릭터의 크리티컬 확률
    [Range(1.5f, 3f)] public float criticalMinDmg; //크리티컬 미니멈 데미지
    [Range(1.5f, 3f)] public float criticalMaxDmg; //크리티컬 맥스 데미지

    [Range(0, 0.7f)] public float dodge; //캐릭터 회피확률
    [Range(3, 8)] public int maxHP; //최대 체력

    [Range(1, 5)] public int maxWeapon = 3;
}
