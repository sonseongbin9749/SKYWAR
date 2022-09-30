using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockPunchAttack : DemonBossAttack
{
    private bool _complete = false;

    public override void Attack(Action<bool> Callback)
    {
        _complete = false;
        StartCoroutine(PunchSequence(Callback));
    }

    IEnumerator PunchSequence(Action<bool> Callback)
    {
        //양쪽 주먹이 다 나가리 된 상태
        if(_aiBrain.LeftHand.gameObject.activeSelf == false && _aiBrain.RightHand.gameObject.activeSelf == false)
        {
            _complete = true;
            Callback?.Invoke(false); //난 공격성공 못했으니 false를 날려
            //그러면 false일때는 바로 다음공격 진행
            yield break;
        }

        if(_aiBrain.LeftHand.gameObject.activeSelf == false)
        {
            _aiBrain.RightHand.AttackShockSequence(_aiBrain.target.position, () =>
            {
                _complete = true;
                Callback?.Invoke(true);
            });
        }
        else
        {
            _aiBrain.LeftHand.AttackShockSequence(_aiBrain.target.position, null);
            yield return new WaitForSeconds(1f); //1초대기
            if(_aiBrain.RightHand.gameObject.activeSelf == false)
            {
                _complete = true;
                Callback?.Invoke(true);
            }
            else
            {
                _aiBrain.RightHand.AttackShockSequence(_aiBrain.target.position, () =>
                {
                    _complete = true;
                    Callback?.Invoke(true);
                });
            }
        }

        //공격이 모두 마무리된거야- 천재개발자 동윤이가 어케든 해줄꺼임. ㄹㅇㅋㅋ
        yield return new WaitForSeconds(2.5f);
        if(_complete == false) //이 경우는 핸드가 죽는경우 밖에 없다.
        {
            Callback?.Invoke(false);
        }

    }


}
