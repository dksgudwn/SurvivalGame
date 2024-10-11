using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemSO _itemSO;
    public ItemSO ItemSO => _itemSO;

    public virtual void Use()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform trm = collision.transform;
        if (trm.TryGetComponent<TestPlayer>(out TestPlayer testPlayer))
        {
            testPlayer.GetItem(this);
        }
    }
}
