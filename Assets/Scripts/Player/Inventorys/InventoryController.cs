using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEditor.Progress;

public class InventoryController : MonoSingleton<InventoryController>
{

    public List<InventoryItem> Items; //方便查看，可以删除(全部物品)

    public Dictionary<int, InventoryItem> EquipmentItemDir;
    public Dictionary<int, InventoryItem> MaterialItemDIr;

    public event Action OnUpdateInventoryCount = delegate { };

    [SerializeField] int InventoryItemUpperLimit=999; //单个栏上限
    [SerializeField] int EquipmentSlotUpperLimit = 10; //存放栏上限
    [SerializeField] int MaterialSlotUpperLimit = 10; //存放栏上限


    [Header("库存UI")]
    [SerializeField] Transform _equipmentSlotParent;
    [SerializeField] Transform _materialSlotParent;
    [SerializeField] InventorySlotController[] _equipmentItemSlots;
    [SerializeField] InventorySlotController[] _materialItemSlots;
    
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
        EquipmentItemDir?.Clear();
        MaterialItemDIr?.Clear();
        _equipmentItemSlots = null;
    }

    /// <summary>
    /// 初始化库存
    /// </summary>
    private void InitInventory()
    {
        Items = new();
        EquipmentItemDir = new ();
        MaterialItemDIr = new ();

        _equipmentItemSlots=_equipmentSlotParent.GetComponentsInChildren<InventorySlotController>();
        _materialItemSlots = _materialSlotParent.GetComponentsInChildren<InventorySlotController>();
    }

    /// <summary>
    /// 制作装备
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool CraftEquipment(EquipmentItemData data)
    {
        List<InventoryItem> needMaterial = new();

        //获得并判断材料是否足够
        for (int i = 0; i < data.CraftMaterial.Count; i++)
        {
            if (MaterialItemDIr.TryGetValue(data.CraftMaterial[i].Count, out InventoryItem inventoryItem))
            {
                if (inventoryItem.GetCount() >= data.CraftMaterial[i].Count)
                {
                    needMaterial.Add(inventoryItem);
                }
            }
            else
            {
                Debug.Log("缺少材料"+ data.CraftMaterial[i].MaterialData.Name);
                return false;
            }
        }

        for (int i = 0; i < needMaterial.Count; i++)
        {
            RemoveItem(needMaterial[i].ItemData);
        }

        needMaterial.Clear();
        Debug.Log("制作装备成功"+data.name);

        return true;
    }

    /// <summary>
    /// 整理武器库存UI槽数据
    /// </summary>
    public void ReorganizeInventoryData()
    {
       if(_equipmentItemSlots==null|| _equipmentItemSlots.Length <= 0)
        {
            Debug.LogWarning("Inventory slots are not initialized or empty.");
            return;
        }

        var inventoryItems = new List<InventoryItem>(EquipmentItemDir.Values);

        for(int i = 0; i < inventoryItems.Count; i++)
        {
            _equipmentItemSlots[i].SetInventorySlotData(inventoryItems[i]);
        }

        for(int i =_equipmentItemSlots.Length - inventoryItems.Count;i< _equipmentItemSlots.Length; i++)
        {
            _equipmentItemSlots[i].SetInventorySlotData(null);
        }
    }

    /// <summary>
    /// 添加库存物品
    /// </summary>
    /// <param name="item"></param>
    public bool AddItem(ItemData item)
    {

        if (ItemType.Equipment == item.Type)
        {
            if (EquipmentItemDir.Count >= EquipmentSlotUpperLimit)
            {
                Debug.Log("武器库存已满");
                return false;
            }

            return AddDirItem(item, EquipmentItemDir);
        }

        if(ItemType.Material == item.Type)
        {
            if(MaterialItemDIr.Count >= MaterialSlotUpperLimit)
            {
                Debug.Log("材料库存已满");
                return false;
            }

            return AddDirItem(item, MaterialItemDIr);
        }

        Debug.Log("错误");
        return false;
    }

    /// <summary>
    /// 减少库存物品
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public void RemoveItem(ItemData item)
    {

        if(ItemType.Equipment == item.Type)
        {
            RemoveDirItem(item, EquipmentItemDir);
        }

        if(ItemType.Material== item.Type)
        {
            RemoveDirItem(item,MaterialItemDIr);
        }
    }

    /// <summary>
    /// 添加物品到容器
    /// </summary>
    /// <param name="item"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    private bool AddDirItem(ItemData item,Dictionary<int, InventoryItem> dir)
    {
        if (dir.TryGetValue(item.Id, out InventoryItem value))
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
            InventoryItem inventoryItem = new InventoryItem(item);
            Items.Add(inventoryItem);
            dir.Add(item.Id, inventoryItem);

            UpdateInventoryData(inventoryItem);
        }

        return true;
    }

    /// <summary>
    /// 从容器中去除物品
    /// </summary>
    /// <param name="item"></param>
    /// <param name="dir"></param>
    private void RemoveDirItem(ItemData item,Dictionary<int,InventoryItem> dir)
    {
        if (dir.TryGetValue(item.Id, out InventoryItem value))
        {
            if (value.GetCount() == 1)
            {
                dir.Remove(item.Id);
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
        if (inventoryItem.ItemData.Type == ItemType.Equipment)
        {
            for (int i = 0; i < _equipmentItemSlots.Length; i++)
            {
                if (_equipmentItemSlots[i].InventoryItem == null)
                {
                    _equipmentItemSlots[i].SetInventorySlotData(inventoryItem);
                    return;
                }
            }

        }

        if(inventoryItem.ItemData.Type == ItemType.Material)
        {
            for (int i = 0; i < _materialItemSlots.Length; i++)
            {
                if (_materialItemSlots[i].InventoryItem == null)
                {
                    _materialItemSlots[i].SetInventorySlotData(inventoryItem);
                    return;
                }
            }
        }
        
    }

    
}
