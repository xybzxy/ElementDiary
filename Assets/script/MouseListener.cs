using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseListener : MonoBehaviour
{
    public string thisName;
    public GameObject nameTag;
    public UnityEvent CallOnPedal;
    GameObject tag;
    void OnMouseEnter()//移植需改
    {
        tag = Instantiate(nameTag,transform.position,transform.rotation,nameTag.transform.parent);
        tag.GetComponent<TagController>().AppearTag(thisName);
    }
    void OnMouseExit()//移植需改
    {
        tag.GetComponent<TagController>().DisappearTag();
        Destroy(tag,0.5f);
    }
    void OnMouseDown()//移植需改
    {
        CallOnPedal.Invoke();
    }
}
