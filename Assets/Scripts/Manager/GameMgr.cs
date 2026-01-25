using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoSingleton<GameMgr>
{
    public CheckPointCtr ActiveCheckPoint;

    [SerializeField] CheckPointCtr[] _allCheckPoints;

    protected override void Awake()
    {
        base.Awake();
        GameEventMgr.OnSaveGame+=SaveGameData;
        GameEventMgr.OnLoadGame+=LoadGameData;
    }

    private void Start()
    {
        _allCheckPoints=FindObjectsOfType<CheckPointCtr>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameEventMgr.OnSaveGame -= SaveGameData;
        GameEventMgr.OnLoadGame -= LoadGameData;
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void RestartScene()
    {
        Scene scene=SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    /// <param name="isPause"></param>
    public void PauseGame(bool isPause)
    {
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }


    /// <summary>
    /// 玩家死亡后需要做的清理
    /// </summary>
    private void PlayerDieHandle()
    {
       
    }

    public void LoadGameData(GameData data)
    {
        foreach(var checkPointController in _allCheckPoints)
        {
            if(data.CheckPoints.TryGetValue(checkPointController.CheckPointId, out bool isActive))
            {
                if (isActive)
                {
                    checkPointController.ActiveCheckPoint();

                    if (checkPointController.CheckPointId == data.LastCheckPointId)
                    {
                        GlobalReferencesMgr.Instance.GamePlayer.transform.position = checkPointController.transform.position;                    }
                }
            }
        }
    }

    public void SaveGameData(GameData data)
    {
        data.CheckPoints.Clear();
        foreach(var item in _allCheckPoints)
        {
            data.CheckPoints.Add(item.CheckPointId,item.IsActive);
        }

        if (ActiveCheckPoint != null)
        {
            data.LastCheckPointId = ActiveCheckPoint.CheckPointId;
        }
        
    }
}
