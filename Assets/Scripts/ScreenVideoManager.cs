using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenVideoManager : MonoBehaviour
{
    public static ScreenVideoManager instance;

    private Resolution activeResolution;
    public Resolution ActiveResolution 
    { 
        get => activeResolution; 
        set
        {
            activeResolution = value;
        } 
    }
    private FullScreenMode fullscreen;
    public FullScreenMode Fullscreen 
    { 
        get => fullscreen; 
        set
        {
            Debug.Log("New fullscreen mode: " + value);
            fullscreen = value;
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
        SetFullscreenMode(Screen.fullScreen);
        
        for (int i = 0; i < Resolutions.Length ; i++)
        {
            if (Resolutions[i].width == Screen.currentResolution.width && Resolutions[i].height == Screen.currentResolution.height)
            {
                ActiveResolution = Resolutions[i];
            }
        }
    }

    public void LoadResolutionConfig(ConfigurationSaveData data)
    {
        SetFullscreenMode(data.Fullscreen);
        activeResolution.width = data.ActiveResolution[0];
        activeResolution.height = data.ActiveResolution[1];
        Screen.SetResolution(activeResolution.width, activeResolution.height, fullscreen);
    }

    public void SetResolution()
    {
        Screen.SetResolution(activeResolution.width, activeResolution.height, fullscreen);
    }

    private void SetFullscreenMode(bool fullscreenBool)
    {
        if (fullscreenBool)
            fullscreen = FullScreenMode.ExclusiveFullScreen;
        else
            fullscreen = FullScreenMode.Windowed;
    }
}
