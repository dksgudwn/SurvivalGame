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
        WeaponSO weaponSO = _itemSO as WeaponSO;

        if (weaponSO == null)
        {
            Debug.LogError($"{name} is not WeaponSO");
        }

        Debug.Log($"Attack. dmg : {weaponSO.Damage}");
    }
}
