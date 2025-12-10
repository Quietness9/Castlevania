using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Attribute Data",menuName ="GameData/Attribute/MainData")]
public class BaseAttributeData : ScriptableObject
{
    [field:Header("基础属性")]
    [field:SerializeField] public int Hp {  get;private set; }
    [field:SerializeField] public int Atk { get; private set; }

    [field:Header("主要属性值")]
    [field:SerializeField] public int Agility { get; private set; }
    [field:SerializeField] public int Vitality { get; private set; }
    [field:SerializeField] public int Strength { get; private set; }
    [field:SerializeField] public int Intelligence { get; private set; }

    [field: Header("防御属性值")]
    [field:SerializeField] public int Armor { get; private set; }
    [field:SerializeField] public int Evasion { get; private set; }
    [field:SerializeField] public int MagicResistance { get; private set; }

    [field: Header("伤害加成属性值")]
    [field:SerializeField] public int CriticalChance { get; private set; }
    [field:SerializeField] public int CriticalDamage { get; private set; }

    [field: Header("火焰属性值")]
    [field: SerializeField] public int FireDamage { get; private set; }
    [field: SerializeField] public float IgniteDurationTime { get; private set; }
    [field: SerializeField] public float IgniteDamageCooldown { get; private set; }

    [field: Header("冰冻属性值")]
    [field: SerializeField] public int IceDamage { get; private set; }
    [field: SerializeField] public float IceDurationTime {  get; private set; }
    [field: SerializeField] public float SlowRatio { get; private set; }


    [field: Header("雷电属性值")]
    [field: SerializeField] public int LightingDamage { get; private set; }
    [field: SerializeField] public float LightingDurationTime {  get; private set; }
}
