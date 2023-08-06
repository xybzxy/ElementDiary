using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueShowOnUI : MonoBehaviour
{
    public GameObject player;
    public Text moneyShow;
    PlayerInfomation playerInfo;
    void Update() 
    {
        playerInfo = player.GetComponent<PlayerInfo>().plin;
        moneyShow.text = playerInfo.money.ToString();
    }
}
