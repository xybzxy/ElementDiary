using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D Rb;
    private Transform trans;
    private Vector3 pos;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        PMove();
    }

    private void PMove()
    {
        pos.x = Input.GetAxis("Horizontal");
        pos.y = Input.GetAxis("Vertical");
        Rb.velocity = new Vector3(pos.x*speed,pos.y*speed,0);
       
        trans.localScale = new Vector3(float.IsNaN(-pos.x / Mathf.Abs(pos.x)) ? trans.localScale.x: -(pos.x / Mathf.Abs(pos.x)), trans.localScale.y,1f);
    }
}
