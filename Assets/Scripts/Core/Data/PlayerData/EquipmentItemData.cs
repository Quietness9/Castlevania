using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New EquipmentItem Data",menuName = "GameData/InventoryItem/EquipmentData")]
public class EquipmentItemData : ItemData
{
    [field:SerializeField] public EquipmentItemType EquipmentType {  get;private set; }
}
