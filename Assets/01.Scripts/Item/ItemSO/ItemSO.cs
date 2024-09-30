using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/DefaultItem")]
public class ItemSO : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public string Description;
    public int MaxSlot;

}
