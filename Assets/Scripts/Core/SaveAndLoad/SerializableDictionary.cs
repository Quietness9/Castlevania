using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] List<TKey> _keys = new ();
    [SerializeField] List<TValue> _values = new ();

    //触发时机: 在 Unity 将对象的数据写入（序列化）到磁盘、内存或 JSON 字符串之前立即调用。
    //主要目的: 让你有机会准备数据，以便它能够被正确地序列化。
    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var pair in this)
        {
            _keys.Add(pair.Key);
            _values.Add(pair.Value);
        }

    }

    //触发时机: 在 Unity 从磁盘、内存或 JSON 字符串读取（反序列化）数据到对象之后立即调用。
    //主要目的: 让你有机会处理刚反序列化出来的数据，将其恢复到你希望的内部表示形式。
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (_keys.Count != _values.Count)
        {
            Debug.Log("键值对不匹配");
        }

        for (int i = 0; i < _keys.Count; ++i)
        {
            this.Add(_keys[i], _values[i]);
        }
    }
}
