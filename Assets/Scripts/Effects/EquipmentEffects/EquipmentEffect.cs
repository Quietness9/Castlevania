using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Effect new",menuName ="GameData/Effect")]
public class EquipmentEffect : ScriptableObject
{

    /// <summary>
    /// 释放装备特效
    /// </summary>
    public virtual void ReleaseEffects()
    {
        Debug.Log("Effect");
    }
}
