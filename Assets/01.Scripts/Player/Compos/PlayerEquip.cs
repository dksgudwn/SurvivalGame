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
            PlayerArmors.TryAdd(armor.ArmorFullName(), armor); // C# 7.3 이상에서만 사용 가능

            // 이벤트 구독
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
            if (IsEquip == true) // 장착하는 경우에
            {
                if(PlayerEquipArmor[part] != BeEquipArmor) // 장착된 장비가 현재 장비와 같은 장비가 아니라면
                {
                    if (PlayerEquipArmor[part] != null) // 이미 해당 슬롯에 장착이 되어 있다면
                    {
                        PlayerEquipArmor[part].EquipArmor(false); // 장착 해제
                        PlayerEquipArmor[part] = null; // 장착 부위 데이터 초기화
                    }

                    PlayerEquipArmor[part] = BeEquipArmor; // 장착할 데이터 삽입
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
