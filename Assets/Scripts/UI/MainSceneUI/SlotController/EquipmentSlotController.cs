using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotController : SlotController, IPointerDownHandler
{

    /// <summary>
    /// 设置展示武器槽数据
    /// </summary>
    /// <param name="inventoryItem"></param>
    public override void SetSlotData(InventoryItem inventoryItem)
    {
        InventoryItemData = inventoryItem;
        itemIcon.sprite = inventoryItem.ItemData.ShowIcon;
    }

    /// <summary>
    /// 卸载武器
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (InventoryItemData.ItemData == null)
            return;

        EquipmentItemData equipmentItemData = InventoryItemData.ItemData as EquipmentItemData;

        InventoryController.Instance.AddItem(InventoryItemData.ItemData);
        GlobalReferencesManager.Instance.GamePlayer.Attribute.RemoveEquipmentModifier(equipmentItemData);

        itemIcon.sprite = _emptySprite;
        InventoryItemData.ItemData = null;
    }

    
}
