using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEditor.Progress;

public class InventoryController : MonoSingleton<InventoryController>
{

    public List<InventoryItem> Items=new(); //方便查看，可以删除(全部物品)
    [SerializeField] List<ItemData> _startItemData=new();

    public Dictionary<int, InventoryItem> EquipmentItemDir = new();
    public Dictionary<int, InventoryItem> MaterialItemDIr = new();

    public event Action OnUpdateInventoryCount = delegate { };

    [SerializeField] int InventoryItemUpperLimit=999; //单个栏上限
    [SerializeField] int EquipmentSlotUpperLimit = 10; //存放栏上限
    [SerializeField] int MaterialSlotUpperLimit = 10; //存放栏上限


    [Header("库存UI")]
    [SerializeField] Transform _equipmentSlotParent;
    [SerializeField] Transform _materialSlotParent;
    InventorySlotController[] _equipmentItemSlots;
    InventorySlotController[] _materialItemSlots;

    [Header("展示UI")]
    [SerializeField] Transform _showEquipmentSlotParent;
    EquipmentSlotController[] _showEquipmentItemSlots;

    //装备冷却计时
    float _weaponCooldownTimer;
    float _armorCooldownTimer;
    float _amuletCooldownTimer;
    float _flaskCooldownTimer;

    private void Start()
    {
        InitInventory();
    }

    private void Update()
    {
        _weaponCooldownTimer-= Time.deltaTime;
        _amuletCooldownTimer-= Time.deltaTime;
        _armorCooldownTimer-= Time.deltaTime;
        _flaskCooldownTimer-=Time.deltaTime;
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
        _materialItemSlots= null;
        _showEquipmentItemSlots= null;

    }

    /// <summary>
    /// 初始化库存
    /// </summary>
    private void InitInventory()
    {  
        _equipmentItemSlots=_equipmentSlotParent.GetComponentsInChildren<InventorySlotController>();
        _materialItemSlots = _materialSlotParent.GetComponentsInChildren<InventorySlotController>();
        _showEquipmentItemSlots=_showEquipmentSlotParent.GetComponentsInChildren<EquipmentSlotController>();

        for(int i = 0; i < _startItemData.Count; i++)
        {
            AddItem(_startItemData[i]);
        }
    }

    /// <summary>
    /// 制作装备
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool IsCraftEquipment(EquipmentItemData data)
    {
        List<InventoryItem> needMaterial = new();

        //获得并判断材料是否足够
        for (int i = 0; i < data.CraftMaterial.Count; i++)
        {
            if (MaterialItemDIr.TryGetValue(data.CraftMaterial[i].MaterialData.Id, out InventoryItem inventoryItem))
            {
                if (inventoryItem.GetCount() >= data.CraftMaterial[i].Count)
                {
                    needMaterial.Add(inventoryItem);
                }
            }
            else
            {
                Debug.Log("缺少材料"+ data.CraftMaterial[i].MaterialData.Name);
                needMaterial.Clear();
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
    /// 获得不同类型的装备
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public EquipmentItemData GetEquipment(EquipmentItemType type)
    {
        EquipmentItemData resultData = null;

        for(int i=0; i < _showEquipmentItemSlots.Length; i++)
        {
            resultData = _showEquipmentItemSlots[i].InventoryItemData?.ItemData as EquipmentItemData;
            if (resultData != null && resultData.EquipmentType == type)
            {
                return resultData;
            }
        }

        return resultData;
    }

    /// <summary>
    /// 整理材料库存UI槽数据
    /// </summary>
    public void OrganizeInventoryData(Dictionary<int,InventoryItem> dir, InventorySlotController[] slots)
    {
        int index = 0;
        foreach(var item  in dir)
        {
            slots[index].SetSlotData(item.Value);
            index++;
        }

        for (int i = dir.Count; i < slots.Length; i++)
        {
            slots[i].SetSlotData(null);
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
    /// 玩家掉落物品
    /// </summary>
    /// <param name="item"></param>
    public void DropDirItem(InventoryItem item)
    {
        if(item.ItemData.Type == ItemType.Equipment)
        {
            EquipmentItemDir.Remove(item.ItemData.Id);
            OrganizeInventoryData(EquipmentItemDir, _equipmentItemSlots);
        }

        if(item.ItemData.Type== ItemType.Material)
        {
            MaterialItemDIr.Remove(item.ItemData.Id);
            OrganizeInventoryData(MaterialItemDIr, _materialItemSlots);
        }

        Items.Remove(item);
        
    }

    #region 辅助函数

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
        }
        else
        {
            InventoryItem inventoryItem = new InventoryItem(item);
            Items.Add(inventoryItem);
            dir.Add(item.Id, inventoryItem);

            UpdateInventoryData(inventoryItem);
        }

        OnUpdateInventoryCount.Invoke();

        return true;
    }

    /// <summary>
    /// 从容器中减少物品
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
            }

            OnUpdateInventoryCount.Invoke();
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
                if (_equipmentItemSlots[i].InventoryItemData == null|| _equipmentItemSlots[i].InventoryItemData.ItemData == null)
                {
                    _equipmentItemSlots[i].SetSlotData(inventoryItem);
                    return;
                }
            }

        }

        if(inventoryItem.ItemData.Type == ItemType.Material)
        {
            for (int i = 0; i < _materialItemSlots.Length; i++)
            {
                if (_materialItemSlots[i].InventoryItemData == null||_materialItemSlots[i].InventoryItemData.ItemData == null)
                {
                    _materialItemSlots[i].SetSlotData(inventoryItem);
                    return;
                }
            }
        }
        
    }

    /// <summary>
    /// 判断是否可以使用武器
    /// </summary>
    /// <param name="type"></param>
    /// <param name="equipment"></param>
    /// <returns></returns>
    public bool CanUseEquipment(EquipmentItemType type,EquipmentItemData equipment)
    {
        switch (type)
        {
            case EquipmentItemType.Weapon: 
                {
                    if (_weaponCooldownTimer <= 0)
                    {
                        _weaponCooldownTimer=equipment.Cooldown;
                        return true;
                    }
                } break;
            case EquipmentItemType.Armor: 
                {
                    if (_armorCooldownTimer <= 0)
                    {
                        _armorCooldownTimer=equipment.Cooldown;
                        return true;
                    }
                } break;
            case EquipmentItemType.Amulet: 
                { 
                    if(_amuletCooldownTimer <= 0)
                    {
                        _amuletCooldownTimer=equipment.Cooldown;
                        return true;
                    }
                } break;
            case EquipmentItemType.Flask: 
                {
                    if (_flaskCooldownTimer <= 0)
                    {
                        _flaskCooldownTimer=equipment.Cooldown;
                        return true;
                    }
                } break;
        }

        Debug.Log("装备效果还在冷却中" + equipment.name);
        return false;
    }

    #endregion

}
