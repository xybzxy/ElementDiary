using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentEquipment : MonoBehaviour
{
    ExperimentManager manager;
    Rigidbody2D rg;
    void Start()
    {
        manager = this.transform.parent.GetComponent<ExperimentManager>();
        rg = this.GetComponent<Rigidbody2D>();
    }
    void OnMouseDown() 
    {
        if(manager.takingEquipment == null)
        {
            manager.takingEquipment = this;
            rg.freezeRotation = true;
            rg.isKinematic = true;
        }
        else
        {
            PutDown();
        }
    }
    void OnMouseOver()
    {
        if(this.TryGetComponent<Container>(out var container))
        {
            if(Input.GetButtonDown("Fire2"))
            {
                manager.infoShow.SetActive(true);
                manager.ShowContents(this.GetComponent<Container>());
            }
            if(manager.infoShow.activeSelf)
            {
                manager.ShowContainerInfo(this.GetComponent<Container>());
            }
        } 
    }
    void OnMouseExit() 
    {
        if(this.TryGetComponent<Container>(out var container))
        {
            manager.infoShow.SetActive(false);
        }
    }
    void PutDown()
    {
        manager.takingEquipment = null;
        manager.angleShow.gameObject.active = false;
        rg.freezeRotation = false;
        rg.isKinematic = false;
    }
}
