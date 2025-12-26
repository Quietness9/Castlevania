using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotController : SlotController, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    [SerializeField] TextMeshProUGUI _itemText;

    private void OnEnable()
    {
        UpdateSlotCountHandle();
    }

    private void Start()
    {
        if(InventoryController.Instance != null)
        {
            InventoryController.Instance.OnUpdateInventoryCount += UpdateSlotCountHandle;
        }
    }

    /// <summary>
    /// 设置库存槽数据
    /// </summary>
    /// <param name="inventoryItem"></param>
    public override void SetSlotData(InventoryItem inventoryItem)
    {
        if (inventoryItem == null)
        {
            itemIcon.sprite = emptySprite;
            return;
        }

        InventoryItemData = inventoryItem;

        itemIcon.sprite = inventoryItem.ItemData.InventoryIcon;

        UpdateSlotCountHandle();
    }

    /// <summary>
    /// 装备装备
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if(InventoryItemData ==null|| InventoryItemData.ItemData==null) 
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            InventoryController.Instance.DropDirItem(InventoryItemData);
            DefaultSet();
            return;
        }

        if (InventoryItemData.ItemData.Type != ItemType.Equipment)
            return;

        MenuController.Instance.CharacterUI.EquipWeapons(InventoryItemData);
    }

    /// <summary>
    /// 更新单个栏的数量
    /// </summary>
    private void UpdateSlotCountHandle()
    {
        if (InventoryItemData == null)
            return;

        if (InventoryItemData.GetCount() > 1)
        {
            _itemText.text = "" + InventoryItemData.GetCount();
        }
        else
        {
            _itemText.text = "";
        }

        if (InventoryItemData.GetCount() <= 0)
        {
            itemIcon.sprite = emptySprite;
            InventoryItemData = null;
        }
    }

    /// <summary>
    /// 回到存储栏默认设置
    /// </summary>
    private void DefaultSet()
    {
        _itemText.text = "";
        itemIcon.sprite = emptySprite;
        InventoryItemData =null;
    }

    /// <summary>
    /// 鼠标划入显示介绍
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InventoryItemData == null)
            return;

        MenuController.Instance.ItemTip.ShowTipContent(InventoryItemData.ItemData);

        //以下功能为提示跟随鼠标
        float offsetX = 0;
        float offsetY = 0;

        Vector2 mousePosition = Input.mousePosition;

        offsetX = 75;
        if (mousePosition.x > 560)
        {
            offsetX = -75;
        }

        offsetY = 40;
        if (offsetY > 600)
        {
            offsetY = -75;
        }

        MenuController.Instance.ItemTip.transform.position = new Vector3(mousePosition.x + offsetX, mousePosition.y + offsetY, 0);
    }

    /// <summary>
    /// 鼠标化出关闭介绍
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        MenuController.Instance.ItemTip.HideTipContent();
    }
}
