using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIController : MonoBehaviour
{
    [Header("武器栏UI")]
    [SerializeField] Transform _equipmentMenuSlotParent;
    [SerializeField] Transform _equipmentItemSlotParent;
    List<GameObject> _equipmentItemSlots = new();
    EquipmentSlotController[] _equipmentSlotControllers;

    private void Awake()
    {
        InitCharacterUI();
    }

    private void OnDestroy()
    {
        _equipmentItemSlots?.Clear();
        _equipmentSlotControllers=null;
    }

    /// <summary>
    /// 玩家UI初始化
    /// </summary>
    private void InitCharacterUI()
    {
        for (int i = 0; i < _equipmentItemSlotParent.childCount; i++)
        {
            _equipmentItemSlots.Add(_equipmentItemSlotParent.GetChild(i).gameObject);
        }

        _equipmentSlotControllers = _equipmentMenuSlotParent.GetComponentsInChildren<EquipmentSlotController>();
    }

    /// <summary>
    /// 刷新展示武器UI数据
    /// </summary>
    public void UpdateEquipmentSlotData()
    {
        for (int i = 0; i < _equipmentMenuSlotParent.childCount; i++)
        {

            if (_equipmentSlotControllers[i].InventoryItemData!= null)
            {
                _equipmentItemSlots[i].GetComponent<Image>().sprite = _equipmentSlotControllers[i].InventoryItemData.ItemData.ShowIcon;
            }

        }
    }

    /// <summary>
    /// 装备各种武器
    /// </summary>
    /// <param name="type">装备类型</param>
    public void EquipWeapons(InventoryItem inventoryItem)
    {

        EquipmentItemData equipmentItemData = inventoryItem.ItemData as EquipmentItemData;

        if (equipmentItemData == null)
        {
            Debug.Log("equipmentItemData is null");
            return;
        }

        switch (equipmentItemData.EquipmentType)
        {
            case EquipmentItemType.Weapon: EquipWeaponHelp(0, inventoryItem, equipmentItemData); break;
            case EquipmentItemType.Armor: EquipWeaponHelp(1, inventoryItem, equipmentItemData); break;
            case EquipmentItemType.Amulet: EquipWeaponHelp(2, inventoryItem, equipmentItemData); break;
            case EquipmentItemType.Flask: EquipWeaponHelp(3, inventoryItem, equipmentItemData); break;
        }

    }

    /// <summary>
    /// 装备函数的辅助函数
    /// </summary>
    /// <param name="index"></param>
    /// <param name="inventoryItem"></param>
    private void EquipWeaponHelp(int index, InventoryItem inventoryItem, EquipmentItemData equipmentItemData)
    {
        if (_equipmentSlotControllers[index].InventoryItemData != null)
            return;

        GlobalReferencesManager.Instance.GamePlayer.Attribute.AddEquipmentModifier(equipmentItemData);
        _equipmentSlotControllers[index].SetSlotData(inventoryItem);
        InventoryController.Instance.RemoveItem(inventoryItem.ItemData);
    }

    
}
