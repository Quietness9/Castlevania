using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour, IPointerDownHandler
{
    public InventoryItem InventoryItem {  get; private set; }

    Image _itemIcon;
    Sprite _emptySprite;
    TextMeshProUGUI _itemText;

    private void Awake()
    {
        _itemIcon = GetComponent<Image>();
        _itemText = GetComponentInChildren<TextMeshProUGUI>();

        _emptySprite=_itemIcon.sprite;
    }

    private void Start()
    {
        if(InventoryController.Instance != null)
        {
            InventoryController.Instance.OnUpdateInventoryCount += UpdateInventorySlotCount;
        }
    }

    /// <summary>
    /// 设置库存槽数据
    /// </summary>
    /// <param name="inventoryItem"></param>
    public void SetInventorySlotData(InventoryItem inventoryItem)
    {
        if(inventoryItem == null)
        {
            _itemIcon.sprite= _emptySprite;
            return;
        }

        InventoryItem = inventoryItem;

        _itemIcon.sprite = inventoryItem.ItemData.InventoryIcon;

        UpdateInventorySlotCount();
    }

    /// <summary>
    /// 装备装备
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if(InventoryItem==null||InventoryItem.ItemData.Type!=ItemType.Equipment) 
            return;

        MenuController.Instance.CharacterUI.EquipWeapons(InventoryItem);

        if (InventoryItem.GetCount()<=0)
        {
            _itemIcon.sprite = _emptySprite;
            InventoryItem = null;
        }
    }

    /// <summary>
    /// 更新单个栏的数量
    /// </summary>
    private void UpdateInventorySlotCount()
    {
        if (InventoryItem == null)
            return;

        if (InventoryItem.GetCount() > 1)
        {
            _itemText.text = "" + InventoryItem.GetCount();
        }
        else
        {
            _itemText.text = "";
        }
    }
}
