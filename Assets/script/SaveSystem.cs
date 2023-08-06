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
            File.WriteAllLines(@"E:\\Users\\��Ϸ\\ģ�⻯ѧ\\Assets\\GameDataDuringTest\\GameData.txt", Data);
            Debug.Log("IsSave");
        }
        else
        {
            Debug.LogError("�㱣����һ������������CanSaveAttribute���࣬���鱣������Ƿ���Ϲ涨");
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
            Data = File.ReadAllLines(@"E:\\Users\\��Ϸ\\ģ�⻯ѧ\\Assets\\GameDataDuringTest\\GameData.txt");
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
                Debug.LogError("���ڵ�����洢ʱ�������������죬���Բ鿴�ļ��е�ԭ��������ȡ����ϵ���޸�");
            }
        }
        else
        {
            Debug.LogError("�������һ������������CanSaveAttribute���࣬������ص����Ƿ���Ϲ涨�����");
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
