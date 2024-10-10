using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO ThisItemData;

    public void DoInteractionEvent()
    {
        Debug.Log(ThisItemData.Name);
    }
}
