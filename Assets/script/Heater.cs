using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heater : MonoBehaviour
{
    [Header("用电")]
    public bool useElectricity;
    public bool isWorking;
    public float power;//功率
    public float efficiency;//效率
    public float temperature;//温度
    Container con;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<Container>(out var container))
        {
            con = container;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<Container>(out var container))
        {
            con = null;
        }
    }
    void Update()
    {
        if(con != null)
            Heating();
    }
    void Heating()
    {
        if(con.temperature < temperature)
        {
            con.heat += con.material.heatTransferRate*con.bottomArea*((temperature - con.temperature + 273.15f)/con.thickness)*Time.deltaTime*0.0001f;
        }
    }
}
