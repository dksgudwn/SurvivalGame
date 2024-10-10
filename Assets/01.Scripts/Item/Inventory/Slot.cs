using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    //private Item _item = null;
    private SlotItem _slotItem = null;
    //private int _count = 0;

    public SlotItem GetSlotItem { get { return _slotItem; } }

    public void SpawnSlotItem(SlotItem prefab)
    {
        _slotItem = prefab;
        Instantiate(prefab, Vector2.zero, Quaternion.identity, transform);
    }
}
