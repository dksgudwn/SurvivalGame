using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public struct ItemModifierData
{
    public Stat ModifierStat;
    public float ModifierValue;
    public bool IsPercent;

    public StatModifier PackgingValues(object source)
    {
        return new StatModifier(ModifierValue, IsPercent, source);
    }
}

public enum ArmorPartsTag
{
    Boots = 0,
    Chest,
    Gloves,
    Helmet,
    Pants,
    Shoulders,
    End
}

public class Armor : MonoBehaviour
{
    public GameObject ArmorObject => gameObject;

    [Header("Armor Values")]
    public string ArmorKind;
    public ArmorPartsTag ArmorTag;

    [Header("Armor Modifier")]
    public List<ItemModifierData> ArmorModifiers;

    public event Action<object, List<ItemModifierData>> OnEquipArmor;
    public event Action<object, List<ItemModifierData>> OnUnEquipArmor;

    private bool IsAlreadyEquiped => gameObject.activeSelf;

    public string ArmorFullName()
    {
        StringBuilder armorName = new StringBuilder();
        armorName.Append(ArmorKind).Append("_");
        armorName.Append(ArmorTag);

        return armorName.ToString();
    }

    public void EquipArmor(bool IsEquip)
    {
        if(IsAlreadyEquiped != IsEquip)
        {
            if(IsEquip == true)
            {
                OnEquipArmor?.Invoke(this, ArmorModifiers);
            }
            if(IsEquip == false)
            {
                OnUnEquipArmor?.Invoke(this, ArmorModifiers);
            }

            ArmorObject.SetActive(IsEquip);
        }
    }

}
