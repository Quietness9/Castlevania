using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalReferencesManager : MonoSingleton<GlobalReferencesManager>
{
    [Header("单例引用")]
    public Player GamePlayer;

    [Header("实例化预制体")]
    [SerializeField] List<GameObject> _editorPrefab=new ();


    protected override void OnDestroy()
    {
        base.OnDestroy();
        _editorPrefab?.Clear ();
    }

    public GameObject GetPrefab(string name)
    {
        var found = _editorPrefab.Find(p => p != null && p.name == name);

        if (found == null)
        {
            Debug.LogWarning($"未找到名称为 '{name}' 的预制体");
            return null;
        }

        return found;
    }


#if UNITY_EDITOR
    [ContextMenu("验证预制体引用")]
private void ValidatePrefabReferences()
    {
        Debug.Log("开始验证预制体引用...");
        int validCount = 0;
        int nullCount = 0;

        for (int i = 0; i < _editorPrefab.Count; i++)
        {
            if (_editorPrefab[i] == null)
            {
                Debug.LogError($"索引 {i}: 空引用", this);
                nullCount++;
            }
            else
            {
                Debug.Log($"索引 {i}: {_editorPrefab[i].name} ✅", this);
                validCount++;
            }
        }

        Debug.Log($"验证完成: 有效 {validCount}个, 空引用 {nullCount}个");
    }
#endif
}
