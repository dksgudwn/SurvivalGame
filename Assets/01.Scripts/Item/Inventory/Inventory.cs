using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    private List<Slot> _slots = new List<Slot>();

    public void PickupItem(Item item, int amount)
    {


        foreach (var slot in _slots)
        {
            if (slot.IsEmpty)
            {

            }
        }
    }
}
