using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager
{

    /// <summary>
    /// 加载游戏数据
    /// </summary>
    /// <param name="data"></param>
    void LoadGameData(GameData data);

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    void SaveGameData(GameData data);
}
