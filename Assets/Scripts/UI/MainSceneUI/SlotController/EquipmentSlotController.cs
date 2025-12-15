using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotController : MonoBehaviour, IPointerDownHandler
{
    public InventoryItem EqInventoryItem { get; private set; }

    Image _itemIcon;
    Sprite _emptySprite;

    private void Awake()
    {
        _itemIcon = GetComponent<Image>();

        _emptySprite=_itemIcon.sprite;
    }

    /// <summary>
    /// 设置展示武器槽数据
    /// </summary>
    /// <param name="inventoryItem"></param>
    public void SetEquipmentSlotData(InventoryItem inventoryItem)
    {
        EqInventoryItem = inventoryItem;
        _itemIcon.sprite = inventoryItem.ItemData.ShowIcon;
    }

    /// <summary>
    /// 卸载武器
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (EqInventoryItem == null)
            return;

        EquipmentItemData equipmentItemData = EqInventoryItem.ItemData as EquipmentItemData;

        InventoryController.Instance.AddItem(EqInventoryItem.ItemData);
        GlobalReferencesManager.Instance.GamePlayer.Attribute.RemoveEquipmentModifier(equipmentItemData);

        _itemIcon.sprite = _emptySprite;
        EqInventoryItem= null;
    }
}
