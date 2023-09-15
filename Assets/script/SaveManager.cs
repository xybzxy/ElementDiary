using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public int gameFileNumber;
    public GameDatas[] datas;
    public PlayerInfo player;
    public Labs[] labs;
    [Space]
    public Building[] labObjects;
    [Space]
    public int deleteFileNumber;
    public Transform fileList;
    public string saveTime;
    public void Start()
    {
        if(datas.Length == 0)
        {
            datas = new GameDatas[1];
        }
    }
    public void Save()
    {
        labs = new Labs[labObjects.Length];
        for (int i = 0; i < labObjects.Length-1; i++)
        {
            for (int j = 0; j < labs.Length-1; j++)
            {
                labs[i].parts[j].name = labObjects[i].builtParts[j].name;
                labs[i].parts[j].rotation = labObjects[i].builtParts[j].rotation;
            }
            labs[i].isBought = labObjects[i].isBought;
        }
        datas[gameFileNumber].playerInfo = player.plin;
        datas[gameFileNumber].buildings = labs;
        datas[gameFileNumber].saveTime = GetDateTime();
        saveTime = datas[gameFileNumber].saveTime;
        SaveSystemByJSON. SaveDataFromGame<GameDatas[]>(datas);
        //Debug.Log(SaveSystemByJSON.filePath);
    }
    public void Load()
    {
        datas = SaveSystemByJSON.LoadDataForGame<GameDatas[]>();
        player.plin = datas[gameFileNumber].playerInfo;
        labs = datas[gameFileNumber].buildings;
        for (int i = 0; i < labObjects.Length-1; i++)
        {
            for (int j = 0; j < labs.Length-1; j++)
            {
                labObjects[i].builtParts[j].name = labs[i].parts[j].name;
                labObjects[i].builtParts[j].rotation = labs[i].parts[j].rotation;
            }
            labObjects[i].isBought = labs[i].isBought;
        }
    }
    public void DeleteFile()//删除存档文件
    {
        int isFound = 0;
        datas[deleteFileNumber] = null;
        GameDatas[] saveForNow = datas;
        datas = new GameDatas[saveForNow.Length-1];
        for (int i = 0; i < datas.Length; i++)
        {
            if(saveForNow[i] == null)
            {
                isFound = -1;
                continue;
            }
            datas[i+isFound] = saveForNow[i];//从数据中移除该存档
        }
        for (int i = 0; i < fileList.childCount-1; i++)//改变剩余存档编号
        {
            if(i > deleteFileNumber)
            {
                fileList.GetChild(deleteFileNumber).GetComponent<SaveUnit>().gameFileNumber--;
            }
        }
        SaveSystemByJSON. SaveDataFromGame<GameDatas[]>(datas);//把更改后的数据保存在本地
        Destroy(fileList.GetChild(deleteFileNumber).gameObject);//删除存档槽位
    }
    string GetDateTime()
    {
        int hour = DateTime.Now.Hour;
        int minute = DateTime.Now.Minute;
        int second = DateTime.Now.Second;
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int day = DateTime.Now.Day;
        string dateTime = string.Format("{3:D4}/{4:D2}/{5:D2}" + " " +"{0:D2}:{1:D2}:{2:D2} ", hour, minute, second, year, month, day);
        return dateTime;
    }
}
[System.Serializable]
public class GameDatas
{
    public PlayerInfomation playerInfo;
    public Labs[] buildings;
    public string saveTime;
    public string fileName;
    //public Color color;
}
[System.Serializable]
public class Labs
{
    public bool isBought;
    public BuildingPartSaveInfo[] parts;
}
[System.Serializable]
public class BuildingPartSaveInfo
{
    public string name;
    public int rotation;
}
