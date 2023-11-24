using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectInBag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public string objname;
    public string number;

    private Transform orginalParentTrans;
    private float orginalWidth, orginalHeight;


    public void OnBeginDrag(PointerEventData eventData)
    {
        orginalParentTrans = this.transform.parent;
        orginalWidth = GetComponent<RectTransform>().rect.width;
        orginalHeight = GetComponent<RectTransform>().rect.height;
        this.transform.position = eventData.position;
        //this.transform.parent = transform.parent.parent;
        this.transform.SetParent(transform.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<RectTransform>().sizeDelta = new Vector2(orginalWidth, orginalHeight);

    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
        GetComponent<RectTransform>().sizeDelta = new Vector2(orginalWidth, orginalHeight);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(orginalWidth, orginalHeight);
        if (eventData.pointerCurrentRaycast.gameObject.tag != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.tag == "BagCell")
            {
                this.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                this.transform.position = this.transform.parent.position;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else if (eventData.pointerCurrentRaycast.gameObject.tag == "BagObject")
            {
                this.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
                this.transform.position = this.transform.parent.position;
                eventData.pointerCurrentRaycast.gameObject.transform.parent = orginalParentTrans;
                eventData.pointerCurrentRaycast.gameObject.transform.position = orginalParentTrans.position;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                this.transform.SetParent(orginalParentTrans);
                this.transform.position = orginalParentTrans.position;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
        else
        {
            this.transform.SetParent(orginalParentTrans);
            this.transform.position = orginalParentTrans.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

    }
}
