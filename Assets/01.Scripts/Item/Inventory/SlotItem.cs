using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IDropHandler
{
    public Item Item { get; private set; }

    public void Initialize(Item item)
    {
        Item = item;
        var image = GetComponent<Image>();
        image.sprite = Item.ItemSO.Sprite;
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
