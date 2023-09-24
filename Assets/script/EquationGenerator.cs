using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class EquationGenerator : MonoBehaviour
{
    [Header("生成新的物质")]
    public Substance newSubstance;
    public InputField nameInput;
    public InputField molecularMassInput;
    public InputField meltingPointInput;
    public InputField boilingPointInput;
    public InputField viscosityInput;
    public Slider colorR;
    public Slider colorG;
    public Slider colorB;
    public Slider colorA;
    public Color outputColor;
    public Image colorBoard;
    public Toggle isPolarMoleculeInput;
    public GameObject substancePrefab;
    public Transform content;
    [Space(20)]
    public ReactionCondition condition;
    public Equation equation;
    [Space(20)]
    public List<Substance> substances;

    [Space(20)]
    [Header("生成新的方程式")]
    public GameObject equationPrefab;
    public Transform reactantShow;
    public Transform resultantShow;
    public List<Reactant> reactants;
    public List<Reactant> resultants;
    public InputField temperatrueInput;
    public InputField environmentInput;
    public InputField catalyzerInput;
    public InputField heatInput;
    public InputField velocityInput;
    public List<Equation> equations;
    void Start()
    {
        equations = JsonConvert.DeserializeObject<List<Equation>>(System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + @"\equations.txt"));
        substances = JsonConvert.DeserializeObject<List<Substance>>(System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + @"\substances.txt"));
        foreach (Substance substance in substances)
        {
            ShowSubstanceContent(substance);
        }
        ScrollView(content,substancePrefab);
    }
    void Update()
    {
        outputColor = QuaternionToColor(new Quaternion(colorR.value,colorG.value,colorB.value,colorA.value));
        colorBoard.color = outputColor;
    }
    void ScrollView(Transform content,GameObject prefab)
    {
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0,content.childCount * prefab.GetComponent<RectTransform>().sizeDelta.y);
    }
    Color QuaternionToColor(Quaternion qua)
    {
        Color color = new Color(0,0,0,0);
        color.r = qua.x;
        color.g = qua.y;
        color.b = qua.z;
        color.a = qua.w;
        return color;
    }
    public void AddNewSubstance()
    {
        newSubstance.name = nameInput.text;
        newSubstance.molecularMass = float.Parse(molecularMassInput.text);
        newSubstance.meltingPoint = float.Parse(meltingPointInput.text);
        newSubstance.boilingPoint = float.Parse(boilingPointInput.text);
        newSubstance.viscosity = float.Parse(viscosityInput.text);
        newSubstance.isPolarMolecule = isPolarMoleculeInput.isOn;
        newSubstance.ChangeColorForm(outputColor);
        bool isStoraged = false;
        foreach (Substance substance in substances)
        {
            if(string.Compare(substance.name,newSubstance.name) == 0)
            {
                substances.Insert(substances.FindIndex(x => x == substance),newSubstance);
                substances.Remove(substance);
                isStoraged = true;
                break;
            }
        }
        if(!isStoraged)
        {
            substances.Add(newSubstance);
        }
        newSubstance = null;
        string toJsonString = JsonConvert.SerializeObject(substances);
        System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + @"\substances.txt", toJsonString);
        int j = content.childCount;
        for (int i = 0; i < j; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        foreach (Substance substance in substances)
        {
            ShowSubstanceContent(substance);
        }
        ScrollView(content,substancePrefab);
    }
    void ShowSubstanceContent(Substance sub)
    {
        GameObject substanceShow = Instantiate(substancePrefab,content.position,content.rotation,content);
        substanceShow.GetComponent<SubstanceShow>().substance = sub;
    }
    public void ShowStoragedSubstance(Substance substance)
    {
        nameInput.text = substance.name;
        molecularMassInput.text = substance.molecularMass.ToString();
        meltingPointInput.text = substance.meltingPoint.ToString();
        boilingPointInput.text = substance.boilingPoint.ToString();
        viscosityInput.text = substance.viscosity.ToString();
        isPolarMoleculeInput.isOn = substance.isPolarMolecule;
        colorR.value = substance.colorR;
        colorG.value = substance.colorG;
        colorB.value = substance.colorB;
        colorA.value = substance.colorA;
    }
    //生成新的配方
    public void AddNewSubstanceToList(string type)
    {
        Transform list;
        if(type == "Reactant")
        {
            list = reactantShow;
        }
        else
        {
            list = resultantShow;
        }
        GameObject newSubstanceShow = Instantiate(equationPrefab,list.position,list.rotation,list);
        Reactant reactant = new Reactant(newSubstanceShow.transform.GetChild(0).GetComponent<InputField>(),newSubstanceShow.transform.GetChild(1).GetComponent<InputField>());
        if(type == "Reactant")
        {
            reactants.Add(reactant);
        }
        else
        {
            resultants.Add(reactant);
        }
        ScrollView(list,equationPrefab);
    }
    public void GenerateEquaration()
    {
        int i = 0;
        int j = 0;
        if(resultants.Capacity != 0 && reactants.Capacity != 0)
        {
            string[] reactantsStr = new string[reactants.Count];
            string[] resultantsStr = new string[resultants.Count];
            foreach (Reactant reactant in reactants)
            {
                reactant.GetString();
                reactantsStr[i] = reactant.count + reactant.name;
                i++;
            }
            foreach (Reactant resultant in resultants)
            {
                resultant.GetString();
                resultantsStr[j] = resultant.count + resultant.name;
                j++;
            }
            ReactionCondition condition = new ReactionCondition(float.Parse(temperatrueInput.text),environmentInput.text,null);
            Equation equation = new Equation(condition,reactantsStr,resultantsStr,float.Parse(heatInput.text),float.Parse(velocityInput.text));
            if(!equations.Contains(equation))
            {
                equations.Add(equation);
            }
            //Debug.Log(equation.strKey);
            string toJsonString = JsonConvert.SerializeObject(equations);
            System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + @"\equations.txt", toJsonString);
            //Debug.Log("保存成功！");
            reactants.Clear();
            resultants.Clear();
            for (int a = 0; a < resultantShow.childCount; a++)
            {
                Destroy(resultantShow.GetChild(a).gameObject);
            }
            for (int a = 0; a < reactantShow.childCount; a++)
            {
                Destroy(reactantShow.GetChild(a).gameObject);
            }
        }
    }
}
[System.Serializable]
public class Reactant
{
    public InputField countInput;
    public InputField nameInput;
    public string count;
    public string name;
    public Reactant(InputField co,InputField na)
    {
        countInput = co;
        nameInput = na;
    }
    public void GetString()
    {
        count = countInput.text;
        name = nameInput.text;
    }
}
