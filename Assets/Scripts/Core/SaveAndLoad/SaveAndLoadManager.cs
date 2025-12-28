using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadManager : MonoSingleton<SaveAndLoadManager>
{
    [SerializeField] string _fileName;

    GameData _gameData;
    FileDataHandler _fileDataHandler;


    //List<ISaveManager> _saveManagers;

    public event Action<GameData> OnSaveEvent=delegate { };
    public event Action<GameData> OnLoadEvent=delegate { };

    private void Start()
    {
        _fileDataHandler=new FileDataHandler(Application.persistentDataPath, _fileName);
        LoadGameAllData();
    }

    /// <summary>
    /// 创建游戏数据
    /// </summary>
    public void NewGameData()
    {
        _gameData = new GameData();
    }


    public void SaveGameAllData()
    {
        Debug.Log("保存游戏数据");
        OnSaveEvent(_gameData);

        _fileDataHandler.SaveDataTransition(_gameData);
        Debug.Log("Save" + _gameData.GoldCoin);
    }

    public void LoadGameAllData()
    {
        _gameData = _fileDataHandler.LoadDataTransition();

        if( _gameData == null)
        {
            NewGameData();
        }

        Debug.Log("加载游戏数据");
        OnLoadEvent(_gameData);

        Debug.Log("load" + _gameData.GoldCoin);
    }

    private void OnApplicationQuit()
    {
        SaveGameAllData();
    }

    //博主使用方法
    /// <summary>
    /// 获得全部需要保存数据的脚本
    /// </summary>
    /// <returns></returns>
    //private List<ISaveManager> FindAllSaveManager()
    //{
    //    IEnumerable<ISaveManager> saveManagers = FindAnyObjectByType<MonoBehaviour>().OfType<ISaveManager>();
    //    return new List<ISaveManager>(saveManagers);
    //}


}
