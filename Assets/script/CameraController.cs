using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public float viewFieldScale;
    public GameObject[] cameras;
    void Update()
    {
        viewFieldScale = Mathf.Clamp(viewFieldScale,60f,120f);
        viewFieldScale -= Input.GetAxis("Mouse ScrollWheel")*10f;
        vcam.m_Lens.FieldOfView = viewFieldScale;
    }
    public void ChangeActiveCamera(string toChangeCameraName)//切换镜头
    {
        foreach (GameObject cam in cameras)
        {
            cam.active = false;
            if(cam.name == toChangeCameraName)
            {
                cam.active = true;
            }
        }
    }
}
