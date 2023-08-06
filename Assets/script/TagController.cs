using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagController : MonoBehaviour
{
    public Text text;
    public Animator ani;
    public void AppearTag(string name)
    {
        text.text = name;
        ani.SetBool("isAppear",true);
    }
    public void DisappearTag()
    {
        ani.SetBool("isAppear",false);
    }
}
