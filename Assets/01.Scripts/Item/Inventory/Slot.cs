using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private SlotItem _slotItemPrefab;
    private Item _item = null;
    private SlotItem _slotItem = null;
    private int _count = 0;

    public bool IsEmpty { get { return _item == null; } }
    public bool IsFull { get { return _count >= _item.ItemSO.MaxSlot; } }
    public Item GetItem { get { return _item; } }


    public void SetItem(Item slotItem, int count)
    {
        _item = slotItem;
        _slotItem = Instantiate(_slotItemPrefab, 
            Vector2.zero, 
            Quaternion.identity, 
            transform);
        _slotItem.SetUI(_item, count);
    }

    public void AddItem(int count)
    {
        _count += count;
        if (_count > _item.ItemSO.MaxSlot)
        {
            int rem = _item.ItemSO.MaxSlot - _count;
            _count = _item.ItemSO.MaxSlot;
            //

        }
    }

    public void DiscardItem()
    {
        _item = null;
        _slotItem = null;

        Destroy(_slotItem.gameObject);
    }
}
