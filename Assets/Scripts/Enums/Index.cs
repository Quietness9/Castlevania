using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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