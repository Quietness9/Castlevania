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

    EquipmentItemData _craftEquipmentData;
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

        _craftEquipmentData= inventoryItem.ItemData as EquipmentItemData;

        _craftImage.sprite= _craftEquipmentData.ShowIcon;
        _craftNameText.text= _craftEquipmentData.Name;
        _craftDescriptionText.text= _craftEquipmentData.GetDescription();

        for(int i = 0; i < _craftEquipmentData.CraftMaterial.Count; i++)
        {
            _materialSlots[i].SetSlotData(_craftEquipmentData.CraftMaterial[i].MaterialData, _craftEquipmentData.CraftMaterial[i].Count);
            _materialSlots[i].gameObject.SetActive(true);
        }

        for(int i= _craftEquipmentData.CraftMaterial.Count; i < _materialSlots.Length; i++)
        {
            _materialSlots[i].gameObject.SetActive(false);
        }

    }


    /// <summary>
    /// 制作装备
    /// </summary>
    /// <param name="data"></param>
    public void MakeEquipment()
    {
        if(_craftEquipmentData==null)
            return;

        if (!InventoryController.Instance.IsCraftEquipment(_craftEquipmentData))
            return;

        if (!InventoryController.Instance.AddItem(_craftEquipmentData))
        {
            GameObject itemObj = GlobalReferencesManager.Instance.GetPrefab("Item");
            if (itemObj != null)
            {
                GameObject dropItem = Instantiate(itemObj, transform.position, Quaternion.identity);
                dropItem.GetComponent<ItemObject>().SetItemData(_craftEquipmentData);
            }
        }
        
    }
}
