using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attribute
{
    [SerializeField] int _baseValue;
    [SerializeField] List<int> _modifier=new ();

    /// <summary>
    /// 设置基础值s
    /// </summary>
    /// <param name="baseValue"></param>
    public void SetBaseValue(int baseValue)
    {
        _baseValue = baseValue;
    }

    /// <summary>
    /// 获得基础值
    /// </summary>
    /// <returns></returns>
    public int GetBaseValue()
    {
        return _baseValue;
    }

    /// <summary>
    /// 获得有加成后的值
    /// </summary>
    public int GetValue()
    {
        int value = _baseValue;

        foreach (int modifier in _modifier)
        {
            value += modifier;
        }

        return value;
    }

    /// <summary>
    /// 添加加成
    /// </summary>
    public void AddModifier(int mod)
    {
        if (mod == 0)
            return;


        _modifier.Add(mod);
    }

    /// <summary>
    /// 去除加成
    /// </summary>
    /// <param name="mod"></param>
    public void RemoveModifier(int mod)
    {
        _modifier.Remove(mod);
    }
}
