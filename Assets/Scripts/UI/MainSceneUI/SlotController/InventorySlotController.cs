using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour, IPointerDownHandler
{
    public InventoryItem InventoryItem {  get; private set; }

    [SerializeField] Image _itemIcon;
    [SerializeField] TextMeshProUGUI _itemText;
    [SerializeField] Sprite _emptySprite;


    private void Start()
    {
        if(InventoryController.Instance != null)
        {
            InventoryController.Instance.OnUpdateInventoryCount += UpdateSlotCountHandle;
            InventoryController.Instance.OnDropItemEvent += DefaultSetHandle;
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

        UpdateSlotCountHandle();
    }

    /// <summary>
    /// 装备装备
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if(InventoryItem==null) 
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            InventoryController.Instance.DropDirItem(InventoryItem);
            InventoryItem = null;
            return;
        }

        if (InventoryItem.ItemData.Type != ItemType.Equipment)
            return;

        MenuController.Instance.CharacterUI.EquipWeapons(InventoryItem);

        if (InventoryItem.GetCount() <= 0)
        {
            _itemIcon.sprite = _emptySprite;
            InventoryItem = null;
        }
    }

    /// <summary>
    /// 更新单个栏的数量
    /// </summary>
    private void UpdateSlotCountHandle()
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

    /// <summary>
    /// 回到存储栏默认设置
    /// </summary>
    private void DefaultSetHandle()
    {
        _itemText.text = "";
        _itemIcon.sprite = _emptySprite;
        InventoryItem=null;
    }
}
