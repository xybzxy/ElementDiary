using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Buildingmode : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    CameraController cam;
    public Transform point;
    public Transform player;
    public Building building;
    [Space]
    public Transform[] buildingTools;
    public Transform sign;
    public int toolNum;
    [Space]
    public GameObject[] partClasses;
    [Space]
    public Text costPrizeShow;
    bool canFinishBuilding;
    PlayerInfo playerInfo;
    void Start()
    {
        sign.position = buildingTools[toolNum].position;
        playerInfo = player.GetComponent<PlayerInfo>();
        cam = vcam.GetComponent<CameraController>();
    }
    void Update()
    {
        if(building != null)
            CostPrizeShow();
    }
    public void ChangeCamera()
    {
        if(point != player)
        {
            vcam.Follow = point;
            
        }
        else if(canFinishBuilding)
        {
            vcam.Follow = point;
        }
    }
    public void StartBuilding()
    {
        building.isBuilding = true;
    }
    public void Exit()
    {
        if(canFinishBuilding)
        {
            playerInfo.plin.money -= building.partPrise;
            building.partPrise = 0;
            building.isBuilding = false;
            this.gameObject.SetActive(false);
            point = player;
        }
    }
    public void GenerateNewPart(GameObject part)
    {
        if(building.newPart == null&&building.toolNum != 3)
        {
            GameObject newPart = Instantiate(part,building.partParent.position,building.partParent.rotation,building.partParent);
            building.newPart = newPart;
        }
    }
    public void DeleteNewPart()
    {
        building.isDeleting = true;
        Destroy(building.newPart);
        building.newPart = null;
        building.isDeleting = false;
    }
    public void ChooseTool(int num)
    {
        toolNum = num;
        sign.position = buildingTools[toolNum].position;
        building.toolNum = toolNum;
    }
    public void ChooseClass(int num)
    {
        for (int i = 0; i < partClasses.Length; i++)
        {
            partClasses[i].SetActive(false);
        }
        partClasses[num].SetActive(true);
        building.cameraY = -num;
    }
    public void SetMovement(float movement)
    {
        building.cameraUpMovement = movement;
    }
    void CostPrizeShow()
    {
        costPrizeShow.text = building.partPrise.ToString();
        if(building.partPrise > playerInfo.plin.money)
        {
            costPrizeShow.color = Color.red;
        }
        else
        {
            costPrizeShow.color = Color.black;
        }
        canFinishBuilding = !(building.partPrise > playerInfo.plin.money);
    }
}
