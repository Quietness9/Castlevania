using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData ItemData;
    [SerializeField] int _itemCount;

    public InventoryItem(ItemData data)
    {
        ItemData = data;
        _itemCount = 1;
    }
    
    /// <summary>
    /// 增加数量
    /// </summary>
    public void AddCount()=>_itemCount++;

    /// <summary>
    /// 减少数量
    /// </summary>
    public void RemoveCount()=>_itemCount--;

    /// <summary>
    /// 获得数量
    /// </summary>
    /// <returns></returns>
    public int GetCount()=> _itemCount;

    /// <summary>
    /// 设置数量（当需要一个图标获得多个物品时使用）
    /// </summary>
    /// <param name="count"></param>
    public void SetCount(int count)=>_itemCount=count;
    
}
