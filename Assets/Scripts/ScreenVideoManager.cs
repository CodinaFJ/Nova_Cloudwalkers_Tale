using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenVideoManager : MonoBehaviour
{
    public static ScreenVideoManager instance;

    private int resolutionIndex;
    public int ResolutionIndex 
    { 
        get => resolutionIndex; 
        set
        {
            resolutionIndex = value;
            Screen.SetResolution(Resolutions[resolutionIndex].width, Resolutions[resolutionIndex].height, Screen.fullScreen);
        } 
    }
    private bool fullscreen;
    public bool Fullscreen 
    { 
        get => fullscreen; 
        set
        {
            fullscreen = value;
            Screen.fullScreen = Fullscreen;
        } 
    }
    public Resolution[] Resolutions {get; private set;}

    private bool configLoaded = false;

    void Awake()
    {
        instance = this;
        SetScreenBasicOptions();
        InitializeScreenResolutions();
    }

    void SetScreenBasicOptions()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void InitializeScreenResolutions()
    {
        Resolutions = Screen.resolutions;
        Fullscreen = Screen.fullScreen;
        
        for (int i = 0; i < Resolutions.Length ; i++)
        {
            if (Resolutions[i].width == Screen.currentResolution.width && Resolutions[i].height == Screen.currentResolution.height)
            {
                ResolutionIndex = i;
            }
        }
    }

    public void LoadResolutionConfig(ConfigurationSaveData data)
    {
        Fullscreen = data.Fullscreen;
        ResolutionIndex = data.ResolutionIndex;
    }
}
