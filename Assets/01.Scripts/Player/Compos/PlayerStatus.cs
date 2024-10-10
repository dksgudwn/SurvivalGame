using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IPlayerComponent
{
    private Player _player;

    [Header("PlayerStat")]
    [SerializeField] private PlayerStatSO baseStatSO; // 게임 시작할 때 적용될 플레이어 기본 스텟
    [SerializeField] private LayerMask interactionLayer;

    // 실사용할 플레이어 스텟
    [HideInInspector] public PlayerStatSO InGameStatSO { get; private set; }

    #region Stat Change Events
    public event Action<StatData> OnMaxHpChange;
    public event Action<StatData> OnMaxHungerChange;
    public event Action<StatData> OnMaxStaminaChange;
    public event Action<StatData> OnArmorRatioChange;
    public event Action<StatData> OnHealthRegenerationChange;
    public event Action<StatData> OnStaminaRegenerationChange;
    public event Action<StatData> OnStrengthChange;
    public event Action<StatData> OnAttackSpeedChange;
    public event Action<StatData> OnMoveSpeedChange;
    #endregion

    private Dictionary<Stat, Action<StatData>> statChangeActions;

    public void Initialize(Player player)
    {
        _player = player;

        // 기본 능력치 복제 및 초기화
        InGameStatSO = Instantiate(baseStatSO);
        InGameStatSO.InitializeStatDictionary();
    }

    // StatChangeActions 초기화
    public void AfterInitialize()
    {
        statChangeActions = new Dictionary<Stat, Action<StatData>>
        {
            { Stat.MaxHp, OnMaxHpChange },
            { Stat.MaxHunger, OnMaxHungerChange },
            { Stat.MaxStamina, OnMaxStaminaChange },
            { Stat.ArmorRatio, OnArmorRatioChange },
            { Stat.HealthRegeneration, OnHealthRegenerationChange },
            { Stat.StaminaRegeneration, OnStaminaRegenerationChange },
            { Stat.Strength, OnStrengthChange },
            { Stat.AttackSpeed, OnAttackSpeedChange },
            { Stat.MoveSpeed, OnMoveSpeedChange }
        };
    }

    /// 지정된 스탯의 현재 값 반환
    /// <param name="stat">가져올 스탯</param>
    /// <returns>스탯의 현재 값</returns>
    public float GetStatValue(Stat stat)
    {
        if (!InGameStatSO.StatDictionary.TryGetValue(stat, out StatData data))
        {
            Debug.LogError($"Error: {stat} is not set in StatSO");
            return 0f;
        }

        return data.StatValue;
    }

    /// 지정된 스탯에 수정치 추가
    /// <param name="stat">수정할 스탯</param>
    /// <param name="modifier">추가할 수정자</param>
    public void AddStatModifier(Stat stat, StatModifier modifier)
    {
        if (!InGameStatSO.StatDictionary.ContainsKey(stat))
        {
            Debug.LogError($"Error: {stat} does not exist in StatDictionary");
            return;
        }

        InGameStatSO.StatDictionary[stat].AddModifier(modifier);
        InvokeStatChangeEvent(stat);
    }

    /// 지정된 스탯에서 수정치 제거
    /// <param name="stat">제거할 스탯</param>
    /// <param name="modifier">제거할 수정자</param>
    public void RemoveStatModifier(Stat stat, StatModifier modifier)
    {
        if (!InGameStatSO.StatDictionary.TryGetValue(stat, out StatData data))
        {
            Debug.LogError($"Error: {stat} does not exist in StatDictionary");
            return;
        }

        data.RemoveModifier(modifier);
        InvokeStatChangeEvent(stat);
    }

    /// 지정된 소스에서 모든 수정치 제거
    /// <param name="stat">제거할 스탯</param>
    /// <param name="source">제거할 소스</param>
    public void RemoveAllModifiersFromSource(Stat stat, object source)
    {
        if (!InGameStatSO.StatDictionary.TryGetValue(stat, out StatData data))
        {
            Debug.LogError($"Error: {stat} does not exist in StatDictionary");
            return;
        }

        data.RemoveAllModifiersFromSource(source);
        InvokeStatChangeEvent(stat);
    }

    /// 스탯 변경 이벤트 호출
    /// <param name="stat">변경된 스탯</param>
    private void InvokeStatChangeEvent(Stat stat)
    {
        if (statChangeActions.TryGetValue(stat, out Action<StatData> action))
        {
            action?.Invoke(baseStatSO.StatDictionary[stat]);
        }
        else
        {
            Debug.LogWarning($"No event associated with stat: {stat}");
        }
    }
}
