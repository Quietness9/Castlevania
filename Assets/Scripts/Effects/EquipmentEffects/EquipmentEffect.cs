using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipmentEffect : ScriptableObject
{
    [TextArea]
    [SerializeField] protected string description;
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
