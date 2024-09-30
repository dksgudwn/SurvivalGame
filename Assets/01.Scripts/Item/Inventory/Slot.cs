using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private SlotItem _slotItem = null;

    public bool IsEmpty { get { return _slotItem == null; } }

    public void SetItem(SlotItem slotItem)
    {
        _slotItem = slotItem;
    }

    public void DiscardItem()
    {
        _slotItem = null;
    }
}
