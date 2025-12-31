using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnActiveDataManager : MonoBehaviour,ISaveManager
{
    [SerializeField] List<MonoBehaviour> _unActiveSaveManagers = new();

    public void LoadGameData(GameData data)
    {
        foreach (var manager in _unActiveSaveManagers)
        {
            ISaveManager saveManager = manager as ISaveManager;
            saveManager?.LoadGameData(data);
        }
    }

    public void SaveGameData(GameData data)
    {
        foreach(var manager in _unActiveSaveManagers)
        {
            ISaveManager saveManager = manager as ISaveManager;
            saveManager?.SaveGameData(data);
        }
    }
}
