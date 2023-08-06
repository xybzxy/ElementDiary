using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUnit : MonoBehaviour
{
    public SaveManager savings;
    public int gameFileNumber;
    public string fileName;
    public string saveTime;
    public Text fileNameText;
    public Text saveTimeText;
    void Start()
    {
        savings = GameObject.FindWithTag("Canvas").GetComponent<SaveManager>();
        fileNameText.text = fileName;
        saveTimeText.text = saveTime;
    }
    void Update() 
    {
        saveTime = savings.saveTime;
        saveTimeText.text = saveTime;
        fileName = savings.datas[gameFileNumber].fileName;
        fileNameText.text = fileName;
    }
    public void LoadThisFile()
    {
        savings.gameFileNumber = gameFileNumber;
        savings.Load();
    }
    public void PrepareDelete()
    {
        savings.deleteFileNumber = gameFileNumber;
        transform.parent.GetComponent<SaveFileList>().MakeSure();
    }
}
