using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttribute : MonoBehaviour
{

    public BaseAttributeData CharacterAttributeData;

    [Header("基础属性")]
    public Attribute Hp; // 最大生命值
    public Attribute Atk; //攻击力
    [field:SerializeField] public int CurrentHealth { get; private set; }

    [Header("主要属性值")]
    public Attribute Agility;  // 敏捷：增加闪避1%，暴击率1%
    public Attribute Vitality; // 体力：增加血量3
    public Attribute Strength; // 力量：增加伤害1，暴击伤害1%
    public Attribute Intelligence; // 智力：增加魔法伤害1，魔法抵抗2

    [Header("防御属性值")]
    public Attribute Armor; // 护甲：每一点减少2点物理伤害
    public Attribute Evasion; // 闪避：每一点增加1%闪避
    public Attribute MagicResistance; // 魔抗：每一点减少2点魔法伤害

    [Header("伤害加成属性值")]
    public Attribute CriticalChance; // 暴击率
    public Attribute CriticalDamage; //暴击伤害 

    [Header("魔法属性值")]
    public Attribute FireDamage; // 火焰伤害
    public Attribute IceDamage; // 冰冻伤害
    public Attribute LightingDamage; // 雷电伤害


    protected virtual void Start()
    {
        InitBaseAttributeData();

        CurrentHealth = GetMaxHealth();
    }

    /// <summary>
    /// 初始化基础属性值
    /// </summary>
    private void InitBaseAttributeData()
    {
        Hp.SetBaseValue(CharacterAttributeData.Hp);
        Atk.SetBaseValue(CharacterAttributeData.Atk);

        Agility.SetBaseValue(CharacterAttributeData.Agility);
        Vitality.SetBaseValue(CharacterAttributeData.Vitality);
        Strength.SetBaseValue(CharacterAttributeData.Strength);
        Intelligence.SetBaseValue (CharacterAttributeData.Intelligence);

        Armor.SetBaseValue(CharacterAttributeData.Armor);
        Evasion.SetBaseValue(CharacterAttributeData.Evasion);
        MagicResistance.SetBaseValue(CharacterAttributeData.MagicResistance);

        CriticalChance.SetBaseValue(CharacterAttributeData.CriticalChance);
        CriticalDamage.SetBaseValue(CharacterAttributeData.CriticalDamage);

        FireDamage.SetBaseValue(CharacterAttributeData.FireDamage);
        IceDamage.SetBaseValue(CharacterAttributeData.IceDamage);
        LightingDamage.SetBaseValue(CharacterAttributeData.LightingDamage);

    }

    #region 获得各种值

    /// <summary>
    /// 获得物理伤害值
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public virtual int GetPhysicalDamage(CharacterAttribute target)
    {
        int targetResistance = target.Armor.GetValue() * 2;
        int damage= Atk.GetValue()+Strength.GetValue();

        if (IsCriticalStrike())
        {
           damage=CalculationCriticalDamage(damage);
        }

        int totalDamage=damage- targetResistance;
        totalDamage=Mathf.Clamp(totalDamage, 0,int.MaxValue);

        return totalDamage;
    }

    /// <summary>
    /// 获得魔法伤害
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public virtual int GetMagicDamage(CharacterAttribute target)
    {
        int targetResistance=target.MagicResistance.GetValue() + target.Intelligence.GetValue()*2;
        int damage= FireDamage.GetValue()+ IceDamage.GetValue()+ LightingDamage.GetValue() + Intelligence.GetValue();

        if (IsCriticalStrike())
        {
            damage=CalculationCriticalDamage(damage);
        }

        int totalDamage = damage- targetResistance;
        totalDamage= Mathf.Clamp(totalDamage,0,int.MaxValue);

        return totalDamage;
    }

    /// <summary>
    /// 获得总闪避值
    /// </summary>
    /// <returns></returns>
    public int GetTotalEvasion()
    {
        int totalEvasion=Evasion.GetValue()+ Agility.GetValue();

         return  totalEvasion;
    }

    /// <summary>
    /// 获得最大生命值
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealth()
    {
        return Hp.GetValue()+ Vitality.GetValue()*3;
    }

    /// <summary>
    /// 获得魔法特效类型
    /// </summary>
    /// <returns></returns>
    public MagicEffectType GetMagicType()
    {
        int fireDamage = FireDamage.GetValue();
        int iceDamage = IceDamage.GetValue();
        int lightingDamage = LightingDamage.GetValue();

        if (Mathf.Max(fireDamage,iceDamage,lightingDamage) <= 0)
        {
            return MagicEffectType.None;
        }

        bool isCanApplyFire = fireDamage > iceDamage && fireDamage > lightingDamage;
        bool isCanApplyIce = iceDamage > fireDamage && iceDamage > lightingDamage;
        bool isCanApplyLighting=lightingDamage > fireDamage && lightingDamage > iceDamage;

        //两个或三个相等时随机旋转一个类型
        while (!isCanApplyFire && !isCanApplyIce && !isCanApplyLighting)
        {
            if (Random.value > 0.5 && fireDamage > 0)
            {
                return MagicEffectType.Ignite;
            }

            if (Random.value > 0.4 && iceDamage > 0)
            {
                return MagicEffectType.Chill;
            }

            if(Random.value > 0.6 && lightingDamage > 0)
            {
                return MagicEffectType.Shock;
            }

        }

        if (isCanApplyFire)
        {
            return MagicEffectType.Ignite;
        }
        if(isCanApplyIce)
        {
            return MagicEffectType.Chill;
        }
        if (isCanApplyLighting)
        {
            return MagicEffectType.Shock;
        }

        return MagicEffectType.None;
    }

    #endregion

    /// <summary>
    /// 应用魔法伤害最高的魔法特效
    /// </summary>
    /// <param name="isIgnite">是否被点燃</param>
    /// <param name="isChill">是否被冰冻</param>
    /// <param name="isShock">是否被电</param>
    public void ApplyMagicEffect(MagicEffectType magicEffectType)
    {

    }

    /// <summary>
    /// 减少当前生命值
    /// </summary>
    /// <param name="hp"></param>
    public void ReduceCurrentHealth(int amount)
    {
        CurrentHealth-=amount;
        CurrentHealth=Mathf.Max(CurrentHealth, 0);
    }

    /// <summary>
    /// 恢复当前生命值
    /// </summary>
    /// <param name="amount"></param>
    public void RestoreCurrentHealth(int amount)
    {
        CurrentHealth += amount;
        CurrentHealth=Mathf.Min(CurrentHealth, GetMaxHealth());
    }


    /// <summary>
    /// 检查是否暴击
    /// </summary>
    /// <returns></returns>
    private bool IsCriticalStrike()
    {
        int totalCriticalChance=CriticalChance.GetValue()+ Agility.GetValue();

        if (Random.Range(0, 100) < totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 计算暴击伤害
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    private int CalculationCriticalDamage(int damage)
    {
        float totalCriticalEnhance = (CriticalDamage.GetValue()+Strength.GetValue())*0.01f;
        float totalCriticalDamage = damage + (damage * totalCriticalEnhance);

        totalCriticalDamage=Mathf.Clamp(totalCriticalDamage,0,int.MaxValue);

        return Mathf.RoundToInt(totalCriticalDamage);
    }
}
