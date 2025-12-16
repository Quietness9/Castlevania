using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    [SerializeField] Vector2 _jumpForceX;
    [SerializeField] Vector2 _jumpForceY;
    
    Rigidbody2D _rb;
    SpriteRenderer _sr;
    ItemData _itemData;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    public void SetItemData(ItemData itemData)
    {
        if(itemData == null)
        {
            Debug.LogWarning("掉落物品数据为null");
            return;
        }

        _itemData = itemData;
        _sr.sprite = itemData.ShowIcon;

        int direction = 1;
        if (Random.value > 0.5)
        {
            direction = -1;
        }

        float forceX=Random.Range(_jumpForceX.x, _jumpForceX.y);
        float forceY=Random.Range(_jumpForceY.x, _jumpForceY.y);

        _rb.velocity = new Vector3(direction*forceX, forceY, 0);
    }

    /// <summary>
    /// 捡起物品
    /// </summary>
    public void PuckUpItem()
    {
        if (InventoryController.Instance.AddItem(_itemData))
        {
            Destroy(gameObject);
        }
    }

    
}
