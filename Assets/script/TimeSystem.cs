using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeSystem : MonoBehaviour
{
    [Header("时间设置")]
    public float time;
    public float timeLengthPerDay;
    public float timePerDay;
    public int dayCount;
    float addedTime;
    [Header("昼夜变化")]
    public UnityEngine.Rendering.Universal.Light2D light;
    public Color lightColor;
    public Color sunriseColor;
    public float sunrise;
    public float sunriseLength;
    public Color daytimeColor;
    public float daytime;
    public float daytimeLength;
    public Color sunsetColor;
    public float sunset;
    public float sunsetLength;
    public Color nightColor;
    public float night;
    public float nightLength;
    public Color mixedColor;
    CheatCodeSystem cheatCodeSystem;
    void Start()
    {
        cheatCodeSystem = GameObject.FindWithTag("Canvas").GetComponent<CheatCodeSystem>();
    }
    void Update()
    {
        TimeControl();
    }
    void TimeControl()
    {
        time = Time.time + addedTime;
        timePerDay = time - dayCount*timeLengthPerDay;
        if(timePerDay >= timeLengthPerDay)
        {
            timePerDay = 0;
            dayCount++;
        }
        ChangeEnvironmentColorByTime(nightColor,sunriseColor,sunrise,sunriseLength,daytime);
        ChangeEnvironmentColorByTime(sunriseColor,daytimeColor,daytime,daytimeLength,sunset);
        ChangeEnvironmentColorByTime(daytimeColor,sunsetColor,sunset,sunsetLength,night);
        ChangeEnvironmentColorByTime(sunsetColor,nightColor,night,nightLength,sunrise);
        light.color = mixedColor;
    }
    void ChangeEnvironmentColorByTime(Color startColor,Color targetColor, float startTime, float lastLength, float nextStartTime)
    {
        if(timePerDay>=startTime&&timePerDay<startTime+lastLength)
        {
            lightColor = Color.Lerp(startColor,targetColor,(timePerDay-startTime)/lastLength);
        }
        else if(timePerDay>=startTime&&timePerDay<nextStartTime)
        {
            lightColor = targetColor;
        }
    }
    public void SetTime()
    {
        time -= addedTime;
        addedTime = cheatCodeSystem.cheatValue - time;
    }
}
