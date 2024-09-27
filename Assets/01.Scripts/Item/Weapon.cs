using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public override void Use()
    {
        Attack();
    }

    private void Attack()
    {
        WeaponSO weaponSO = (WeaponSO)_itemSO;

        Debug.Log($"Attack. dmg : {weaponSO.Damage}");
    }
}
