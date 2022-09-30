using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DemonBossAIBrain : EnemyAIBrain
{
    public enum AttackType
    {
        FlapperPunch = 0,
        ShockPunch = 1,
        Fireball = 2,
        SummonPortal = 3
    }

    public class EnemyAttackData
    {
        public DemonBossAttack atk; //����
        public UnityEvent animAction; //���ݽ� �ǵ�� �ִϸ��̼�
        public float time; //��Ÿ��
    }

    public Dictionary<AttackType, EnemyAttackData> _attackDictionary = new Dictionary<AttackType, EnemyAttackData>();

    protected AIDemonBossPhaseData _phaseData;
    public AIDemonBossPhaseData PhaseData => _phaseData;

    private Hand _leftHand;
    public Hand LeftHand => _leftHand;
    private Hand _rightHand;
    public Hand RightHand => _rightHand;

    public UnityEvent OnHandAttack = null;
    public UnityEvent OnCastAttack = null;
    public UnityEvent OnKillAllEnemies = null; //��� �� �� ���̴� �̺�Ʈ

    private Queue<AttackType> _atkQueue;

    protected override void Awake()
    {
        base.Awake();
        _phaseData = transform.Find("AI").GetComponent<AIDemonBossPhaseData>();
        _leftHand = transform.Find("LeftHand").GetComponent<Hand>();
        _rightHand = transform.Find("RightHand").GetComponent<Hand>();

        _atkQueue = new Queue<AttackType>();
    }

    #region ������ �������� ����
    private void SetDictionary()
    {
        Transform attackTrm = transform.Find("AttackType");

        EnemyAttackData shockPunchData = new EnemyAttackData
        {
            atk = attackTrm.GetComponent<ShockPunchAttack>(),
            animAction = OnHandAttack,
            time = 2f
        };

        _attackDictionary.Add(AttackType.ShockPunch, shockPunchData);
    }
    #endregion


    protected override void Update()
    {
        //�׾��ٸ� �ƹ� ���۵� ���ϴ� �ڵ�

        _currentState.UpdateState();
    }


    public void Attack(AttackType type)
    {
        //��������Ϳ��ٰ� �ش� ������ �����Ѵٰ� true�� �����ؾ� ��.
        FieldInfo fInfo = typeof(AIDemonBossPhaseData).GetField(
                type.ToString(), BindingFlags.Public | BindingFlags.Instance);
        fInfo.SetValue(_phaseData, true);

        //�״����� ��ųʸ����� ������ �����ͼ� �׿� ���缭 �����ϸ� �ȴ�.

        EnemyAttackData atkData = null;
        _attackDictionary.TryGetValue(type, out atkData);

        if(atkData != null)
        {
            atkData.atk.Attack(result =>
            {
                _phaseData.idleTime = result == true ? atkData.time : 0.2f;
                SetNextPattern();
                fInfo.SetValue(_phaseData, false);
            });

            atkData.animAction.Invoke();
        }
    }

    private void SetNextPattern()
    {
        if(_atkQueue.Count == 0)
        {
            ShuffleAttackType();
        }

        _phaseData.nextAttackType = _atkQueue.Dequeue();
    }

    

    private void ShuffleAttackType()
    {
        Array types = Enum.GetValues(typeof(AttackType));

        List<AttackType> list = new List<AttackType>();
        foreach(AttackType t in types)
        {
            list.Add(t);
        }

        for(int i = 0; i < list.Count; i++)
        {
            int idx = Random.Range(0, list.Count - i);
            _atkQueue.Enqueue(list[idx]);
            list[idx] = list[list.Count - i - 1];
        }

        //while(_atkQueue.Count > 0)
        //{
        //    Debug.Log(_atkQueue.Dequeue());
        //}
    }
}
