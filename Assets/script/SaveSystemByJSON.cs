using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;

public static class SaveSystemByJSON
{
    //玩家设置的数据结构体
    struct SettingData
    {
        bool gameAgain;

    }
    //玩家保存数据的路径
    public static string filePath;
    //设置的路径
    public static string filePathForSetting;
    /// <summary>
    /// 设置存档名字
    /// </summary>
    public static void SetFileName()
    {

    }
    /// <summary>
    /// 保存游戏内进程的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="saveObject"></param>
    public static void SaveDataFromGame<T>(T saveObject)
    {
        SetFilePath();
        string toJsonString = JsonConvert.SerializeObject(saveObject);
        System.IO.File.WriteAllText(filePath + @"\saveWithGame.txt", toJsonString);
        Debug.Log("保存成功！");
    }
    /// <summary>
    /// 加载用于游戏进程的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T LoadDataForGame<T>()
    {

        T TheGameObject = JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(filePath + @"\saveWithGame.txt"));
        return TheGameObject;
    }
    /// <summary>
    /// 保存游戏内玩家设置的数据
    /// </summary>
    private static void SaveDataForSetting()
    {

    }
    /// <summary>
    /// 获得程序运行路径
    /// </summary>
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //保存路径添加文件夹
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void SetFilePath()
    {
        filePath = System.IO.Directory.GetCurrentDirectory();
    }
    /*代码示例
     *  //保存
     *  player object = new player();
     *  保存
     *  SaveSystemByJSON.SaveDataFrom<player>(object)
     *  加载
     *  object = SaveSystemByJSON.LoadDataForGam<player>(); 
     */
}
