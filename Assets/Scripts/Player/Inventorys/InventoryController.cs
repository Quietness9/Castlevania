using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoSingleton<InventoryController>
{
    public List<InventoryItem> Items; //方便查看，可以删除
    public Dictionary<int, InventoryItem> InventoryItemDir;

    public void Start()
    {
        Items = new ();
        InventoryItemDir = new ();
    }

    /// <summary>
    /// 添加库存物品
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(ItemData item)
    {
        if(InventoryItemDir.TryGetValue(item.Id, out InventoryItem value))
        {
            value.AddCount();
        }
        else
        {
            InventoryItem inventoryItem = new InventoryItem(item);
            Items.Add(inventoryItem);
            InventoryItemDir.Add(item.Id, inventoryItem);
        }
    }

    /// <summary>
    /// 减少库存物品
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int RemoveItem(ItemData item)
    {
        if( InventoryItemDir.TryGetValue(item.Id,out InventoryItem value))
        {
            if (value.GetCount()<=1)
            {
                InventoryItemDir.Remove(item.Id);
                Items.Remove(value);
            }
            else
            {
                value.RemoveCount();
            }

            return 1;
        }
        else
        {
            Debug.LogWarning("库存中没有此物体" + item.name);
            return -1;
        }
    }
}
