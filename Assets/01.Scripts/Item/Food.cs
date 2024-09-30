using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    public override void Use()
    {
        Eat();
    }

    private void Eat()
    {
        FoodSO foodSO = (FoodSO)_itemSO;

        Debug.Log($"Eat. hp, hunger, stam recoverAmount : {foodSO.HealthRecoveryAmount} " +
            $"{foodSO.HungerRecoveryAmount} {foodSO.StaminaRecoveryAmount}");
    }
}
