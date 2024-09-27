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
	}

	private bool isDirty = true;
	private float _value;
	public float Value
	{
		get 
		{
			if (isDirty)
			//���� ��ȭ�ߴٸ�?
			{
				_value = CalculateFinalValue();
				//�ٽ� ����� ���� �Ҵ�
				isDirty = false;
				//��ȭ���� ���� ���·� ����
			}
			
			return _value;
			//����� �� ��ȯ
		}
	}

	public void AddModifier(StatModifier modifier)
	{
		isDirty = true;
		statModifiers.Add(modifier);
	}

	public bool RemoveModifier(StatModifier mod)
	{
		if (statModifiers.Remove(mod))
		{
			isDirty = true;
			return true;
		}

		Debug.LogError("Error: Modifier is not removed");
		return false;
	}

	private float CalculateFinalValue()
	{
		float finalValue = baseValue;
		float percentSum = 1f;
		// �⺻ �� ����
		StatModifier modifier;
		// �ݺ� ���� ����
		
		for (int count = 0; count < statModifiers.Count; count++)
		// ���� StatModifier�� ����ŭ �ݺ� 
		{
			modifier = statModifiers[count];
			if (modifier._IsPercent == true)
				percentSum += 0.01f * modifier._Value;
			// �ۼ�Ʈ ��ġ�� �´ٸ� ������� ��ȯ�� �ۼ�Ʈ ���� �߰�
			else if (modifier._IsPercent == false)
				finalValue += modifier._Value;
			// �ۼ�Ʈ ��ġ�� �ƴ϶�� �ٷ� finalValue�� ���ϱ�
		}

		return (float)Math.Round(finalValue * percentSum, 3);
		// �⺻ �� + ���� ��ġ ���� ������ �� �Ҽ��� 3�ڸ����� ó��
	}
}

public class StatModifier
{
	public readonly float _Value;
	public readonly bool _IsPercent;

	public StatModifier(float Value, bool IsPercent)
	{
		_Value = Value;
		_IsPercent = IsPercent;
	}
}

[CreateAssetMenu(menuName = "SO/Player/PlayerStat", fileName = "New Player Stat")]
public class PlayerStatSO : ScriptableObject
{
	public List<StatData> Stats;

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
}
