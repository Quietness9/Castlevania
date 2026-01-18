using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotCtr : MonoBehaviour
{
    public InventoryItem InventoryItemData;

    [SerializeField] protected Image itemIcon;
    public Sprite EmptySprite;
    /// <summary>
    /// …Ë÷√≤€ ˝æ›
    /// </summary>
    /// <param name="inventoryItem"></param>
    public virtual void SetSlotData(InventoryItem inventoryItem) { }
   
}
