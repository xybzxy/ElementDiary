using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyingPedal : MonoBehaviour
{
    public float prise;
    public Text prizeShow;
    public ThingBought thing;
    public PlayerInfo plin;
    void Update()
    {
        prizeShow.text = "售价"+prise+"元";
    }
    public void Buy()
    {
        if(plin.plin.money >= prise)
        {
            plin.plin.money -= prise;
            thing.isBought = true;
            this.gameObject.SetActive(false);
        }
    }
}
