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
        //���� �ָ��� �� ������ �� ����
        if(_aiBrain.LeftHand.gameObject.activeSelf == false && _aiBrain.RightHand.gameObject.activeSelf == false)
        {
            _complete = true;
            Callback?.Invoke(false); //�� ���ݼ��� �������� false�� ����
            //�׷��� false�϶��� �ٷ� �������� ����
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
            yield return new WaitForSeconds(1f); //1�ʴ��
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

        //������ ��� �������Ȱž�- õ�簳���� �����̰� ���ɵ� ���ٲ���. ��������
        yield return new WaitForSeconds(2.5f);
        if(_complete == false) //�� ���� �ڵ尡 �״°�� �ۿ� ����.
        {
            Callback?.Invoke(false);
        }

    }


}
