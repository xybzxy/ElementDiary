using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemical : MonoBehaviour
{
    public string name;
    public float temperature;
    public Substance substance;
    public float size;
    public float massOfEachParticle;
    public string state;
    Rigidbody2D rg;
    public ExperimentManager manager;
    void Start()
    {
        Reaction reaction = new Reaction();
        substance = reaction.FindSubstance(name);
        rg = GetComponent<Rigidbody2D>();
        rg.mass = massOfEachParticle;
        this.GetComponent<SpriteRenderer>().color = QuaternionToColor(new Quaternion(substance.colorR,substance.colorG,substance.colorB,substance.colorA));
        manager = transform.parent.GetComponent<ExperimentManager>();
        temperature = manager.normalTemperature;
        massOfEachParticle = substance.molecularMass*0.001f;
    }
    void Update()
    {
        if(temperature > substance.meltingPoint + 1f && temperature < substance.boilingPoint - 1f)
        {
            state = "l";
        }
        transform.localScale = new Vector3(size,size,size);
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        if(state == "l" && other.gameObject.TryGetComponent<Chemical>(out var chemical))
        {
            if(chemical.substance.isPolarMolecule == substance.isPolarMolecule)
            {
                MoveToPosition(rg,other.transform.position,substance.viscosity);
            }
        }
    }
    void MoveToPosition(Rigidbody2D rb, Vector3 aim, float force)
    {
        Vector3 rbPos = rb.transform.position;
        Vector2 direction = new Vector2((aim.x - rbPos.x)*force*0.001f,(aim.y - rbPos.y)*force*0.001f);
        rb.AddForce(direction);
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
}
