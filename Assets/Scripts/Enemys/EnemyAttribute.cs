using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttribute : CharacterAttribute
{
    [SerializeField] int _level=1;
    [SerializeField] EnemyLevelModifierData _modifierData;

    protected override void Start()
    {
        LevelEnhanceAttribute();
        base.Start();
    }

    /// <summary>
    /// 添加通过等级提高敌人属性
    /// </summary>
    private void LevelEnhanceAttribute()
    {
        if(_level==1)
            return;

        Hp.AddModifier(GetEnhanceValue(Hp,_modifierData.HpModifier));
        Atk.AddModifier(GetEnhanceValue(Atk, _modifierData.AtkModifier));

        Agility.AddModifier(GetEnhanceValue(Agility, _modifierData.AgilityModifier));
        Vitality.AddModifier(GetEnhanceValue(Vitality,_modifierData.VitalityModifier));
        Strength.AddModifier(GetEnhanceValue(Strength, _modifierData.StrengthModifier));
        Intelligence.AddModifier(GetEnhanceValue(Intelligence, _modifierData.IntelligenceModifier));

        Armor.AddModifier(GetEnhanceValue(Armor, _modifierData.ArmorModifier));
        Evasion.AddModifier(GetEnhanceValue(Evasion, _modifierData.EvasionModifier));
        MagicResistance.AddModifier(GetEnhanceValue(MagicResistance, _modifierData.MagicResistanceModifier));

        CriticalChance.AddModifier(GetEnhanceValue(CriticalChance, _modifierData.CriticalChanceModifier));
        CriticalDamage.AddModifier(GetEnhanceValue(CriticalDamage, _modifierData.CriticalDamageModifier));
    }
    
    /// <summary>
    /// 获得提示后的值
    /// </summary>
    /// <param name="baseAttribute"></param>
    /// <param name="mul"></param>
    /// <returns></returns>
    private int GetEnhanceValue(Attribute baseAttribute,float mul)
    {
        return Mathf.RoundToInt(baseAttribute.GetValue()* _level*mul);
    }
}
