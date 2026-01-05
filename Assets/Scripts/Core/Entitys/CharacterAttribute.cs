using System;
using System.Collections;
using System.Xml.Linq;
using UnityEngine;

public class CharacterAttribute : MonoBehaviour
{

    public BaseAttributeData CharacterAttributeData;
    public event Action OnDieEvent = delegate { };
    public event Action OnChangeHealthEvent = delegate { };
    public event Action OnAttributeSlotEvent = delegate { };

    [Header("基础属性")]
    public Attribute Hp; // 最大生命值
    public Attribute Atk; //攻击力
    [field: SerializeField] public int CurrentHealth { get; private set; }

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
    public bool IsDie { get; private set; }
    public bool IsInvincible {  get; private set; }

    Character _character;
    Coroutine _magicCoroutine;

    int _igniteDamage;
    float _igniteDamageTimer;
    float _igniteDamageCooldown; //造成一次点燃伤害冷却

    float _slowRatio;

    protected virtual void Awake()
    {
        _character = GetComponentInParent<Character>();

        InitBaseAttributeData();
        
    }

    protected virtual void Start() { }

    protected virtual void Update()
    {
        _igniteDamageTimer -= Time.deltaTime;

        if (SelfMagicType == MagicEffectType.Ignite && _igniteDamageTimer <= 0)
        {
            TakeIgniteDamage();
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

        IsDie = false;
        CurrentHealth = GetMaxHealth();
    }

    /// <summary>
    /// 受到物理伤害
    /// </summary>
    /// <param name="character">造成伤害的对象</param>
    /// <param name="ratio">伤害比例</param>
    public virtual void TakePhysicalDamage(Character character,float ratio=1)
    {


        if (IsSuccessfulEvasion()||IsInvincible)
            return;

        int totalDamage = Mathf.RoundToInt(GetPhysicalDamage(character.Attribute)*ratio);
        _character.Fx.StartCoroutine("FlashFX");
        _character.Fx.CreateHitFX(HitFXType.HitFX00,transform);

        ReduceCurrentHealth(totalDamage);
    }

    /// <summary>
    /// 受到魔法伤害
    /// </summary>
    /// <param name="character"></param>
    /// <param name="ratio">伤害比例</param>
    public virtual void TakeMagicDamage(Character character, float ratio = 1)
    {
        if (IsSuccessfulEvasion()||IsInvincible)
            return;

        int totalDamage= Mathf.RoundToInt(GetMagicDamage(character.Attribute)*ratio);
        ApplyMagicEffect(character.Attribute);
        _character.Fx.StartCoroutine("FlashFX");

        ReduceCurrentHealth(totalDamage);
    }

    /// <summary>
    /// 获得物理伤害值
    /// </summary>
    /// <param name="attackTarget">造成攻击对象</param>
    /// <returns></returns>
    private int GetPhysicalDamage(CharacterAttribute attackTarget)
    {
        float targetResistance = Armor.GetValue();

        if (SelfMagicType == MagicEffectType.Chill)
        {
            targetResistance *= 0.8f;
        }
        targetResistance *= 2;


        float damage = attackTarget.GetTotalAtk();

        if (attackTarget.IsCriticalStrike())
        {
            damage = attackTarget.CalculationCriticalDamage(damage);
        }

        float totalDamage = damage - targetResistance;
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);

        return Mathf.RoundToInt(totalDamage);
    }

    #region 魔法效果

    /// <summary>
    /// 获得魔法伤害
    /// </summary>
    /// <param name="attackTarget">造成攻击对象</param>
    /// <returns></returns>
    private int GetMagicDamage(CharacterAttribute attackTarget)
    {
        int targetResistance = GetTotalMagicResistance();

        int damage = attackTarget.FireDamage.GetValue() + attackTarget.IceDamage.GetValue() +
            attackTarget.LightingDamage.GetValue() + attackTarget.Intelligence.GetValue();

        if (attackTarget.IsCriticalStrike())
        {
            damage = attackTarget.CalculationCriticalDamage(damage);
        }

        int totalDamage = damage - targetResistance;
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);

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

        if (Mathf.Max(fireDamage, iceDamage, lightingDamage) <= 0)
        {
            return MagicEffectType.None;
        }

        bool isCanApplyFire = fireDamage > iceDamage && fireDamage > lightingDamage;
        bool isCanApplyIce = iceDamage > fireDamage && iceDamage > lightingDamage;
        bool isCanApplyLighting = lightingDamage > fireDamage && lightingDamage > iceDamage;

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

            if (UnityEngine.Random.value > 0.6 && lightingDamage > 0)
            {
                return MagicEffectType.Shock;
            }

        }

        if (isCanApplyFire)
        {
            return MagicEffectType.Ignite;
        }
        if (isCanApplyIce)
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
    public void ApplyMagicEffect(CharacterAttribute attackAttribute)
    {

        if (SelfMagicType!= attackAttribute.GetMagicType()&&_magicCoroutine!=null)
        {
            StopMagicEffect();
            StopCoroutine(_magicCoroutine);
            _magicCoroutine = null;
        }

        SelfMagicType = attackAttribute.GetMagicType();

        switch (SelfMagicType)
        {
            case MagicEffectType.Ignite:
                {
                    _igniteDamage = Mathf.RoundToInt(attackAttribute.FireDamage.GetValue() * 0.2f);
                    _igniteDamageCooldown = attackAttribute.CharacterAttributeData.IgniteDamageCooldown;
                    if (!_character.Fx.IsStartColorChange)
                    {
                        _magicCoroutine=StartCoroutine(IgniteEffectCo(attackAttribute.CharacterAttributeData.IgniteDurationTime));
                    }
                }
                break;
            case MagicEffectType.Chill:
                {
                    _slowRatio = attackAttribute.CharacterAttributeData.SlowRatio;
                    if (!_character.Fx.IsStartColorChange)
                    {
                        _magicCoroutine=StartCoroutine(ChillEffectCo(attackAttribute.CharacterAttributeData.IceDurationTime));
                    }
                    
                }
                break ;
            case MagicEffectType.Shock:
                {
                    TakeShockDamage(attackAttribute);
                    if (!_character.Fx.IsStartColorChange)
                    {
                        _magicCoroutine = StartCoroutine(ShockEffectCo(attackAttribute.CharacterAttributeData.LightingDurationTime));
                    }
                }
                break;
            case MagicEffectType.None:break;
            default: Debug.LogWarning("没有此类型的魔法特效"); break;
        }

    }

    /// <summary>
    /// 造成点燃伤害
    /// </summary>
    private void TakeIgniteDamage()
    {
        _igniteDamageTimer = _igniteDamageCooldown;
        ReduceCurrentHealth(Mathf.RoundToInt(_igniteDamage));
    }

    /// <summary>
    /// 点燃特效
    /// </summary>
    /// <param name="durationTime"></param>
    /// <returns></returns>
    private IEnumerator IgniteEffectCo(float durationTime)
    {
        
        StartMagicEffect("IgniteColorChange", "IgniteFX", _character.Fx.FxData.RepeatTime);


        yield return new WaitForSeconds(durationTime);

        StopMagicEffect();
    }

    /// <summary>
    /// 冰冻特效
    /// </summary>
    /// <param name="durationTime"></param>
    /// <returns></returns>
    private IEnumerator ChillEffectCo(float durationTime)
    {
        
        StartMagicEffect("ChillColorChange", "ChillFX", _character.Fx.FxData.RepeatTime);
        _character.SlowCharacterSpeed(_slowRatio);

        yield return new WaitForSeconds(durationTime);
        
        _character.ReturnCharacterDefaultSpeed();
        StopMagicEffect();
    }

    /// <summary>
    /// 产生雷电造成伤害
    /// </summary>
    private void TakeShockDamage(CharacterAttribute attackTarget)
    {
        GameObject shockPre = GlobalReferencesManager.Instance.GetPrefab("ShockStrike");
        if (shockPre == null)
            return;

        GameObject shockObj = Instantiate(shockPre, transform.position, Quaternion.identity);
        shockObj.GetComponent<ShockStrikeController>().ShockEffect();

        int shockDamage = Mathf.RoundToInt(attackTarget.LightingDamage.GetValue() * 0.2f);

        ReduceCurrentHealth(shockDamage);
        _character.Fx.StartCoroutine("FlashFX");


    }

    /// <summary>
    /// 雷电特效
    /// </summary>
    /// <param name="durationTime"></param>
    /// <returns></returns>
    private IEnumerator ShockEffectCo(float durationTime)
    {
        
        StartMagicEffect("ShockColorChange", "ShockFX", _character.Fx.FxData.RepeatTime);

        yield return new WaitForSeconds(durationTime);

        StopMagicEffect();
    }

    /// <summary>
    /// 停止魔法特效
    /// </summary>
    private void StopMagicEffect()
    {
        if (_character == null || _character.Fx == null)
        {
            Debug.LogWarning("Character is null or Fx is null");
            return;
        }

        _character.Fx.CancelColorChange();
        _character.Fx.UnLockColorChange();
        SelfMagicType = MagicEffectType.None;
    }

    /// <summary>
    /// 开始魔法特效
    /// </summary>
    /// <param name="funName"></param>
    /// <param name="repeatTime"></param>
    private void StartMagicEffect(string funName,string particleName, float repeatTime)
    {
        if (_character == null || _character.Fx == null)
        {
            Debug.LogWarning("Character is null or Fx is null");
            return;
        }

        _character.Fx.SpawnMagicParticle(particleName);
        _character.Fx.InvokeRepeating(funName, 0, repeatTime);
        _character.Fx.LockColorChange();
    }

    #endregion

    #region 不同属性对其他属性影响后的总值

    /// <summary>
    /// 获得总魔法抗性
    /// </summary>
    /// <returns></returns>
    public int GetTotalMagicResistance() => MagicResistance.GetValue() + Intelligence.GetValue() * 2;

    /// <summary>
    /// 获得总暴击伤害增幅
    /// </summary>
    /// <returns></returns>
    public int GetTotalCriticalDamage() => CriticalDamage.GetValue() + Strength.GetValue();

    /// <summary>
    /// 获得总共暴击率
    /// </summary>
    /// <returns></returns>
    public int GetTotalCriticalChance() => CriticalChance.GetValue() + Agility.GetValue();

    /// <summary>
    /// 获得总共Atk值
    /// </summary>
    /// <returns></returns>
    public int GetTotalAtk()=>Atk.GetValue() + Strength.GetValue();
    

    /// <summary>
    /// 获得总共闪避值
    /// </summary>
    /// <returns></returns>
    public int GetTotalEvasion()=> Evasion.GetValue() + Agility.GetValue();


    /// <summary>
    /// 获得最大生命值
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealth()=> Hp.GetValue() + Vitality.GetValue() * 3;

    #endregion

    #region Modifier修改

    /// <summary>
    /// 添加装备加成
    /// </summary>
    /// <param name="eqData"></param>
    public void AddEquipmentModifier(EquipmentItemData eqData)
    {
        Hp.AddModifier(eqData.Hp);
        Atk.AddModifier(eqData.Atk);

        Agility.AddModifier(eqData.Agility);
        Vitality.AddModifier(eqData.Vitality);
        Strength.AddModifier(eqData.Strength);
        Intelligence.AddModifier(eqData.Intelligence);

        Armor.AddModifier(eqData.Armor);
        Evasion.AddModifier(eqData.Evasion);
        MagicResistance.AddModifier(eqData.MagicResistance);

        CriticalChance.AddModifier(eqData.CriticalChance);
        CriticalDamage.AddModifier(eqData.CriticalDamage);

        FireDamage.AddModifier(eqData.FireDamage);
        IceDamage.AddModifier(eqData.IceDamage);
        LightingDamage.AddModifier(eqData.LightingDamage);

        OnAttributeSlotEvent.Invoke();
    }

    /// <summary>
    /// 移除装备加成
    /// </summary>
    /// <param name="eqData"></param>
    public void RemoveEquipmentModifier(EquipmentItemData eqData)
    {
        Hp.RemoveModifier(eqData.Hp);
        Atk.RemoveModifier(eqData.Atk);

        Agility.RemoveModifier(eqData.Agility);
        Vitality.RemoveModifier(eqData.Vitality);
        Strength.RemoveModifier(eqData.Strength);
        Intelligence.RemoveModifier(eqData.Intelligence);

        Armor.RemoveModifier(eqData.Armor);
        Evasion.RemoveModifier(eqData.Evasion);
        MagicResistance.RemoveModifier(eqData.MagicResistance);

        CriticalChance.RemoveModifier(eqData.CriticalChance);
        CriticalDamage.RemoveModifier(eqData.CriticalDamage);

        FireDamage.RemoveModifier(eqData.FireDamage);
        IceDamage.RemoveModifier(eqData.IceDamage);
        LightingDamage.RemoveModifier(eqData.LightingDamage);

        OnAttributeSlotEvent.Invoke();
    }

    /// <summary>
    /// 添加buff加成
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <param name="duration"></param>
    public void AddBuffModifier(AttributeType type,int value,float duration)
    {
        StartCoroutine(AddBuffModifierCo(type, value, duration));
    }


    private IEnumerator AddBuffModifierCo(AttributeType type, int value, float duration)
    {
        Attribute attribute = GetAttribute(type);

        if (attribute == null)
        {
            Debug.LogWarning($"Attribute of type {type} not found. Buff modifier not applied.");
            yield break; // 如果是 null，则立即结束协程
        }

        attribute.AddModifier(value);

        yield return new WaitForSeconds(duration);

        attribute.RemoveModifier(value);
    }

    /// <summary>
    /// 获得属性值
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Attribute GetAttribute(AttributeType type)
    {
        switch (type)
        {
            case AttributeType.Hp:return Hp;
            case AttributeType.Atk:return Atk;
            case AttributeType.Agility: return Agility;
            case AttributeType.Vitality: return Vitality;
            case AttributeType.Strength: return Strength;
            case AttributeType.Intelligence: return Intelligence;
            case AttributeType.Armor: return Armor;
            case AttributeType.Evasion: return Evasion;
            case AttributeType.MagicResistance: return MagicResistance;
            case AttributeType.CriticalChance: return CriticalChance;
            case AttributeType.CriticalDamage:return CriticalDamage;
            case AttributeType.FireDamage:return FireDamage;
            case AttributeType.IceDamage:return IceDamage;
            case AttributeType.LightingDamage:return LightingDamage;
            default:return null;
                
        }
    }

    #endregion

    #region 伤害计算辅助函数

    /// <summary>
    /// 判断角色是否闪避成功
    /// </summary>
    /// <returns></returns>
    public virtual bool IsSuccessfulEvasion()
    {
        float totalEvasion =GetTotalEvasion(); 

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
        int totalCriticalChance = GetTotalCriticalChance();

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
        float totalCriticalEnhance = GetTotalCriticalDamage() * 0.01f;
        float totalCriticalDamage = damage + (damage * totalCriticalEnhance);

        totalCriticalDamage = Mathf.Clamp(totalCriticalDamage, 0, int.MaxValue);

        return Mathf.RoundToInt(totalCriticalDamage);
    }

    #endregion

    /// <summary>
    /// 减少当前生命值
    /// </summary>
    /// <param name="hp"></param>
    public virtual void ReduceCurrentHealth(int amount)
    {
        if (CurrentHealth <= 0)
            return;

        Debug.Log("造成伤害" + amount);
        CurrentHealth -= amount;
        CurrentHealth = Mathf.Max(CurrentHealth, 0);

        OnChangeHealthEvent.Invoke();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 恢复当前生命值
    /// </summary>
    /// <param name="amount"></param>
    public virtual void RecoverCurrentHealth(int amount)
    {
        Debug.Log("恢复血量" + amount);
        CurrentHealth += amount;
        CurrentHealth = Mathf.Min(CurrentHealth, GetMaxHealth());

        OnChangeHealthEvent.Invoke();
    }

    /// <summary>
    /// 角色死亡
    /// </summary>
    public void Die()
    {
        IsDie = true;
        OnDieEvent.Invoke();
    }


    /// <summary>
    /// 设置无敌
    /// </summary>
    /// <param name="isInvincible"></param>
    public void MakeInvincible()=>IsInvincible = true;
    
    /// <summary>
    /// 取消无敌
    /// </summary>
    /// <param name="invincible"></param>
    public void CancelInvincible()=>IsInvincible=false;
}
