using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Security.Cryptography;


/// <summary>
/// 合成系统
/// </summary>
public class ReactionSystem
{
    private ReactionCondition rc;
    private List<IReactionObject> reactionObjects;
    public bool isHaveReaction;
    public string[] equations;
    public List<string> equationsList;
    private string[] ReactionObjectName;
    public string returnEquation;
    public ReactionSystem(List<IReactionObject> RObj, ReactionCondition obj)
    {
        ReactionObjectName = new string[RObj.Count];
        int i = 0;
        rc = obj;
        reactionObjects = RObj;
        equations = File.ReadAllLines(System.IO.Directory.GetCurrentDirectory() + @"\");
        string key;
        foreach (string eq in equations)
        {
            equationsList.Add(eq);
        }
        foreach (IReactionObject eq in RObj)
        {
            ReactionObjectName[i] = eq.name;
            i++;
        }
        string m = "";
        foreach (string reactantName in ReactionObjectName)
        {
            m += reactantName;
        }

        key = KeyMaker(m);
        foreach (string eq in equationsList)
        {
            if (eq.Contains(key))
            {
                returnEquation = eq;
                isHaveReaction = true;
                break;
            }
            else
            {
                isHaveReaction = false;
            }
        }


    }
    /// <summary>
    /// 启动反应系统
    /// </summary>
    public void Start()
    {

    }
    public void FoundEquation()
    {

    }
    static string KeyMaker(string sSourceData)
    {

        byte[] tmpSource;
        byte[] tmpHash;
        //Create a byte array from source data.
        tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);
        tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
        return ByteArrayToString(tmpHash);

    }
    static string ByteArrayToString(byte[] arrInput)
    {
        int i;
        StringBuilder sOutput = new StringBuilder(arrInput.Length);
        for (i = 0; i < arrInput.Length; i++)
        {
            sOutput.Append(arrInput[i].ToString("X2"));
        }
        return sOutput.ToString();
    }

}
/// <summary>
/// 反应条件类
/// </summary>
public class ReactionCondition
{
    //温度
    public float temperatrue;
    //环境
    public string environment;
    //催化剂
    public IReactionObject catalyzer;
    public ReactionCondition(float tp, string en, IReactionObject ca)
    {
        this.temperatrue = tp;
        this.environment = en;
        this.catalyzer = ca;
    }
}
//配方类
public class Equation
{
    //反应条件
    public ReactionCondition rc;
    //反应物
    public string[] reactantsNames;
    //生成物
    public string[] resultantsNames;
    //生成的热量
    public float generatedHeat;
    //反应速率
    public float velocity;
    //方程式标识
    public string strKey;

    public Equation(ReactionCondition reaction, string[] reactantsNames, string[] resultantsNames, float generatedHeat, float velocity)
    {
        this.rc = reaction;
        this.reactantsNames = reactantsNames;
        this.resultantsNames = resultantsNames;
        this.generatedHeat = generatedHeat;
        this.velocity = velocity;
        string m = "";
        foreach (string reactantName in reactantsNames)
        {
            m += reactantName;
        }
        this.strKey = KeyMaker(m);

    }
    /// <summary>
    /// 排序算法
    /// </summary>
    /// <param name="ints"></param>
    /// <returns></returns>
    private static int[] Paixv(int[] ints)
    {
        int[] list = ints;
        int num = list.Length - 1;
        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < num - i; j++)
            {
                if (list[j] > list[j + 1])
                {
                    int temp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = temp;
                }
            }
        }
        return list;

    }
    static string KeyMaker(string sSourceData)
    {

        byte[] tmpSource;
        byte[] tmpHash;
        //Create a byte array from source data.
        tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);
        tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
        return ByteArrayToString(tmpHash);

    }
    static string ByteArrayToString(byte[] arrInput)
    {
        int i;
        StringBuilder sOutput = new StringBuilder(arrInput.Length);
        for (i = 0; i < arrInput.Length; i++)
        {
            sOutput.Append(arrInput[i].ToString("X2"));
        }
        return sOutput.ToString();
    }
}
/// <summary>
/// 基础物质
/// </summary>
[System.Serializable]
public partial class IReactionObject
{
    public string name;
    public float molecularMass;
    public float meltingPoint;
    public float boilingPoint;
    public float viscosity;
    public bool isPolarMolecule;
}



