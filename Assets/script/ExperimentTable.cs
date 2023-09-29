using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentTable : MonoBehaviour
{
    public Transform manager;
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Chemical")
        {
            other.transform.parent = manager;
        }
    }
}
