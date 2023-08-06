using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Cinemachine;

public class WeatherSystem : MonoBehaviour
{
    public Weather[] weathers;
    public Weather[] upcomingWeathers;
    public GameObject weatherEffect;
    public string weatherType;
    public float lastTime;
    public float strength;
    public float weatherGreyscale;
    public AudioSource soundEffectPlayer;
    public TimeSystem timeSystem;
    public CinemachineVirtualCamera vcam;
    public bool weatherable;
    public int frozenWeather;
    Transform follow;
    GameObject effectIsPlaying;
    Vector3 playerPos;
    Color mixedColor;
    public float time;
    float startTime;
    float endTime;
    CheatCodeSystem cheatCodeSystem;
    void Start()
    {
        //weatherable = true;
        AddRandomWeathers(3);
        follow = vcam.Follow;
        cheatCodeSystem = GameObject.FindWithTag("Canvas").GetComponent<CheatCodeSystem>();
    }
    void Update()
    {
        DuringWeather();
    }
    void AddRandomWeathers(int addWeatherCount)
    {
        AddEmptyElementToWeather(addWeatherCount);
        for (int i = 0; i < addWeatherCount; i++)
        {
            if(weatherable)
            {
                upcomingWeathers[upcomingWeathers.Length-i-1] = weathers[Random.Range(0,weathers.Length)];
            }
            else
            {
                upcomingWeathers[upcomingWeathers.Length-i-1] = weathers[frozenWeather]; 
            }
        }
    }
    void ApplyWeather()
    {
        Weather currentWeather = upcomingWeathers[0];
        lastTime = Random.Range(currentWeather.lastTimeMin,currentWeather.lastTimeMax);
        strength = Random.Range(1,11);
        weatherEffect = currentWeather.prefab;
        soundEffectPlayer.clip = currentWeather.soundEffect; 
        weatherType = currentWeather.weatherName;
        weatherGreyscale = currentWeather.colorGreyscale;
        startTime = timeSystem.time;
        endTime = startTime + lastTime;
        if(weatherType != "晴天")
        {
            mixedColor = timeSystem.lightColor;
            mixedColor.r *= weatherGreyscale*strength;
            mixedColor.g *= weatherGreyscale*strength;
            mixedColor.b *= weatherGreyscale*strength;
        }
        else
        {
            mixedColor = timeSystem.lightColor;
        }
        soundEffectPlayer.Play();
        if(effectIsPlaying != null)
            Destroy(effectIsPlaying);
        ApplyEffect(weatherEffect);
    }
    void DuringWeather()
    {
        time = timeSystem.time;
        if(time >= endTime)
        {
            DeleteFirstElementInWeather();
            AddRandomWeathers(1);
            ApplyWeather();
            //Debug.Log(endTime);
        }
        if(weatherType != "晴天")
        {
            if(time - startTime<=lastTime*0.1f)
            {
                timeSystem.mixedColor = Color.Lerp(timeSystem.lightColor,mixedColor,(time-startTime)/(lastTime*0.1f));
            }
            else if(endTime - time<=lastTime*0.1f)
            {
                timeSystem.mixedColor = Color.Lerp(timeSystem.lightColor,mixedColor,(endTime-time)/(lastTime*0.1f));
            }
            if(time - startTime<=lastTime*0.01f)
            {
                soundEffectPlayer.volume = Mathf.Lerp(0,1,(time-startTime)/(lastTime*0.01f));
            }
            else if(endTime - time<=lastTime*0.01f)
            {
                soundEffectPlayer.volume = Mathf.Lerp(0,1,(endTime-time)/(lastTime*0.01f));
            }
        }
        else
        {
            mixedColor = timeSystem.lightColor;
            timeSystem.mixedColor = mixedColor;
            soundEffectPlayer.volume = 1;
        }
        if(effectIsPlaying != null)
        {
            effectIsPlaying.transform.position = follow.position;
        }
    }
    void ApplyEffect(GameObject prefab)
    {
        float x = follow.position.x;
        float y = follow.position.y;
        Vector3 pos = new Vector3(x,y,0);
        effectIsPlaying = Instantiate(prefab,pos,transform.rotation,transform);
    }
    void AddEmptyElementToWeather(int addedElementCount)
    {
        Weather[] forNow = upcomingWeathers;
        upcomingWeathers = new Weather[forNow.Length+addedElementCount];
        for (int i = 0; i < forNow.Length; i++)
        {
            upcomingWeathers[i] = forNow[i];    
        }
    }
    void DeleteFirstElementInWeather()
    {
        Weather[] forNow = upcomingWeathers;
        upcomingWeathers = new Weather[forNow.Length-1];
        for (int i = 0; i < upcomingWeathers.Length; i++)
        {
            upcomingWeathers[i] = forNow[i+1];    
        }
    }
    public void FreezeWeather()
    {
        weatherable = false;
        frozenWeather = cheatCodeSystem.cheatValue;
        upcomingWeathers[1] = weathers[cheatCodeSystem.cheatValue];
        upcomingWeathers[2] = weathers[cheatCodeSystem.cheatValue];
        upcomingWeathers[3] = weathers[cheatCodeSystem.cheatValue];
    }
    public void ThawWeather()
    {
        weatherable = true;
    }                  
}
[System.Serializable]
public class Weather
{
    public string weatherName;
    public float lastTimeMin;
    public float lastTimeMax;
    public GameObject prefab;
    public float colorGreyscale;
    public AudioClip soundEffect;
}
