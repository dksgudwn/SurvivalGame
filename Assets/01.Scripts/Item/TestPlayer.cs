using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private Item _curItem;
    public void GetItem(Item item)
    {
        _curItem = item;
        Debug.Log($"Get Item : {item}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _curItem.Use();
        }
    }
}
