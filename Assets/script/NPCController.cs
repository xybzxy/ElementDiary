using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("基本数据")]
    public AIMove AIMove;
    public Vector3 outputMovement;
    public int mode;
    public float moveDelay;
    public Transform path;
    public Transform currentPathPoint;
    public int pointNum;
    public float avoidDis;
    public int avoidAccuracy;
    [Header("指令")]
    public bool follow;
    public bool lead; 
    Transform player;
    Vector3 moveVec;
    bool modeChangable = true;
    public Transform stopper;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        //Debug.Log(CalculateAngle(Mathf.Sqrt(3),2));
    }
    void Update()
    {
        AIMove.pos = outputMovement;
        currentPathPoint = path.GetChild(pointNum);
        SetRay();
        if(modeChangable)
        {
            MoveModeControl();
        }
        switch (mode)
        {
            case 0:
                MoveTo(player.position,2f);
                break;
            case 1:
                MoveTo(currentPathPoint.position,0f);
                if(CalculateDistance(transform.position,currentPathPoint.position) < 0.1f)
                {
                    ChangePathPoint();
                }
                break;
            case 2:
                Vector3 avoidPos = new Vector3 (transform.position.x*2 - player.position.x,transform.position.y*2 - player.position.y,0);
                MoveTo(avoidPos,0f);
                break;
            case 3:
                Lead();
                break;
            case 4:
                Wait();
                break;
        }
        //Debug.Log(mode);
    }
    float CalculateDistance(Vector3 pos1,Vector3 pos2)
    {
        float distance = (pos1 - pos2).magnitude;
        return distance;
    }
    void Wait()
    {
        if(CalculateDistance(stopper.position,transform.position) <= avoidDis*2)
        {
            modeChangable = false;
            outputMovement = new Vector3(0,0,0);
        }
        else
        {
            mode = 1;
            stopper = null;
            modeChangable = true;
        }
    }
    void Lead()
    {
        if(CalculateDistance(transform.position,player.position) <= 8f)
        {
            MoveTo(currentPathPoint.position,0f);
            if(CalculateDistance(transform.position,currentPathPoint.position) < 0.1f)
            {
                ChangePathPoint();
            }
        }
        else
        {
            outputMovement = new Vector3(0,0,0);
        }
    }
    void SetRay()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(),GetComponent<Collider2D>());
        RaycastHit2D hit = Physics2D.Raycast(transform.position,moveVec,3f);
        Debug.DrawLine(transform.position,hit.point,Color.red);
        if(hit.collider != null && hit.collider.transform != this.transform)
        {
            stopper = hit.collider.transform;
        }
    }
    void MoveTo(Vector3 pos,float limit)
    {
        float distance = CalculateDistance(pos,transform.position); 
        moveVec = new Vector3(CheckFloatValue(pos.x-transform.position.x),CheckFloatValue(pos.y-transform.position.y),0);
        if(distance > limit)
        {
            outputMovement = Vector3.Lerp(outputMovement,moveVec,moveDelay * Time.deltaTime);
        }
        else
        {
            outputMovement = new Vector3(0,0,0);
        }
    }
    void MoveModeControl()
    {
        float playerDis = CalculateDistance(transform.position,player.position);
        float nextPointDis = CalculateDistance(player.position,currentPathPoint.position);
        if(follow)
        {
            mode = 0;
        }
        else if(lead)
        {
            mode = 3;
        }
        else if(playerDis <= 2 && nextPointDis <= 0.5f)
        {
            //Debug.Log("躲避");
            mode = 2;
            ChangePathPoint();
        }
        else if(stopper != null && CalculateDistance(stopper.position,transform.position) <= avoidDis*2)
        {
            //Debug.Log("绕行");
            mode = 4;
        }
        else
        {
            mode = 1;
        }
    }
    void ChangePathPoint()//切换目标路径点
    {
        pointNum++;
        moveVec = new Vector3(0,0,0);
        if(pointNum > path.childCount-1)
        {
            pointNum = 0;
        }
    }
    int CheckFloatValue(float value)
    {
        if (value > 0)
        {
            return 1;
        }
        else if (value < 0)
        {
            return -1;
        }
        else
        {
            return 0; 
        }
    }
}
