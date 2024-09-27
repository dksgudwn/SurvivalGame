using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/Food")]
public class FoodSO : ItemSO
{
    public float HealthRecoveryAmount;
    public float HungerRecoveryAmount;
    public float StaminaRecoveryAmount;
}
