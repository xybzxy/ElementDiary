using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;

public class SaveSystem
{
   public static void Save<T>(T obj)
    {
        int N=0;
        string[]Data=new string[10];
        Type type = typeof(T);
        if (type.IsDefined(typeof(CanSaveAttribute), false))
        {
            CanSaveAttribute can=type.GetCustomAttribute<CanSaveAttribute>();
            FieldInfo[] fields = type.GetFields();
            Data[N] = can.author;
            N++;
            Data[N] = can.versionNumber;
            N++;
            foreach (FieldInfo field in fields)
            {
                //field.GetValue(obiect);
                Data[N] = Convert.ToString(field.GetValue(obj));
                N++;

                // Debug.Log(field.GetValue(obj));
            }
            File.WriteAllLines(@"E:\\Users\\游戏\\模拟化学\\Assets\\GameDataDuringTest\\GameData.txt", Data);
            Debug.Log("IsSave");
        }
        else
        {
            Debug.LogError("你保存了一个不包含特性CanSaveAttribute的类，请检查保存的类是否符合规定");
        }
    }
    public static void Load<T>(T obj)
    {
        int N = 0;
        Type type = typeof(T);
        if (type.IsDefined(typeof(CanSaveAttribute), false))
        {
            CanSaveAttribute can = type.GetCustomAttribute<CanSaveAttribute>();
            FieldInfo[] fields = type.GetFields();
            string[] Data = new string[10];
            Data = File.ReadAllLines(@"E:\\Users\\游戏\\模拟化学\\Assets\\GameDataDuringTest\\GameData.txt");
            if (Data[N] == can.author && Data[N+1]==can.versionNumber)
            {
                N = N + 2;
                //PropertyInfo[] propertyInfos = type.GetProperties();
                foreach (FieldInfo field in fields)
                {
                    switch (Convert.ToString(field.FieldType))
                    {
                        case "System.Int32":
                            field.SetValue(obj, Convert.ToInt32(Data[N]));
                            break;
                        case "System.Double":
                            field.SetValue(obj, Convert.ToDouble(Data[N]));
                            break;
                        case "System.String":
                            field.SetValue(obj, Data[N]);
                            break;
                        case "System.Single":
                            field.SetValue(obj, Convert.ToSingle(Data[N]));
                            break;
                        case "System.Decimal":
                            field.SetValue(obj, Convert.ToDecimal(Data[N]));
                            break;
                        case "System.Boolean":
                            field.SetValue(obj, Convert.ToBoolean(Data[N]));
                            break;



                    }
                    Debug.Log(Convert.ToString(field.FieldType));
                    N++;
                }
            }
            else
            {
                Debug.LogError("现在的类与存储时的特性有所差异，可以查看文件中的原作者名字取得联系并修改");
            }
        }
        else
        {
            Debug.LogError("你加载了一个不包含特性CanSaveAttribute的类，请检查加载的类是否符合规定或存在");
        }
    }
    
}
[AttributeUsage(AttributeTargets.Class)]
public class CanSaveAttribute : System.Attribute
{
    public string author="";
    public string versionNumber="";
    public bool isCheck;
}
