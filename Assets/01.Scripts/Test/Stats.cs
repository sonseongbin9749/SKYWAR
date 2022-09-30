using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Stats
{
    public Action OnStatChanged = null;

    public static int STAT_MIN = 0;
    public static int STAT_MAX = 20;

    private SingleStat _attackStat;
    private SingleStat _defenceStat;
    
    public enum Type
    {
        Attack,
        Defence
    }
    
    public Stats(int atkAmount, int defAmount)
    {
        _attackStat = new SingleStat(atkAmount);
        _defenceStat = new SingleStat(defAmount);
    }

    public int GetStatAmount(Type statType)
    {
        return GetSingleStat(statType).GetStatAmount();
    }

    private SingleStat GetSingleStat(Type statType)
    {
        String statFieldName = $"_{statType.ToString().ToLower()}Stat";
        FieldInfo fInfo = this.GetType().GetField(statFieldName, 
                                BindingFlags.Instance | BindingFlags.NonPublic);
        SingleStat s = fInfo.GetValue(this) as SingleStat;
        return s;
    }
    public void SetStatAmount(Type statType, int amount)
    {
        GetSingleStat(statType).SetStatAmount(amount);
        OnStatChanged?.Invoke();
    }

    public float GetStatAmountNormalized(Type statType)
    {
        return GetSingleStat(statType).GetStatAmountNormalize();
    }

    public void IncStatAmount(Type statType)
    {
        SetStatAmount(statType, GetStatAmount(statType) + 1);
    }

    public void DecStatAmount(Type statType)
    {
        SetStatAmount(statType, GetStatAmount(statType) - 1);
    }

    private class SingleStat
    {
        private int _stat;
        public SingleStat(int amount)
        {
            SetStatAmount(amount);
        }

        public void SetStatAmount(int amount)
        {
            _stat = Mathf.Clamp(amount, STAT_MIN, STAT_MAX);
        }

        public int GetStatAmount()
        {
            return _stat;
        }

        public float GetStatAmountNormalize()
        {
            return (float)_stat / STAT_MAX;
        }
    }
}
