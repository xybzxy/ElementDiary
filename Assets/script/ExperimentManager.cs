using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentManager : MonoBehaviour
{
    public ExperimentEquipment takingEquipment;
    public Camera cam;
    public float equipmentRotateSpeed;
    public float equipmentMoveSpeed;
    [Space(20)]
    public Text angleShow;
    float rotateAngle;
    public GameObject infoShow;
    public Text containerInfo;
    public Transform contentShow;
    public GameObject contentShowPrefab;
    [Space(20)]
    public float normalTemperature;
    public float airHeatTransferRate;
    void Update() 
    {
        TakeEquipment();
        if(infoShow.activeSelf)
        {
            infoShow.transform.position = Input.mousePosition;
        }
    }
    void TakeEquipment()
    {
        if(takingEquipment != null)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenToWorld = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y,10));
            takingEquipment.transform.position = Vector3.MoveTowards(takingEquipment.transform.position,screenToWorld,equipmentMoveSpeed);
            takingEquipment.transform.Rotate(Vector3.forward*(int)(equipmentRotateSpeed*100*Input.GetAxis("Mouse ScrollWheel")*Time.deltaTime));
            rotateAngle = GetInspectorRotationValueMethod(takingEquipment.transform).z;
            angleShow.text = (int)rotateAngle + "°";
            angleShow.transform.position = mousePos;
        }
        angleShow.gameObject.active = takingEquipment != null;
    }
    public void ShowContainerInfo(Container con)
    {
        containerInfo.text = con.name + "" + con.temperature.ToString("#0.00") + "℃"; 
    }
    public void ShowContents(Container con)
    {
        infoShow.SetActive(true);
        if(contentShow.childCount != 0)
        {
            for (int i = 0; i < contentShow.childCount; i++)
            {
                Destroy(contentShow.GetChild(i).gameObject);
            }
        }
        Chemical[] contents = con.contents;
        string[] names = new string[contents.Length];
        for (int i = 0; i < contents.Length; i++)
        {
            if(contents[i] != null)
            {
                names[i] = contents[i].name;
            }
            else
            {
                names[i] = "";
            }
        }
        ConcentratedChemicals[] infos = CountChemicals(names);
        for (int i = 0; i < infos.Length; i++)
        {
            if(infos[i].name != "")
            {
                GameObject textBoard = Instantiate(contentShowPrefab,contentShow.position,contentShow.rotation,contentShow);
                textBoard.transform.GetChild(0).GetComponent<Text>().text = infos[i].name + " " + infos[i].count*infos[i].FindParticleMass(infos[i].name,contents)*1000f + "g";
            }
        }
    }
    public ConcentratedChemicals[] CountChemicals(string[] chemicals)
    {
        Dictionary<string, int> chemicalCounts = new Dictionary<string, int>();

        // 统计化学品出现的次数
        foreach (string chem in chemicals)
        {
            if (chemicalCounts.ContainsKey(chem))
            {
                chemicalCounts[chem]++;
            }
            else
            {
                chemicalCounts[chem] = 1;
            }
        }

        // 创建合并相同化学品后的数据列表
        List<ConcentratedChemicals> concentratedChemicals = new List<ConcentratedChemicals>();
        foreach (KeyValuePair<string, int> pair in chemicalCounts)
        {
            ConcentratedChemicals concentratedChemical = new ConcentratedChemicals();
            concentratedChemical.name = pair.Key;
            concentratedChemical.count = pair.Value;            
            concentratedChemicals.Add(concentratedChemical);
        }
        ConcentratedChemicals[] concentratedChemicalsArray = ConvertListToArray<ConcentratedChemicals>(concentratedChemicals);
        // 返回合并相同化学品后的数据列表
        return concentratedChemicalsArray;
    }
    public T[] ConvertListToArray<T>(List<T> list)
    {
        T[] array = list.ToArray();
        return array;
    }
    public Vector3 GetInspectorRotationValueMethod(Transform transform)
    {
        // 获取原生值
        System.Type transformType = transform.GetType();
        PropertyInfo m_propertyInfo_rotationOrder = transformType.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
        object m_OldRotationOrder = m_propertyInfo_rotationOrder.GetValue(transform, null);
        MethodInfo m_methodInfo_GetLocalEulerAngles = transformType.GetMethod("GetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
        object value = m_methodInfo_GetLocalEulerAngles.Invoke(transform, new object[] { m_OldRotationOrder });
        string temp = value.ToString();
        //将字符串第一个和最后一个去掉
        temp = temp.Remove(0, 1);
        temp = temp.Remove(temp.Length - 1, 1);
        //用‘，’号分割
        string[] tempVector3;
        tempVector3 = temp.Split(',');
        //将分割好的数据传给Vector3
        Vector3 vector3 = new Vector3(float.Parse(tempVector3[0]), float.Parse(tempVector3[1]), float.Parse(tempVector3[2]));
        return vector3;
    }
}
public class ConcentratedChemicals
{
    public string name;
    public int count;
    public float FindParticleMass(string name, Chemical[] chemicals)
    {
        if(name != "")
        {
           float mass = 0;
            foreach (Chemical chemical in chemicals)
            {
                if(chemical != null && name == chemical.name)
                {
                    mass = chemical.massOfEachParticle;
                    break;
                }
            } 
            return mass;
        }
        else
        {
            return 0;
        }
    }
}
[System.Serializable]
public class Substance : IReactionObject
{
    public float colorR;
    public float colorG;
    public float colorB;
    public float colorA;
    public void ChangeColorForm(Color color)
    {
        Quaternion changedColorData = new Quaternion(color.r,color.g,color.b,color.a);
        colorR = changedColorData.x;
        colorG = changedColorData.y;
        colorB = changedColorData.z;
        colorA = changedColorData.w;
    }
}
