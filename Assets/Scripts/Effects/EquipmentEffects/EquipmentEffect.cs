using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipmentEffect : ScriptableObject
{
    [SerializeField] protected float destroyTime;


    /// <summary>
    ///  Õ∑≈Ãÿ–ß
    /// </summary>
    /// <param name="transform"></param>
    public virtual void ReleaseEffects(Transform transform)
    {
        Debug.Log("Effect");
    }
}
