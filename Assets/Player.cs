using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;
    public float speed = 10f;
    public SaveDataObject SObject;
    public Text X;
    public Text Y;
    public Text Z;
    public Text NameUI;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D=GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") *speed);
        X.text = Convert.ToString(transform.position.x);
        Y.text = Convert.ToString(transform.position.y);
        Z.text = Convert.ToString(transform.position.z);
    }
    public void ClickSave()
    {
        Player player=GetComponent<Player>();
        SObject=new SaveDataObject(player,"LiLei");
         SaveSystem.Save<SaveDataObject>(SObject);
        //SaveSysteam.Save<Mydemo>(new Mydemo("这是一个测试"));
    }
    public void ClickLoad()
    {
        Player player = GetComponent<Player>();
        SObject = new SaveDataObject(player);
        SaveSystem.Load<SaveDataObject>(SObject);
        transform.position = new Vector3(SObject.X,SObject.Y,SObject.Z);
        NameUI.text = SObject.playerName;
        Debug.Log("IsLoad");
    }

}
[CanSave(author = "小羊宝正", versionNumber = "版本1.2",isCheck =true)]
public class SaveDataObject
{
    
    public float X;
    public float Y;
    public float Z;
    public string playerName;

    public  SaveDataObject(Player player,string name)
    {
        X = player.transform.position.x;
        Y = player.transform.position.y;
        Z = player.transform.position.z;
        playerName = name;
    }
    public SaveDataObject(Player player)
    {
        X = player.transform.position.x;
        Y = player.transform.position.y;
        Z = player.transform.position.z;
        
    }
}
[CanSave(author ="小羊宝正",versionNumber ="版本1.1",isCheck =true)]
public class Mydemo
{
    public string Demo;
    public Mydemo(string d)
    {
        Demo = d;
    }
}