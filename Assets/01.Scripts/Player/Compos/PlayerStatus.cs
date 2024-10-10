using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IPlayerComponent
{
    private Player _player;

    [Header("PlayerStat")]
    [SerializeField] private PlayerStatSO baseStatSO; // ���� ������ �� ����� �÷��̾� �⺻ ����
    [SerializeField] private LayerMask interactionLayer;

    // �ǻ���� �÷��̾� ����
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

        // �⺻ �ɷ�ġ ���� �� �ʱ�ȭ
        InGameStatSO = Instantiate(baseStatSO);
        InGameStatSO.InitializeStatDictionary();
    }

    // StatChangeActions �ʱ�ȭ
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

    /// ������ ������ ���� �� ��ȯ
    /// <param name="stat">������ ����</param>
    /// <returns>������ ���� ��</returns>
    public float GetStatValue(Stat stat)
    {
        if (!InGameStatSO.StatDictionary.TryGetValue(stat, out StatData data))
        {
            Debug.LogError($"Error: {stat} is not set in StatSO");
            return 0f;
        }

        return data.StatValue;
    }

    /// ������ ���ȿ� ����ġ �߰�
    /// <param name="stat">������ ����</param>
    /// <param name="modifier">�߰��� ������</param>
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

    /// ������ ���ȿ��� ����ġ ����
    /// <param name="stat">������ ����</param>
    /// <param name="modifier">������ ������</param>
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

    /// ������ �ҽ����� ��� ����ġ ����
    /// <param name="stat">������ ����</param>
    /// <param name="source">������ �ҽ�</param>
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

    /// ���� ���� �̺�Ʈ ȣ��
    /// <param name="stat">����� ����</param>
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
