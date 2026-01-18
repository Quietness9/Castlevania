using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftSlotCtr : SlotCtr,IPointerDownHandler
{
    [SerializeField] TextMeshProUGUI _craftNameText;


    public void SetSlotData(ItemData itemData,int materialCount=0)
    {
        if(itemData==null)
            return;

        if (itemData.Type == ItemType.Equipment)
        {
           InventoryItemData=new InventoryItem(itemData);
        }

        itemIcon.sprite = itemData.DropIcon;
        _craftNameText.text = itemData.Name;

        if (materialCount>0)
        {
            itemIcon.sprite = itemData.InventoryIcon;
            _craftNameText.text = ""+materialCount;
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MenuCtr.Instance.CraftUI.SetMaterialSlotData(InventoryItemData);
    }
}
