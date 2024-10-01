using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IPlayerComponent
{
    private Player _player;

	[Header("PlayerStat")]
	[SerializeField] private PlayerStatSO BaseStatSO; //게임 시작할 때 적용될 플레이어 기본 스텟
	[HideInInspector] public PlayerStatSO InGameStatSO; //실사용할 플레이어 스텟

    #region Stat Change Events
    public event Action OnMaxHpChange;
	public event Action OnMaxHungerChange;
	public event Action OnMaxStaminaChange;
	public event Action OnArmorRatioChange;
	public event Action OnHealthRegenerationChange;
	public event Action OnStaminaRegenerationChange;
	public event Action OnStrengthChange;
	public event Action OnAttackSpeedChange;
	public event Action OnMoveSpeedChange;
    #endregion

	public Dictionary<Stat, Action> StatChangeActions = new Dictionary<Stat, Action>();

    public void Initialize(Player player)
	{
		_player = player;

        InGameStatSO = Instantiate(BaseStatSO);
        InGameStatSO.InitializeStatDictionary();
	}

    public void AfterInitialize()
    {
        StatChangeActions = new Dictionary<Stat, Action>();

		StatChangeActions.Add(Stat.MaxHp, OnMaxHpChange);
		StatChangeActions.Add(Stat.MaxHunger, OnMaxHungerChange);
		StatChangeActions.Add(Stat.MaxStamina, OnMaxStaminaChange);
		StatChangeActions.Add(Stat.ArmorRatio, OnArmorRatioChange);
		StatChangeActions.Add(Stat.HealthRegeneration, OnHealthRegenerationChange);
		StatChangeActions.Add(Stat.StaminaRegeneration, OnStaminaRegenerationChange);
		StatChangeActions.Add(Stat.Strength, OnStrengthChange);
		StatChangeActions.Add(Stat.AttackSpeed, OnAttackSpeedChange);
		StatChangeActions.Add(Stat.MoveSpeed, OnMoveSpeedChange);
    }

    public float GetStatValue(Stat getStat)
	{
		if (InGameStatSO.StatDictionary.TryGetValue(getStat, out StatData data) == false)
		{
			Debug.LogError($"Error: {getStat} is not Setted in StatSO");
			return 0f;
		}

		return data.StatValue;
    }

	public void AddStatModifier(Stat modifierStat, StatModifier modifier)
		=> InGameStatSO.StatDictionary[modifierStat].AddModifier(modifier);

    public void RemoveStatModifier(Stat modifierStat, StatModifier modifier)
    {
        if (InGameStatSO.StatDictionary.TryGetValue(modifierStat, out StatData data) == false) return;

        data.RemoveModifier(modifier);
    }
}
