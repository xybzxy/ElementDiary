using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheatCodeSystem : MonoBehaviour
{
    public GameObject inputObject;
    public InputField inputField;
    public Text codeRepeat;
    public PlayerMove playerMove;
    public int cheatValue;
    public CheatCodes[] cheatCodes;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            playerMove.enabled = !playerMove.enabled;
            inputObject.SetActive(!inputObject.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            EndEdit();
            inputField.text = null;
        }
    }
    void EndEdit()
    {
        bool isFound = false;
        for (int i = 0; i < cheatCodes.Length; i++)
        {
            //Debug.Log(inputField.text.Contains(cheatCodes[i].cheatCode));
            if(inputField.text.Contains(cheatCodes[i].cheatCode))
            {
                isFound = true;
                if(inputField.text.Length > cheatCodes[i].cheatCode.Length)
                {
                    try
                    {
                        cheatValue = int.Parse(inputField.text.Substring(cheatCodes[i].cheatCode.Length));
                    }
                    catch{}
                }
                else
                {
                    cheatValue = 1;
                }
                cheatCodes[i].OnCheatCodeUsing.Invoke();
                codeRepeat.color = Color.green;
                codeRepeat.text = "The code is used correctly.";
                break;
            }
        }
        if(!isFound)
        {
            codeRepeat.color = Color.red;
            codeRepeat.text = "The string"+ "__" + inputField.text + "__" + "cannot be found.Please check your spelling or other things.";
        }
        isFound = false;
    }
}
[System.Serializable]
public class CheatCodes
{
    public string cheatCode;
    public UnityEvent OnCheatCodeUsing;
}
