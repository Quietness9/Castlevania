using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class InventoryController : MonoSingleton<InventoryController>
{

    public List<InventoryItem> Items; //方便查看，可以删除
    public Dictionary<int, InventoryItem> InventoryItemDir; //全部库存数据
    public event Action OnUpdateInventoryCount = delegate { };

    [SerializeField] int InventoryItemUpperLimit=999; //单个栏上限
    [SerializeField] int InventorySlotUpperLimit = 10; //存放栏上限
    

    [Header("库存UI")]
    [SerializeField] Transform _inventorySlotParent;
    [SerializeField] InventorySlotController[] _inventoryItemSlots;

    private void Start()
    {
        InitInventory();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnUpdateInventoryCount = delegate { }; 

        // 清理其他可能的引用...
        Items?.Clear();
        InventoryItemDir?.Clear();
        _inventoryItemSlots = null;
    }

    /// <summary>
    /// 初始化库存
    /// </summary>
    private void InitInventory()
    {
        Items = new();
        InventoryItemDir = new();

        _inventoryItemSlots=_inventorySlotParent.GetComponentsInChildren<InventorySlotController>();
    }

    /// <summary>
    /// 整理库存UI槽数据
    /// </summary>
    public void ReorganizeInventoryData()
    {
       if(_inventoryItemSlots==null|| _inventoryItemSlots.Length <= 0)
        {
            Debug.LogWarning("Inventory slots are not initialized or empty.");
            return;
        }

        var inventoryItems = new List<InventoryItem>(InventoryItemDir.Values);

        for(int i = 0; i < inventoryItems.Count; i++)
        {
            _inventoryItemSlots[i].SetInventorySlotData(inventoryItems[i]);
        }

        for(int i =_inventoryItemSlots.Length - inventoryItems.Count;i< _inventoryItemSlots.Length; i++)
        {
            _inventoryItemSlots[i].SetInventorySlotData(null);
        }
    }

    /// <summary>
    /// 添加库存物品
    /// </summary>
    /// <param name="item"></param>
    public bool AddItem(ItemData item)
    {
        if(InventoryItemDir.TryGetValue(item.Id, out InventoryItem value))
        {
            if (value.GetCount() >= InventoryItemUpperLimit)
            {
                Debug.Log("单个物品到达上限");
                return false;
            }
               
            value.AddCount();
            OnUpdateInventoryCount.Invoke();
        }
        else
        {
            if (InventoryItemDir.Count >= InventorySlotUpperLimit)
            {
                Debug.Log("物品栏已经到达上限");
                return false;
            }

            InventoryItem inventoryItem = new InventoryItem(item);
            Items.Add(inventoryItem);
            InventoryItemDir.Add(item.Id, inventoryItem);

            UpdateInventoryData(inventoryItem);
        }

        //ReorganizeInventoryData();

        return true;
    }

    /// <summary>
    /// 减少库存物品
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public void RemoveItem(ItemData item)
    {
        if( InventoryItemDir.TryGetValue(item.Id,out InventoryItem value))
        {
            if (value.GetCount()==1)
            {
                InventoryItemDir.Remove(item.Id);
                Items.Remove(value);
                value.RemoveCount();
            }
            else
            {
                value.RemoveCount();
                OnUpdateInventoryCount.Invoke();
            }
        }
        else
        {
            Debug.LogWarning("库存中没有此物体" + item.name);
        }
    }

    /// <summary>
    /// 更新库存数据
    /// </summary>
    /// <param name="inventoryItem"></param>
    private void UpdateInventoryData(InventoryItem inventoryItem)
    {
        for(int i = 0; i < _inventoryItemSlots.Length; i++)
        {
            if( _inventoryItemSlots[i].InventoryItem== null)
            {
                _inventoryItemSlots[i].SetInventorySlotData(inventoryItem);
                return;
            }
        }
    }
}
