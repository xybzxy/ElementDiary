using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartInfo : MonoBehaviour
{
    public BuildingPart buildingPart;
    Building building;
    public int rotation;
    public int laterotation;
    void Start()
    {
        rotation = 0;
        laterotation = 0;
        buildingPart.enabled = true;
        buildingPart.thisPart = this.gameObject;
        building = transform.parent.parent.GetComponent<Building>();
    }
    void Update()
    {
        if(buildingPart.rotation >= 4)
        {
            buildingPart.rotation = 0;
        }
        rotation = buildingPart.rotation;
        if(rotation != laterotation)
        {
            RotatePart(rotation);
        }
    }
    private void LateUpdate() 
    {
        laterotation = buildingPart.rotation;
    }
    public void RotatePart(int spin)
    {
        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(spin).gameObject.SetActive(true);
    }
    void OnMouseDown()
    {
        if(building.toolNum == 3)
        {
            buildingPart.thisPart = this.gameObject;
            building.toDeletePart = this;
        }
    }
}
[System.Serializable]
public class BuildingPart
{
    public bool enabled;
    public string name;
    public Vector2 usedSpaces;
    public int buildingOrder;
    public int rotation;
    //public Sprite UIImage;
    public GameObject prefab;
    public GameObject thisPart;
    public float prise;
}
