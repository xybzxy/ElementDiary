using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Container : MonoBehaviour
{
    public float temperature;//当前温度
    public string materialName;//材质
    public string containerName;//容器名字
    public float bottomArea;//底面积
    public float thickness;//厚度
    public float mass;//质量
    public float heat;//热量
    float brokenSpeed;//摔坏的最小速度
    float normalTemperature;
    float airHeatTransferRate;
    Rigidbody2D rg;
    public CMaterial material;
    [Header("内容物")]
    public Chemical[] contents;
    public List<Chemical> contentsList; 
    Transform manager;
    void Start()
    {
        rg = this.GetComponent<Rigidbody2D>();
        rg.mass = mass;
        manager = this.transform.parent;
        material = manager.GetComponent<ContainerMaterials>().FindMaterials(materialName);
        normalTemperature = manager.GetComponent<ExperimentManager>().normalTemperature;
        airHeatTransferRate = manager.GetComponent<ExperimentManager>().airHeatTransferRate;
        heat = material.heatCapacity*mass*(normalTemperature+ 273.15f);
    }
    void Update()
    {
        TemperatureCalculate();
        HeatRadiation();
        contents = contentsList.ToArray();
    }
    void TemperatureCalculate()
    {
        temperature = heat/(material.heatCapacity*mass) - 273.15f;
    }
    void HeatRadiation()
    {
        if(temperature > normalTemperature)
        {
            heat += -airHeatTransferRate*bottomArea*((temperature - normalTemperature + 273.15f)/thickness)*Time.deltaTime*0.001f;
        }
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Chemical" && this.transform.GetChild(0).gameObject.active == true && other.transform.parent != this.transform.GetChild(0))
        {
            other.transform.parent = this.transform.GetChild(0);
            Chemical chem = other.GetComponent<Chemical>();
            if(!contentsList.Contains(chem))
            {
                contentsList.Add(chem);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Chemical" && this.transform.GetChild(0).gameObject.active == true)
        {
            if(this.transform.GetChild(0).childCount != 0)
            {
                Chemical chem = other.GetComponent<Chemical>();
                if(contentsList.Contains(chem) && contentsList.Count != 0)
                {
                    contentsList.Remove(chem);
                }
                other.transform.SetParent(manager);
            }
        }
    }
    /*
    public T[] AddEmptyElements<T>(T[] array, int count)
    {
        T[] newArray = new T[array.Length + count];
        
        for (int i = 0; i < array.Length; i++)
        {
            newArray[i] = array[i];
        }
        
        return newArray;
    }
    */
}
