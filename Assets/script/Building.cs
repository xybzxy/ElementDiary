using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Transform cameraPoint;
    public bool isBuilding;
    public bool isBought;
    [Space]
    [Header("需要设置的变量")]
    public GameObject grid;
    public RectTransform grids;
    public float eachGridPrize;
    public float partPrise;
    [Space]
    public int toolNum;
    public PartInfo toDeletePart;
    [Space]
    public BuildingBlocks[] blocks;
    public float gridCount;
    public Transform partParent;
    public GameObject newPart;
    public Vector3 goalPos;
    public int blockNum;
    public Color buildableArea;
    public Color unbuildableArea;
    public bool isBuildable;
    public GameObject[] foundBlocksShow;
    public BuildingBlocks clickedGrid;
    public Vector2 toBuildArea;
    public float cameraY;
    public float cameraUpMovement = 1;
    [Space]
    public BuildingPart[] builtParts;
    [Space]
    //单个功能变量区
    int clickStep;
    bool canBatch = true;
    bool drawed;
    GameObject deletePart;
    public Vector3 startPos;
    public Vector2 startGrid;
    public bool isDeleting;
    void Start()
    {
        PlaceGrids();
        //Debug.Log(FindBlocks(12,new Vector2(1,2))[1]);
        this.GetComponent<ThingBought>().prise = eachGridPrize * gridCount;
        isBought = this.GetComponent<ThingBought>().isBought;
    }
    public void SendThisInfo(Buildingmode pedal)
    {
        pedal.point = cameraPoint;
        pedal.building = this;
    }
    void Update()
    {
        CameraMove(Input.GetButton("Fire2")&&isBuilding,Input.GetKeyDown(KeyCode.F));
        DuringBuilding();
    }
    void CameraMove(bool canMove,bool isBack)
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        cameraPoint.position =  new Vector3(cameraPoint.position.x,cameraPoint.position.y,cameraY*cameraUpMovement);
        if(canMove)
        {
            cameraPoint.Translate(x,y,0);
        }
        else if(isBack)
        {
            cameraPoint.position = new Vector3(this.transform.position.x,this.transform.position.y,cameraY);
        }
    }
    void PlaceGrids()
    {
        gridCount = grids.sizeDelta.x*grids.sizeDelta.y;
        blocks = new BuildingBlocks[(int)gridCount];
        for (int i = 0; i < gridCount; i++)
        {
            blocks[i] = Instantiate(grid,grids.position,grids.rotation,grids).transform.GetChild(0).GetComponent<BuildingBlocks>();
            blocks[i].blockNum = i;
        }
    }
    void DuringBuilding()
    {
        grids.gameObject.SetActive(isBuilding&&toolNum != 3);
        if(isBuilding && newPart != null)
        {
            newPart.transform.position = new Vector3(goalPos.x,goalPos.y,newPart.transform.position.z);
            if(Input.GetKeyDown(KeyCode.O))
            {
                newPart.GetComponent<PartInfo>().buildingPart.rotation++;
            }
        }
        switch (toolNum)
        {
            case 0:
                if(isBuilding && newPart != null)
                {
                    toBuildArea = newPart.GetComponent<PartInfo>().buildingPart.usedSpaces;
                    isBuildable = blocks[blockNum].blockPos.x+toBuildArea.x<=grids.sizeDelta.x&&blocks[blockNum].blockPos.y+toBuildArea.y<=grids.sizeDelta.y&&IfIsUsed(newPart.GetComponent<PartInfo>().buildingPart.buildingOrder);
                }
                if(Input.GetButtonDown("Fire1")&&isBuildable&&!isDeleting)
                {
                    BuildPart();
                }
                break;
            case 1:
                if(newPart != null)
                {
                    isBuildable = startGrid.x+toBuildArea.x<=grids.sizeDelta.x&&startGrid.y+toBuildArea.y<=grids.sizeDelta.y&&IfIsUsed(newPart.GetComponent<PartInfo>().buildingPart.buildingOrder);
                    //Debug.Log(startGrid.x+toBuildArea.x<=grids.sizeDelta.x);
                    //Debug.Log(startGrid.y+toBuildArea.y<=grids.sizeDelta.y);
                    //Debug.Log(IfIsUsed(newPart.GetComponent<PartInfo>().buildingPart.buildingOrder));
                    BatchBuilding();
                }
                break;
            case 2:
               if(newPart != null)
               {
                    toBuildArea = newPart.GetComponent<PartInfo>().buildingPart.usedSpaces;
                    isBuildable = blocks[blockNum].blockPos.x+toBuildArea.x<=grids.sizeDelta.x&&blocks[blockNum].blockPos.y+toBuildArea.y<=grids.sizeDelta.y&&IfIsUsed(newPart.GetComponent<PartInfo>().buildingPart.buildingOrder);
                    ContinuouslyBuild();
               }
               break;
            case 3:
               if(isBuilding&&toDeletePart != null&&toDeletePart.gameObject != null)
               {
                    newPart = null;
                    DeleteBuiltPart();
               }
               break;
        }
    }
    void BuildPart()//建造部件
    {
        BuildingPart[] storageForNow = builtParts;
        builtParts = new BuildingPart[storageForNow.Length+1];
        for (int i = 0; i < storageForNow.Length; i++)
        {
            builtParts[i] = storageForNow[i];
        }
        builtParts[builtParts.Length-1] = newPart.GetComponent<PartInfo>().buildingPart;
        partPrise += builtParts[builtParts.Length-1].prise;
        foreach (GameObject block in foundBlocksShow)
        {
            block.GetComponent<BuildingBlocks>().isUsed[newPart.GetComponent<PartInfo>().buildingPart.buildingOrder] = true;
        }
        newPart = null;
        isBuildable = false;
    }
    void BatchBuilding()//批量建造
    {
        Vector2 range = newPart.GetComponent<PartInfo>().buildingPart.usedSpaces;
        if(Input.GetButtonDown("Fire1")&&isBuildable)
        {
            clickStep++;
            switch (clickStep)
            {
                case 1:
                    startPos = goalPos;
                    startGrid = clickedGrid.blockPos;
                    break;
                case 2:
                    Vector3 partPos;
                    int partCountX = (int)(toBuildArea.x/range.x);
                    int partCountY = (int)(toBuildArea.y/range.y); 
                    int partCount = partCountX*partCountY;
                    //添加新的零件信息
                    BuildingPart[] storageForNow = builtParts;
                    builtParts = new BuildingPart[storageForNow.Length+partCount];
                    for (int i = 0; i < storageForNow.Length; i++)
                    {
                        builtParts[i] = storageForNow[i];
                    }
                    //生成零件模型
                    for (int i = 0; i < partCountX; i++)
                    {
                        for (int j = 0; j < partCountY; j++)
                        {
                            partPos = new Vector3(startPos.x-i*range.x,startPos.y+j*range.y,newPart.transform.position.z);
                            GameObject newRealPart = Instantiate(newPart,partPos,transform.rotation,partParent);
                            builtParts[builtParts.Length - partCount + i*partCountY + j] = newRealPart.GetComponent<PartInfo>().buildingPart;
                            partPrise += newRealPart.GetComponent<PartInfo>().buildingPart.prise;
                            //Debug.Log("生成");
                        }
                    }
                    foundBlocksShow = FindBlocks(startGrid,new Vector2(partCountX*range.x,partCountY*range.y));
                    foreach (GameObject block in foundBlocksShow)
                    {
                        block.GetComponent<BuildingBlocks>().isUsed[newPart.GetComponent<PartInfo>().buildingPart.buildingOrder] = true;
                    }
                    foundBlocksShow = FindBlocks(startGrid,toBuildArea);
                    Destroy(newPart);//删除母零件
                    canBatch = true;
                    newPart = null;
                    isBuildable = false;
                    clickStep = 0;
                    toBuildArea = new Vector2(0,0);
                    //Debug.Log("结束");
                    break;
            }
        }
        switch (clickStep)
        {
            case 1:
                int areaX;
                int areaY;
                int batchAreaX = (int)(goalPos.x-startPos.x);
                int batchAreaY = (int)(goalPos.y-startPos.y);
                areaX = Mathf.Abs(batchAreaX) + (int)range.x;
                areaY = Mathf.Abs(batchAreaY) + (int)range.y;
                toBuildArea = new Vector2(areaX,areaY);
                canBatch = batchAreaX < 0 && batchAreaY > 0;
                break;
        }
    }
    void ContinuouslyBuild()//连续建造
    {
        if(Input.GetButton("Fire1")&&isBuildable)
        {
            //生成建筑模型
            GameObject newRealPart = Instantiate(newPart,new Vector3(goalPos.x,goalPos.y,newPart.transform.position.z),transform.rotation,partParent);
            //添加新建筑信息
            BuildingPart[] storageForNow = builtParts;
            builtParts = new BuildingPart[storageForNow.Length+1];
            for (int i = 0; i < storageForNow.Length; i++)
            {
                builtParts[i] = storageForNow[i];
            }
            builtParts[builtParts.Length-1] = newRealPart.GetComponent<PartInfo>().buildingPart;
            partPrise += builtParts[builtParts.Length-1].prise;
            //占用建筑格子
            foreach (GameObject block in foundBlocksShow)
            {
                block.GetComponent<BuildingBlocks>().isUsed[newPart.GetComponent<PartInfo>().buildingPart.buildingOrder] = true;
            }
            isBuildable = false;
            drawed = true;
        }
        else if(Input.GetButtonUp("Fire1")&&drawed)
        {
            Destroy(newPart);
            newPart = null;
            drawed = false;
        }
    }
    void DeleteBuiltPart()//拆除部件
    {
        foreach (BuildingPart part in builtParts)
        {
            if(part.thisPart != null&&part.thisPart == toDeletePart.gameObject)
            {
                //删除建筑信息
                Vector2 origin = new Vector2(0,0);
                part.enabled = false;
                bool foundDeleteObject = false;
                BuildingPart toDelete = null;
                BuildingPart lastPart;
                if(part != builtParts[builtParts.Length-1])
                    toDelete = part;
                    lastPart = builtParts[builtParts.Length-1];
                    builtParts[builtParts.Length-1] = toDelete;
                    for (int i = 0; i < builtParts.Length-1; i++)
                    {
                        if(!builtParts[i].enabled)
                        {
                            builtParts[i] = lastPart;
                            break;
                        }                    
                    }
                //Debug.Log(builtParts.Length);
                partPrise -= part.prise;
                BuildingPart[] storageForNow = builtParts;
                builtParts = new BuildingPart[storageForNow.Length-1];
                for (int i = 0; i < builtParts.Length; i++)
                {
                    builtParts[i] = storageForNow[i];
                }
                //解除占用建筑格子
                foreach (BuildingBlocks block in blocks)
                {
                    if(block.transform.position.x == toDeletePart.transform.position.x&&block.transform.position.y == toDeletePart.transform.position.y)
                    {
                        origin = block.blockPos;
                    }
                }
                GameObject[] toReuseBlocksObjects = FindBlocks(origin,toDeletePart.buildingPart.usedSpaces);
                BuildingBlocks[] toReuseBlocks = new BuildingBlocks[toReuseBlocksObjects.Length];
                for (int i = 0; i < toReuseBlocks.Length; i++)
                {
                    toReuseBlocks[i] = toReuseBlocksObjects[i].GetComponent<BuildingBlocks>();
                }
                foreach (BuildingBlocks block in toReuseBlocks)
                {
                    block.isUsed[toDeletePart.buildingPart.buildingOrder] = false;
                }
                //删除建筑部件模型
                deletePart = toDeletePart.gameObject;
                break;
            }
        }
        toDeletePart = null;
        Destroy(deletePart);
    }
    bool IfIsUsed(int order)//确认该区域未被占用
    {
        bool used = false;
        if(foundBlocksShow.Length != 0)
        {
            for (int i = 0; i < foundBlocksShow.Length-1; i++)
            {
                if(foundBlocksShow[i] == null)
                    break;
                used = !foundBlocksShow[i].GetComponent<BuildingBlocks>().isUsed[order];
                if(!used)
                    break;
            }
        }
        else
        {
            used = true;
        } 
        return used;
    }
    public GameObject[] FindBlocks(Vector2 origin,Vector2 range)//寻找建筑格子
    {
        int foundBlocksCount = 0;
        int blocksCount = 0;
        blocksCount = (int)(range.x*range.y);
        GameObject[] foundBlocks = new GameObject[blocksCount];
        for (int i = (int)origin.y; i < origin.y+range.y; i++)
        {
            for (int j = (int)origin.x; j < origin.x+range.x; j++)
            {
                if(j+(int)(grids.sizeDelta.x*i)>blocks.Length-1)
                    break;
                foundBlocks[foundBlocksCount] = blocks[j+(int)(grids.sizeDelta.x*i)].gameObject;
                foundBlocksCount++;
            }
        }
        return foundBlocks;
    }
    public void ShowArea(Vector2 point)
    {
        if(newPart != null)
        {
            switch(toolNum)
            {
                case 0:
                    foundBlocksShow = FindBlocks(point,toBuildArea);
                    break;
                case 1:
                    if(canBatch)
                    {
                        foundBlocksShow = FindBlocks(startGrid,toBuildArea);
                    }
                    break;
                case 2:
                    foundBlocksShow = FindBlocks(point,toBuildArea);
                    break;
                case 3:
                    break;
            }
        }
        GameObject[] toShowBlocks = foundBlocksShow;
        foreach (GameObject block in toShowBlocks)
        {
            if(block != null)
            {
                if(isBuildable)
                {
                    block.GetComponent<SpriteRenderer>().color = buildableArea;
                }
                    else
                {
                    block.GetComponent<SpriteRenderer>().color = unbuildableArea;
                }
            }
        }
    }
    public void ClearShowArea(Color normalColor)
    {
        foreach (GameObject block in foundBlocksShow)
        {
            if(block != null)
            {
                block.GetComponent<SpriteRenderer>().color = normalColor;  
            }
        }
    }
}
