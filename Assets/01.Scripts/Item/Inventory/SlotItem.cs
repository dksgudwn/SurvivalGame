using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IDropHandler
{
    public Item Item { get; private set; }
    public int Count { get; private set; } = 0;

    public void Initialize(Item item)
    {
        Item = item;
        var image = GetComponent<Image>();
        image.sprite = Item.ItemSO.Sprite;
    }

    public void AddItem(int count)
    {
        Count += count;
        if (Count > Item.ItemSO.MaxSlot)
        {
            int rem = Item.ItemSO.MaxSlot - Count;
            Count = Item.ItemSO.MaxSlot;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
