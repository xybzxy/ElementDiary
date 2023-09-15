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
    void Start()
    {
        substances = JsonConvert.DeserializeObject<List<Substance>>(System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + @"\substances.txt"));
        foreach (Substance substance in substances)
        {
            ShowSubstanceContent(substance);
        }
        ScrollView();
        /*
        string[] reactants = {"氯酸钾"};
        string[] resultants = {"氯化钾","氧气"};
        condition = new ReactionCondition(400f,"s",null);
        equation = new Equation(condition,reactants,resultants,0,10);
        Debug.Log(equation.strKey);
        string toJsonString = JsonConvert.SerializeObject(equation);
        System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + @"\equations.txt", toJsonString);
        Debug.Log("保存成功！");
        */
    }
    void Update()
    {
        outputColor = QuaternionToColor(new Quaternion(colorR.value,colorG.value,colorB.value,colorA.value));
        colorBoard.color = outputColor;
    }
    void ScrollView()
    {
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0,content.childCount * substancePrefab.GetComponent<RectTransform>().sizeDelta.y);
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
        ScrollView();
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
}
