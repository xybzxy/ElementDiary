using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    [Header("基本数据")]
    public CameraController camControl;
    public TimeSystem timeSystem;
    bool movePlayer;
    GameObject blackCurtain;
    Transform player;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        blackCurtain = GameObject.FindWithTag("BlackCurtain").transform.GetChild(0).gameObject;
    }
    public void MovePlayer(bool a)
    {
        movePlayer = a;
    }
    public void MoveToAim(Place place)
    {
        blackCurtain.active = true;
        player.GetComponent<PlayerMove>().enabled = movePlayer;
        IEnumerator enumerator = DelayedMoveToAim(place);
        Coroutine coroutine = StartCoroutine(enumerator);
    }
    IEnumerator DelayedMoveToAim(Place place)
    {
        yield return new WaitForSecondsRealtime(0.6f);
        camControl.ChangeActiveCamera(place.cam.gameObject.name);
        timeSystem.ChangeEnvironmentColor(place.isAffectedByGlobalLight);
        if(place.UI != null)
        {
            place.UI.active = true;
        }
        if(movePlayer)
        {
            player.transform.position = place.aim.position;
        }
        yield break;
    }
}
