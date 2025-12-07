using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttribute : MonoBehaviour
{

    public BaseAttributeData CharacterAttributeData;
    public event Action DieEvent;

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
    public Attribute FireDamage; // 火焰伤害：持续造成伤害
    public Attribute IceDamage; // 冰冻伤害：减速，减少20%的护甲
    public Attribute LightingDamage; // 雷电伤害：减少20%的命中率

    public MagicEffectType SelfMagicType { get; private set; } = MagicEffectType.None; //自身受到的魔法效果

    float _igniteDamageTimer;
    float _igniteDamageCooldown; //造成一次点燃伤害冷却
    int _igniteDamage;

    float _chillTimer;
    float _shockTimer;

    protected virtual void Start()
    {
        InitBaseAttributeData();

        CurrentHealth = GetMaxHealth();
    }

    protected virtual void Update()
    {
        _igniteDamageTimer-= Time.deltaTime;
        _chillTimer-= Time.deltaTime;
        _shockTimer-= Time.deltaTime;

        if (SelfMagicType == MagicEffectType.Ignite && _igniteDamageTimer <= 0)
        {
            TakeIgniteDamage();
        }

        if(SelfMagicType==MagicEffectType.Chill && _chillTimer <= 0)
        {
            ChillEffect();
        }

        if (SelfMagicType == MagicEffectType.Shock && _shockTimer <= 0)
        {
            ShockEffect();
        }
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
        Intelligence.SetBaseValue(CharacterAttributeData.Intelligence);

        Armor.SetBaseValue(CharacterAttributeData.Armor);
        Evasion.SetBaseValue(CharacterAttributeData.Evasion);
        MagicResistance.SetBaseValue(CharacterAttributeData.MagicResistance);

        CriticalChance.SetBaseValue(CharacterAttributeData.CriticalChance);
        CriticalDamage.SetBaseValue(CharacterAttributeData.CriticalDamage);

        FireDamage.SetBaseValue(CharacterAttributeData.FireDamage);
        IceDamage.SetBaseValue(CharacterAttributeData.IceDamage);
        LightingDamage.SetBaseValue(CharacterAttributeData.LightingDamage);

    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="character">造成伤害的对象</param>
    /// <param name="isUseMagic">是否使用魔法伤害</param>
    public virtual void TakeDamage(Character character, bool isUseMagic = false)
    {
        if (IsSuccessfulEvasion())
            return;

        int totalDamage = GetPhysicalDamage(character.Attribute);

        if (isUseMagic == true)
        {
            totalDamage += GetMagicDamage(character.Attribute);
            ApplyMagicEffect(character.Attribute);
        }

        Debug.Log("造成伤害" + totalDamage);

        ReduceCurrentHealth(totalDamage);
    }

    /// <summary>
    /// 获得物理伤害值
    /// </summary>
    /// <param name="attackTarget">攻击对象</param>
    /// <returns></returns>
    private int GetPhysicalDamage(CharacterAttribute attackTarget)
    {
        float targetResistance =Armor.GetValue();

        if (SelfMagicType == MagicEffectType.Chill)
        {
            targetResistance *= 0.8f;
        }
        targetResistance *= 2;


        float damage= attackTarget.Atk.GetValue()+ attackTarget.Strength.GetValue();

        if (attackTarget.IsCriticalStrike())
        {
           damage= attackTarget.CalculationCriticalDamage(damage);
        }

        float totalDamage=damage- targetResistance;
        totalDamage=Mathf.Clamp(totalDamage, 0,int.MaxValue);

        return Mathf.RoundToInt(totalDamage);
    }

    #region 魔法效果

    /// <summary>
    /// 获得魔法伤害
    /// </summary>
    /// <param name="attackTarget">攻击对象</param>
    /// <returns></returns>
    private int GetMagicDamage(CharacterAttribute attackTarget)
    {
        int targetResistance=MagicResistance.GetValue() + Intelligence.GetValue()*2;

        int damage= attackTarget.FireDamage.GetValue()+ attackTarget.IceDamage.GetValue()+ 
            attackTarget.LightingDamage.GetValue() + attackTarget.Intelligence.GetValue();

        if (attackTarget.IsCriticalStrike())
        {
            damage= attackTarget.CalculationCriticalDamage(damage);
        }

        int totalDamage = damage- targetResistance;
        totalDamage= Mathf.Clamp(totalDamage,0,int.MaxValue);

        return totalDamage;
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
            if (UnityEngine.Random.value > 0.5 && fireDamage > 0)
            {
                return MagicEffectType.Ignite;
            }

            if (UnityEngine.Random.value > 0.4 && iceDamage > 0)
            {
                return MagicEffectType.Chill;
            }

            if(UnityEngine.Random.value > 0.6 && lightingDamage > 0)
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


    /// <summary>
    /// 应用魔法伤害最高的魔法效果
    /// </summary>
    /// <param name="attackTarget">攻击者</param>
    public void ApplyMagicEffect(CharacterAttribute attackTarget)
    {
        SelfMagicType = attackTarget.GetMagicType();

        switch (SelfMagicType)
        {
            case MagicEffectType.Ignite:
                {
                    _igniteDamage = Mathf.RoundToInt(attackTarget.FireDamage.GetValue() * 0.2f);
                    _igniteDamageCooldown=attackTarget.CharacterAttributeData.IgniteDamageCooldown;
                }
                break;
            case MagicEffectType.Chill:return;
            case MagicEffectType.Shock:return;
            case MagicEffectType.None:return;
            default:Debug.LogWarning("没有此类型的魔法特效");break;
        }
    }

    /// <summary>
    /// 造成点燃伤害
    /// </summary>
    private void TakeIgniteDamage()
    {
        _igniteDamageTimer = _igniteDamageCooldown;
        ReduceCurrentHealth(Mathf.RoundToInt(_igniteDamage));
        Debug.Log(CurrentHealth);
    }

    private void ChillEffect()
    {

    }

    private void ShockEffect()
    {

    }

    #endregion

    /// <summary>
    /// 获得最大生命值
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealth()
    {
        return Hp.GetValue() + Vitality.GetValue() * 3;
    }

    /// <summary>
    /// 减少当前生命值
    /// </summary>
    /// <param name="hp"></param>
    public void ReduceCurrentHealth(int amount)
    {
        CurrentHealth-=amount;
        CurrentHealth=Mathf.Max(CurrentHealth, 0);
        if(CurrentHealth <= 0)
        {
            Die();
        }
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
    /// 判断角色是否闪避成功
    /// </summary>
    /// <returns></returns>
    public virtual bool IsSuccessfulEvasion()
    {
        float totalEvasion = Evasion.GetValue() + Agility.GetValue();

        if (SelfMagicType == MagicEffectType.Shock)
        {
            totalEvasion *= 0.8f;
        }

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("闪避成功");
            return true;
        }

        return false;
    }


    /// <summary>
    /// 检查是否暴击
    /// </summary>
    /// <returns></returns>
    private bool IsCriticalStrike()
    {
        int totalCriticalChance=CriticalChance.GetValue()+ Agility.GetValue();

        if (UnityEngine.Random.Range(0, 100) < totalCriticalChance)
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
    private int CalculationCriticalDamage(float damage)
    {
        float totalCriticalEnhance = (CriticalDamage.GetValue()+Strength.GetValue())*0.01f;
        float totalCriticalDamage = damage + (damage * totalCriticalEnhance);

        totalCriticalDamage=Mathf.Clamp(totalCriticalDamage,0,int.MaxValue);

        return Mathf.RoundToInt(totalCriticalDamage);
    }

    /// <summary>
    /// 角色死亡
    /// </summary>
    private void Die()
    {
        DieEvent.Invoke();
    }
}
