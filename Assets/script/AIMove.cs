using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    private Rigidbody2D Rb;
    private Transform trans;
    public Vector3 pos;
    public float speed;
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
    }
    void Update()
    {
        Move();
    }
    private void Move()
    {
        Rb.velocity = new Vector3(pos.x*speed,pos.y*speed,0);
       
        trans.localScale = new Vector3(float.IsNaN(-pos.x / Mathf.Abs(pos.x)) ? trans.localScale.x: -(pos.x / Mathf.Abs(pos.x)), trans.localScale.y,1f);
    }
}
