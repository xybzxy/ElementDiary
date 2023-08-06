using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    CheatCodeSystem cheatCodeSystem;
    public PlayerInfomation plin;
    void Start()
    {
        cheatCodeSystem = GameObject.FindWithTag("Canvas").GetComponent<CheatCodeSystem>();
    }
    public void AddMoney()
    {
        plin.money += cheatCodeSystem.cheatValue;
    }
    public void SetMoney()
    {
        plin.money = cheatCodeSystem.cheatValue;
    }
}
[System.Serializable]
public class PlayerInfomation
{
    public string playerName;
    public int playerNumber;
    //public Sprite playerImage;
    //public Vector3 playerPosition;
    public float money;
}