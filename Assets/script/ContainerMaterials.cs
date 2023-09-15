using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerMaterials : MonoBehaviour
{
    public CMaterial[] materials;
    public CMaterial FindMaterials(string name)
    {
        CMaterial m = null;
        foreach (CMaterial material in materials)
        {
            if(material.name == name)
            {
                m = material;
            }
        }
        if(m == null)
        {
            Debug.LogError("没有找到"+name+"材质，请检查是否出错");
            return m;
        }
        else
        {
            return m;
        }
    }
}
[System.Serializable]
public class CMaterial
{
    public string name;
    public float heatTransferRate;//导热率
    public float heatCapacity;//比热容
    public float toughness;//韧性
}
