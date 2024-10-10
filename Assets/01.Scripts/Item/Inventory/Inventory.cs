using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static UnityEditor.Progress;


public class Inventory : MonoBehaviour
{
    [SerializeField] private SlotItem _slotItemPrefab;

    [SerializeField] private List<Slot> _slots = new List<Slot>();

    public void PickupItem(Item item, int amount)
    {

        foreach (var slot in _slots)
        {
/*            if(slot.GetItem == item &&
                !slot.IsFull)
            {
                slot.AddItem(amount);
            }

            if (slot.IsEmpty)
            {
                SetItem(item, amount, slot);
            }*/
        }
    }

/*    private void SetItem(Item item, int count, Slot slot)
    {
        _item = slotItem;
        SlotItem slotItem = Instantiate(_slotItemPrefab,
            Vector2.zero,
            Quaternion.identity,
            slot.transform);
        slotItem.SetUI(_item, count);
    }*/
}
