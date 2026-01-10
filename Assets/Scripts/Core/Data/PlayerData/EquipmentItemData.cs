using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New EquipmentItem Data",menuName = "GameData/InventoryItem/EquipmentData")]
public class EquipmentItemData : ItemData
{

    [field:SerializeField] public EquipmentItemType EquipmentType {  get;private set; }

    [Header("装备特效")]
    [SerializeField] List<EquipmentEffect> _equipmentEffects = new();
    [field: SerializeField] public float Cooldown { get; private set; }

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

    int _descriptionCount;

    /// <summary>
    /// 使用武器特效
    /// </summary>
    /// <param name="transform">生产特效的位置</param>
    public void UseEquipmentEffect(Transform transform)
    {
        foreach(var effect in _equipmentEffects)
        {
            effect.ReleaseEffects(transform);
        }
    }

    /// <summary>
    /// 获得状态描述
    /// </summary>
    /// <returns></returns>
    public override string GetDescription()
    {
        stringBuilder.Length = 0;
        _descriptionCount = 0;

        AddItemDescription(Hp, "Hp");
        AddItemDescription(Atk, "Atk");

        AddItemDescription(Agility, "Agility");
        AddItemDescription(Vitality, "Vitality");
        AddItemDescription(Strength, "Strength");
        AddItemDescription(Intelligence, "Intelligence");

        AddItemDescription(Armor, "Armor");
        AddItemDescription(Evasion, "Evasion");
        AddItemDescription(MagicResistance, "MagicResist");

        AddItemDescription(CriticalChance, "CriticalChance");
        AddItemDescription(CriticalDamage, "CriticalDamage");

        AddItemDescription(FireDamage, "FireDamage");
        AddItemDescription(IceDamage, "IceDamage");
        AddItemDescription(LightingDamage, "LightDamage");

        if (_descriptionCount<minDescriptionLength)
        {
            for(int i = 0; i < minDescriptionLength-_descriptionCount; i++)
            {
                stringBuilder.AppendLine();
                stringBuilder.Append("");
            }
        }

        if(description.Length > 0)
        {
            stringBuilder.AppendLine();
            stringBuilder.Append("效果:"+description);
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 添加描述
    /// </summary>
    /// <param name="value"></param>
    /// <param name="name"></param>
    private void AddItemDescription(int value,string name)
    {
        if (value != 0)
        {
            if (stringBuilder.Length > 0)
            {
                stringBuilder.AppendLine();
            }

            stringBuilder.Append("+ "+name+":"+value);
            _descriptionCount++;
        }
    }

}
