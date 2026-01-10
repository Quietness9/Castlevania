using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrencyType
{
    GoldCoin,//金币
    Soul//灵魂

}

public enum HitFXType
{
    HitFX00,
    HitFX01,
}



public enum SwordType
{
    Ordinary,//普通模式
    Bounce,//敌人交替弹射模式
    Pierce,//穿透模式
    Spin//旋转模式
}

public enum MagicEffectType
{
    Ignite,//点燃
    Chill,//冰冻
    Shock,//雷电
    None//没有特效类型
}

public enum ItemType
{
    Material, //材料
    Equipment //装备
}

public enum EquipmentItemType
{
    Weapon, //武器
    Armor, //护甲
    Amulet, //饰品
    Flask, //药品
}

//对象池类型
public enum PoolType
{
    GameObject,//游戏对象
    CraftListObject,//制作栏对象
    ParticleObject//粒子对象
}

public enum SlimeType
{
    Big,
    Medium,
    Small
}

public enum AttributeType
{
    Hp,
    Atk,
    Agility,
    Vitality,
    Strength,
    Intelligence,
    Armor,
    Evasion,
    MagicResistance,
    CriticalChance,
    CriticalDamage,
    FireDamage,
    IceDamage,
    LightingDamage
}



