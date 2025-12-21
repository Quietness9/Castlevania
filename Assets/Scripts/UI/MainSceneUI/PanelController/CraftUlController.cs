using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftUlController : MonoBehaviour
{
    [SerializeField] Image _craftImage;
    [SerializeField] Transform _materialSlotParent;
    [SerializeField] TextMeshProUGUI _craftNameText;
    [SerializeField] TextMeshProUGUI _craftDescriptionText;
    [SerializeField] EquipmentItemData _defaultShowData;

    CraftSlotController[] _materialSlots;

    private void Awake()
    {
        InitCraftUI();
    }

    /// <summary>
    /// 初始化制作界面
    /// </summary>
    private void InitCraftUI()
    {
        _materialSlots=_materialSlotParent.GetComponentsInChildren<CraftSlotController>();
        for(int i=0;i< _materialSlots.Length; i++)
        {
            _materialSlots[i].gameObject.SetActive(false);
        }

        SetMaterialSlotData(new InventoryItem(_defaultShowData));

        _craftImage.gameObject.SetActive(true);
        _craftNameText.gameObject.SetActive(true);
        _craftDescriptionText.gameObject.SetActive(true);

    }

    /// <summary>
    /// 设置材料槽数据
    /// </summary>
    /// <param name="inventoryItem"></param>
    public void SetMaterialSlotData(InventoryItem inventoryItem)
    {
        if (inventoryItem == null || inventoryItem.ItemData == null)
            return;

        EquipmentItemData equipmentItemData= inventoryItem.ItemData as EquipmentItemData;

        _craftImage.sprite=equipmentItemData.ShowIcon;
        _craftNameText.text=equipmentItemData.Name;
        _craftDescriptionText.text=equipmentItemData.GetDescription();

        for(int i = 0; i < equipmentItemData.CraftMaterial.Count; i++)
        {
            _materialSlots[i].SetSlotData(equipmentItemData.CraftMaterial[i].MaterialData, equipmentItemData.CraftMaterial[i].Count);
            _materialSlots[i].gameObject.SetActive(true);
        }

        for(int i= equipmentItemData.CraftMaterial.Count; i < _materialSlots.Length; i++)
        {
            _materialSlots[i].gameObject.SetActive(false);
        }

    }

}
