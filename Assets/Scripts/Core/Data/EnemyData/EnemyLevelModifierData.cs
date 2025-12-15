using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New LvModifier Data",menuName ="GameData/Enemy/LvModifierData")]
public class EnemyLevelModifierData : ScriptableObject
{
    [field:Range(0, 1f)]
    [field:SerializeField] public float HpModifier { get; private set; }
    [field: Range(0, 1f)]
    [field: SerializeField] public float AtkModifier { get; private set; }

    [field: Range(0, 1f)]
    [field: SerializeField] public float AgilityModifier { get; private set; }
    [field: Range(0, 1f)]
    [field: SerializeField] public float VitalityModifier { get; private set; }
    [field: Range(0, 1f)]
    [field: SerializeField] public float StrengthModifier { get; private set; }
    [field: Range(0, 1f)]
    [field: SerializeField] public float IntelligenceModifier { get; private set; }

    [field: Range(0, 1f)]
    [field: SerializeField] public float ArmorModifier { get; private set; }
    [field: Range(0, 1f)]
    [field: SerializeField] public float EvasionModifier { get; private set; }
    [field: Range(0, 1f)]
    [field: SerializeField] public float MagicResistanceModifier { get; private set; }

    [field: Range(0, 1f)]
    [field: SerializeField] public float CriticalChanceModifier { get; private set; }
    [field: Range(0, 1f)]
    [field: SerializeField] public float CriticalDamageModifier { get; private set; }

}
