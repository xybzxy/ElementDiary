using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingBought : MonoBehaviour
{
    public float prise;
    public bool isBought;
    public GameObject buyBoard;
    public GameObject managerBoard;
    public Building building;
    void Update()
    {
        ChangeBoards();
    }
    void ChangeBoards()
    {
        buyBoard.SetActive(!isBought);
        managerBoard.SetActive(isBought&&!building.isBuilding);
    }
    public void SendThisInfo(BuyingPedal pedal)
    {
        pedal.thing = this;
        pedal.prise = prise;
    }
}
