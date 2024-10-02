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
