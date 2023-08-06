using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileList : MonoBehaviour
{
    public SaveManager savings;
    public GameObject fileUIPrefab;
    public GameObject makeSurePanel;
    void Start()
    {
        GameDatas[] datas = savings.datas;
        if(datas.Length != 0)
        {
            for (int i = 0; i < datas.Length; i++)//生成存档槽位
            {
                GameObject file = Instantiate(fileUIPrefab,transform.position,transform.rotation,transform);
                SaveUnit unit = file.GetComponent<SaveUnit>();
                unit.saveTime = datas[i].saveTime;
                unit.fileName = datas[i].fileName;
            }
        }
    }
    
    public void MakeSure()
    {
        makeSurePanel.SetActive(true);
    }
}
