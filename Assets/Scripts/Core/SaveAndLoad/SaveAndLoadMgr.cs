using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoadMgr : MonoSingleton<SaveAndLoadMgr>
{
    [SerializeField] string _fileName;
    [SerializeField] bool _isEncryption = false;


    GameData _gameData;
    FileDataHandler _fileDataHandler;

    
    List<ISaveManager> _saveManagers;

    protected override void Awake()
    {
        isDontDestroy=true;
        base.Awake();
    }

    private void Start()
    {
        _fileDataHandler=new FileDataHandler(Application.persistentDataPath, _fileName);

        // 3. 订阅场景加载完成事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _saveManagers?.Clear();
    }

    /// <summary>
    /// 创建游戏数据
    /// </summary>
    public void NewGameData()
    {
        _gameData = new GameData();
    }

    /// <summary>
    /// 保存所有的游戏数据
    /// </summary>
    public void SaveGameAllData()
    {
        if (_saveManagers == null)
            return;

        Debug.Log("保存游戏数据");
        foreach (var item in _saveManagers)
        {
            item.SaveGameData(_gameData);
        }


        _fileDataHandler.SaveDataTransition(_gameData,_isEncryption);
    }

    /// <summary>
    /// 加载所有游戏数据
    /// </summary>
    public void LoadGameAllData()
    {
        if (_saveManagers == null)
            return;

        _gameData = _fileDataHandler.LoadDataTransition(_isEncryption);

        if( _gameData == null)
        {
            NewGameData();
        }

        Debug.Log("加载游戏数据");
        foreach(var item in _saveManagers)
        {
            item.LoadGameData(_gameData);
        }

    }


    /// <summary>
    /// 判断是否有数据
    /// </summary>
    /// <returns></returns>
    public bool HaveGameData()
    {
        if (_fileDataHandler.LoadDataTransition(_isEncryption) != null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 删除游戏数据
    /// </summary>
    [ContextMenu("Delete file data")]
    public void DeleteSaveGameData()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);
        _fileDataHandler.DeleteGameData();
    }

    private void OnApplicationQuit()
    {
        SaveGameAllData();
    }

    //博主使用方法
    /// <summary>
    /// 获得处于活跃状态全部需要保存数据的脚本
    /// </summary>
    /// <returns></returns>
    private List<ISaveManager> FindAllSaveManager()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    /// <summary>
    /// 场景加载完后的回调
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene '{scene.name}' loaded. Mode: {mode}");

        _saveManagers = FindAllSaveManager();
        Invoke("LoadGameAllData", 0.5f);
    }

}
