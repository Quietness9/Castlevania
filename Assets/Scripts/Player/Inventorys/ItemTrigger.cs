using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    ItemObject _itemObj=>GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (!player.Attribute.IsDie)
            {
                _itemObj.PuckUpItem();
            }
        }
    }
}
