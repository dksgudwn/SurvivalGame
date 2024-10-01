using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    MaxHp = 0,
    MaxHunger = 1,
    MaxStamina = 2,
    ArmorRatio = 3,
    HealthRegeneration = 4,
    StaminaRegeneration = 5,
    Strength = 6,
    AttackSpeed = 7,
    MoveSpeed = 8,
    End = 9
}

[Serializable]
public class StatData
{
    public float baseValue;
    public Stat statType;

    private readonly List<StatModifier> statModifiers = new List<StatModifier>();

    public StatData(float BaseValue, Stat InitStatType)
    {
        baseValue = BaseValue;
        statType = InitStatType;
        statModifiers = new List<StatModifier>();
    }

    private bool isDirty = true;
    private float statValue;
    public float StatValue
    {
        get
        {
            if (isDirty)
            //값이 변화했다면?
            {
                statValue = CalculateFinalValue();
                //다시 계산한 값을 할당
                isDirty = false;
                //변화하지 않은 상태로 설정
            }

            return statValue;
            //계산한 값 반환
        }
    }

    public void AddModifier(StatModifier modifier)
    {
        isDirty = true;
        statModifiers.Add(modifier);
    }

    public bool RemoveModifier(StatModifier modifier)
    {
        if (statModifiers.Remove(modifier))
        {
            isDirty = true;
            return true;
        }

        Debug.LogError("Error: Modifier is not removed : " + statType + "\nBase : " + baseValue + " Value: " + StatValue);
        return false;
    }

    public bool RemoveAllModifierInSource(object source)
    {
        bool didRemove = false;

        for (int count = statModifiers.Count - 1; count >= 0; count--)
        {
            if (statModifiers[count]._Source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(count);
            }
        }

        return didRemove;
    }

    public float CalculateFinalValue()
    {
        float finalValue = baseValue;
        float percentSum = 1f;
        // 기본 값 대입
        
        Debug.Log(statModifiers.Count);
        if (statModifiers.Count > 0)
        {
            StatModifier modifier;
            // 반복 생성 방지

            for (int count = 0; count < statModifiers.Count; count++)
            // 현재 StatModifier의 수만큼 반복 
            {
                modifier = statModifiers[count];
                if (modifier._IsPercent == true)
                    percentSum += 0.01f * modifier._Value;
                // 퍼센트 수치가 맞다면 백분율로 변환해 퍼센트 값에 추가
                else if (modifier._IsPercent == false)
                    finalValue += modifier._Value;
                // 퍼센트 수치가 아니라면 바로 finalValue에 더하기
            }
        }

        if (percentSum > 0f) return (float)Math.Round(finalValue * percentSum, 3);
        // 기본 값 + 고정 수치 값에 곱연산 후 소수점 3자리까지 처리
        else return (float)Math.Round(finalValue, 3);
        // 기본 값 소수점 3자리까지 처리
    }
}

public class StatModifier
{
    public readonly float _Value;
    public readonly bool _IsPercent;
    public readonly object _Source;

    public StatModifier(float Value, bool IsPercent, object Source)
    {
        _Value = Value;
        _IsPercent = IsPercent;
        _Source = Source;

    }
}

[CreateAssetMenu(menuName = "SO/Player/PlayerStat", fileName = "New Player Stat")]
public class PlayerStatSO : ScriptableObject
{
    public List<StatData> Stats;
    public Dictionary<Stat, StatData> StatDictionary = new Dictionary<Stat, StatData>();

    /*
        private void OnEnable()
        {
            if(Stats.Count < (int)Stat.End)
            {
                Stats = new List<StatData>();

                for (int count = 0; count < (int)Stat.End; count++)
                {
                    Stats.Add(new StatData(0, (Stat)count));
                }

            }
        }
    */

    public void InitializeStatDictionary()
    {
        if (Stats == null)
        {
            Debug.LogError($"Error: {this.name}'s Stat is null. Please check inpector.");
        }

        StatDictionary = new Dictionary<Stat, StatData>();
        StatData TempStatData;

        foreach (StatData data in Stats)
        {
            TempStatData = new StatData(data.baseValue, data.statType);
            StatDictionary.Add(data.statType, TempStatData);
        }
    }
}
