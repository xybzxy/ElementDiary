using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubstanceShow : MonoBehaviour
{
    public EquationGenerator generator;
    public Substance substance;
    Text name;
    void Start()
    {
        generator = this.transform.parent.parent.parent.parent.parent.GetComponent<EquationGenerator>();
        name = this.transform.GetChild(0).GetComponent<Text>();
        name.text = substance.name;
    }
    public void ShowSubstance()
    {
        generator.ShowStoragedSubstance(substance);
    }
}
