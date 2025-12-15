using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New EquipmentItem Data",menuName = "GameData/InventoryItem/EquipmentData")]
public class EquipmentItemData : ItemData
{

    [field:SerializeField] public EquipmentItemType EquipmentType {  get;private set; }

    [field: Header("基础属性")]
    [field: SerializeField] public int Hp { get; private set; }
    [field: SerializeField] public int Atk { get; private set; }

    [field: Header("主要属性值")]
    [field: SerializeField] public int Agility { get; private set; }
    [field: SerializeField] public int Vitality { get; private set; }
    [field: SerializeField] public int Strength { get; private set; }
    [field: SerializeField] public int Intelligence { get; private set; }

    [field: Header("防御属性值")]
    [field: SerializeField] public int Armor { get; private set; }
    [field: SerializeField] public int Evasion { get; private set; }
    [field: SerializeField] public int MagicResistance { get; private set; }

    [field: Header("伤害加成属性值")]
    [field: SerializeField] public int CriticalChance { get; private set; }
    [field: SerializeField] public int CriticalDamage { get; private set; }

    [field:Header("魔法属性值")]
    [field: SerializeField] public int FireDamage { get; private set; }
    [field: SerializeField] public int IceDamage { get; private set; }
    [field: SerializeField] public int LightingDamage { get; private set; }

    [field:Header("制作材料")]
    [field: SerializeField] public List<CraftMaterialKind> CraftMaterial {  get; private set; }

    [Serializable]
    public struct CraftMaterialKind
    {
        public ItemData MaterialData;
        public int Count;
    }

}
