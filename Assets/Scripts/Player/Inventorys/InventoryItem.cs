using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    [SerializeField] ItemData _itemData;
    [SerializeField] int _itemCount;

    public InventoryItem(ItemData data)
    {
        _itemData = data;
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

}
