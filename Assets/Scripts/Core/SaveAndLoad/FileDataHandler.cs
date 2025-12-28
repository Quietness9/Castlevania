using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/// <summary>
/// 数据转换
/// </summary>

public class FileDataHandler
{
    string _dataSavePath = "";
    string _dataFileName = "";

    public FileDataHandler(string  dataSavePath, string dataFileName)
    {
        _dataSavePath = dataSavePath;
        _dataFileName = dataFileName;
    }

    /// <summary>
    /// 将需要保存的数据转换为Json文件保存
    /// </summary>
    /// <param name="data"></param>
    public void SaveDataTransition(GameData data)
    {
        string fullPath=Path.Combine(_dataSavePath, _dataFileName);

        try
        {
            //从完整的文件路径 (fullPath) 中提取出目录路径（文件夹路径）。
            //例如，如果 fullPath 是 "C:\MyGame\Saves\playerData.json"，那么 Path.GetDirectoryName(fullPath) 就会返回 "C:\MyGame\Saves\"。
            //Directory.CreateDirectory(...):创建指定路径的目录（文件夹）。如果指定的目录不存在，它会创建该目录。如果指定的目录已经存在，它会静默地忽略，不会报错。
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //JsonUtility.ToJson: JsonUtility 是 Unity 提供的一个静态类，用于将 C# 对象序列化为 JSON 字符串，
            //或将 JSON 字符串反序列化为 C# 对象。ToJson 是其中的序列化方法。
            //true: 输出格式化的 JSON 字符串，包含换行符和缩进，使其更易于人类阅读。
            //false: 输出紧凑的 JSON 字符串，没有多余的空白字符，文件体积更小。
            string dataToStore =JsonUtility.ToJson(data,true);

            //FileStream: 这是 .NET 提供的一个类，用于直接操作文件，可以进行读取、写入、追加等操作。
            //FileMode.Create: 这是一个枚举值，告诉 FileStream 如何打开文件。FileMode.Create 的意思是：
            //如果指定的文件不存在，则创建一个新文件。
            //如果指定的文件已经存在，则覆盖（删除旧内容，创建一个空文件）该文件。
            using (FileStream stream=new FileStream(fullPath, FileMode.Create))
            {

                //StreamWriter: 这是 .NET 提供的另一个类，专门用于将文本内容写入流（Stream）。它提供了 Write 和 WriteLine 等方便的方法来写入字符串。
                //Write(dataToStore): StreamWriter.Write 方法将 dataToStore 字符串（在之前的代码中由 JsonUtility.ToJson 生成的 JSON 文本）
                //写入到与 StreamWriter 关联的 FileStream，即写入到 fullPath 指定的文件中。
                using (StreamWriter writer=new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch(Exception e)
        {
            Debug.LogError("尝试保存游戏数据时出错:"+fullPath+"\n"+e.Message);
        }
    }


    /// <summary>
    /// 将需要加载的数据转换为游戏数据形式
    /// </summary>
    public GameData LoadDataTransition()
    {
        string fullPath=Path.Combine(_dataSavePath , _dataFileName);
        GameData loadData = null;

        //这个方法用于检查指定路径 (fullPath) 的文件是否存在。
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using(FileStream stream=new FileStream(fullPath , FileMode.Open))
                {
                    using(StreamReader reader=new StreamReader(stream))
                    {
                        dataToLoad= reader.ReadToEnd();
                    }
                }

                loadData=JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("尝试加载游戏数据时出错:" + fullPath + "\n" + e.Message);
            }
        }

        return loadData;
    }
}
