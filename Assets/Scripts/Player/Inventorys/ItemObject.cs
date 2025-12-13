using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;

    SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _sr.sprite = _itemData.DropIcon;
        gameObject.name = _itemData.Name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.TryGetComponent(out Player player))
            {
                if (InventoryController.Instance.AddItem(_itemData))
                {
                    Destroy(gameObject);
                }
                
            }
        }
    }
}
