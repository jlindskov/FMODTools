using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class MediaPlayerExample : MonoBehaviour
{
    public FMODParameter volumeParameter; 
    public FMODBus MusicBus; 

    public Slider volumeSlider;

    [EventRef]
    public string music;

    public Bus bus; 

    private EventInstance musicInstance;


    private void Start()
    {
        volumeSlider.value = 1f; 
    }

    void Update()
    {
        FmodEvent.SetBusVolume(MusicBus.busName, volumeSlider.value);

    }
    
    
    public void Play()
    {
        if (!FmodEvent.IsPlaying(musicInstance))
        {
            musicInstance = FmodEvent.Play(music, gameObject.transform, null);
        }

       
    }
    
    public void Stop()
    {
        FmodEvent.Stop(musicInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    
    public void Resume()
    {
        FmodEvent.Resume(musicInstance);
    }
    
    public void Pause()
    {
        FmodEvent.Pause(musicInstance);
    }

    public float ConvertToDecibel(float value)
    {
        return Mathf.Log10(Mathf.Max(value, 0.0001f))*20f;
    }
}
