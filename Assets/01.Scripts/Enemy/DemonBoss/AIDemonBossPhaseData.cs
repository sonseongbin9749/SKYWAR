using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDemonBossPhaseData : MonoBehaviour
{
    public int phase;
    public bool isActive = false;

    public bool FlapperPunch;
    public bool ShockPunch;
    public bool SummonPortal;
    public bool Fireball;

    public bool hasLeftArm;
    public bool hasRightArm;

    //두 팔중에 하나라도 살아있으면
    public bool HasArms => hasLeftArm == true || hasRightArm == true;

    public DemonBossAIBrain.AttackType nextAttackType; //다음 공격타입
    public float idleTime;

    public bool CanAttack => !FlapperPunch && !ShockPunch && !SummonPortal && !Fireball;
}
