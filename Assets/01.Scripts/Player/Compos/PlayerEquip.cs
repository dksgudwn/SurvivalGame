using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerEquip : MonoBehaviour, IPlayerComponent
{
    private Player _player;
    private PlayerStatus Status;

    private Dictionary<ArmorPartsTag, Armor> PlayerEquipArmor = new Dictionary<ArmorPartsTag, Armor>();

    private Dictionary<string, Armor> PlayerArmors = new Dictionary<string, Armor>();

    private StringBuilder ArmorName = new StringBuilder();

    public void Initialize(Player player)
    {
        _player = player;
        Status = player.GetCompo<PlayerStatus>();

        PlayerArmors = new Dictionary<string, Armor>();

        GetComponentsInChildren<Armor>().ToList().ForEach(armor =>
        {
            armor.EquipArmor(false);

            PlayerArmors.TryAdd(armor.ArmorFullName(), armor);

            PlayerArmors[armor.ArmorFullName()].OnEquipArmor += OnAddModifier;
            PlayerArmors[armor.ArmorFullName()].OnUnEquipArmor += OnRemoveModifier;
        });
    }

    public void AfterInitialize()
    {
        ArmorName = new StringBuilder();

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
        ArmorName.Clear();
        ArmorName.Append(armorKind).Append("_");
        ArmorName.Append(part);

        if (PlayerArmors.TryGetValue(ArmorName.ToString(), out Armor BeEquipArmor) == true)
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
            Debug.Log("Equip " + ArmorName.ToString());
            return true;
        }

        Debug.Log("Can't find " + ArmorName.ToString());
        return false;
    }

    private void OnAddModifier(Stat modifyStat, StatModifier modifier)
    {
        Status.AddStatModifier(modifyStat, modifier);
    }

    private void OnRemoveModifier(Stat modifyStat, StatModifier modifier)
    {
        Status.RemoveAllModiiferInSource(modifyStat, modifier);
    }
}
