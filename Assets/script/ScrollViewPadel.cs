using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewPadel : MonoBehaviour
{
    public Transform scorllPadel;
    public Vector3 startedPos;
    public int maxMovestep;
    float movement;
    public float movestep;
    int moveVector;
    public float totalMove;
    bool isMove;
    void Start()
    {
        startedPos = scorllPadel.position;
    }
    void Update()
    {
        if(isMove)
        {
            movement = Mathf.Lerp(movement,movestep,0.1f);
            if(movement < movestep - 1)
            {
                scorllPadel.position = new Vector3(startedPos.x + movement*moveVector,scorllPadel.position.y,0);
            } 
            else
            {
                isMove = false;
            }
        }
    }
    public void Move(int moveVec)
    {
        if(totalMove + movestep * moveVec <= 0 && totalMove + movestep * moveVec >= -maxMovestep*movestep)
        {
            startedPos = scorllPadel.position;
            movement = 0;
            isMove = true;
            moveVector = moveVec;
            totalMove += movestep*moveVector;
        }
    }
}
