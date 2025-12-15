using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//库存物品数据
[CreateAssetMenu(fileName ="New Item Data",menuName ="GameData/InventoryItem/ItemData")]
public class ItemData : ScriptableObject
{
    [field:SerializeField] public int Id {  get; private set; }
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public Sprite DropIcon { get; private set; }
    [field:SerializeField] public Sprite InventoryIcon { get; private set; }
    [field:SerializeField] public Sprite ShowIcon { get; private set; }
    [field:SerializeField] public ItemType Type { get; private set; }

    [field: Range(0, 1f)]
    [field: SerializeField] public float DropChance { get; private set; }
}
