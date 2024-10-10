using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerEquip : MonoBehaviour, IPlayerComponent
{
    private Player _player;
    private PlayerStatus Status;

    private Dictionary<ArmorPartsTag, Armor> PlayerEquipArmor = new Dictionary<ArmorPartsTag, Armor>();

    private Dictionary<string, Armor> PlayerArmors = new Dictionary<string, Armor>();

    public void Initialize(Player player)
    {
        _player = player;
        Status = player.GetCompo<PlayerStatus>();

        PlayerArmors = new Dictionary<string, Armor>();

        foreach (var armor in GetComponentsInChildren<Armor>())
        {
            armor.EquipArmor(false);
            PlayerArmors.TryAdd(armor.ArmorFullName(), armor); // C# 7.3 �̻󿡼��� ��� ����

            // �̺�Ʈ ����
            armor.OnEquipArmor += OnAddModifier;
            armor.OnUnEquipArmor += OnRemoveModifier;
        }
    }

    public void AfterInitialize()
    {
        PlayerEquipArmor = new Dictionary<ArmorPartsTag, Armor>();

        for (int count = 0; count < (int)ArmorPartsTag.End; count++)
        {
            PlayerEquipArmor.Add((ArmorPartsTag)count, null);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CheckEquipArmor("Starter", (ArmorPartsTag)Random.Range(0, (int)ArmorPartsTag.End), true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckEquipArmor("PlateSet", (ArmorPartsTag)Random.Range(0, (int)ArmorPartsTag.End), true);
        }
    }

    public bool CheckEquipArmor(string armorKind, ArmorPartsTag part, bool IsEquip)
    {
        var armorName = new StringBuilder();
        armorName.Append(armorKind).Append("_").Append(part);

        if (PlayerArmors.TryGetValue(armorName.ToString(), out Armor BeEquipArmor) == true)
        {
            if (IsEquip == true) // �����ϴ� ��쿡
            {
                if(PlayerEquipArmor[part] != BeEquipArmor) // ������ ��� ���� ���� ���� ��� �ƴ϶��
                {
                    if (PlayerEquipArmor[part] != null) // �̹� �ش� ���Կ� ������ �Ǿ� �ִٸ�
                    {
                        PlayerEquipArmor[part].EquipArmor(false); // ���� ����
                        PlayerEquipArmor[part] = null; // ���� ���� ������ �ʱ�ȭ
                    }

                    PlayerEquipArmor[part] = BeEquipArmor; // ������ ������ ����
                }

            }
            else if (IsEquip == false) PlayerEquipArmor[part] = null;

            BeEquipArmor.EquipArmor(IsEquip);
            Debug.Log("Equip " + armorName.ToString());
            return true;
        }

        Debug.Log("Can't find " + armorName.ToString());
        return false;
    }

    private void OnAddModifier(object source, List<ItemModifierData> mods)
    {
        foreach(ItemModifierData mod in mods)
        {
            Status.AddStatModifier(mod.ModifierStat, mod.PackgingValues(source));
        }
    }

    private void OnRemoveModifier(object source, List<ItemModifierData> mods)
    {
        foreach (ItemModifierData mod in mods)
        {
            Status.RemoveAllModifiersFromSource(mod.ModifierStat, mod.PackgingValues(source));
        }
    }
}
