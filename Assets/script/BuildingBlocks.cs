using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlocks : MonoBehaviour
{
    Building building;
    public bool[] isUsed = new bool[4];
    public int blockNum;
    public Vector2 blockPos;
    public Color normalColor;
    void Start()
    {
        building = transform.parent.parent.parent.GetComponent<Building>();
        normalColor = GetComponent<SpriteRenderer>().color;
        blockPos = new Vector2(blockNum%building.grids.sizeDelta.x, blockNum/(int)(building.grids.sizeDelta.x));
    }
    void OnMouseOver() 
    {
        building.goalPos = this.transform.position;
        building.blockNum = blockNum;
    }
    void OnMouseEnter()
    {
        if(building.newPart != null)
        {
            building.ShowArea(blockPos);
        }
    }
    void OnMouseExit() 
    {
        building.ClearShowArea(normalColor);
    }
    void OnMouseDown()
    {
        building.clickedGrid = this;
    }
}
