using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public float viewFieldScale;
    void Update()
    {
        viewFieldScale = Mathf.Clamp(viewFieldScale,60f,120f);
        viewFieldScale -= Input.GetAxis("Mouse ScrollWheel")*10f;
        vcam.m_Lens.FieldOfView = viewFieldScale;
    }
}
