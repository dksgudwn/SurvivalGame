using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemCountTxt;



    public void SetUI(Item item, int count)
    {
        _itemImage.sprite = item.ItemSO.Sprite;

        _itemCountTxt.text = count.ToString();
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
