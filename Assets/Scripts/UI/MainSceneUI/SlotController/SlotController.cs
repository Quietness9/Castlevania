using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    public InventoryItem InventoryItemData;

    [SerializeField] protected Image itemIcon;
    [SerializeField] protected Sprite _emptySprite;
    /// <summary>
    /// …Ë÷√≤€ ˝æ›
    /// </summary>
    /// <param name="inventoryItem"></param>
    public virtual void SetSlotData(InventoryItem inventoryItem) { }
   
}
