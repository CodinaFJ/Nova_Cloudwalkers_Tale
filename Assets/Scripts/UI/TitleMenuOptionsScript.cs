using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleMenuOptionsScript : MonoBehaviour
{
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject audioOptions;
    [SerializeField] GameObject videoOptions;

    [SerializeField] Toggle FullscreenToggle;
    [SerializeField] TextMeshProUGUI resolutionOption;
    [SerializeField] Button resolutionRightButton;
    [SerializeField] Button resolutionLeftButton;

    Resolution[] resolutions;
    bool fullscreenBool;

    List<string> resolutionOptions = new List<string>();
    int currentResolutionIndex;

    private void Start() 
    {
        optionsMenu.SetActive(true); 
        audioOptions.SetActive(false); 
        videoOptions.SetActive(false); 

        FullscreenToggle.isOn = Screen.fullScreen;
        fullscreenBool = Screen.fullScreen;

        resolutions = Screen.resolutions;
        
        for (int i = 0; i < resolutions.Length ; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionOption.text = resolutionOptions[currentResolutionIndex];
        if(currentResolutionIndex <= 0) resolutionLeftButton.interactable = false;
        else resolutionLeftButton.interactable = true;

        if(currentResolutionIndex >= resolutionOptions.Count - 1) resolutionRightButton.interactable = false; 
        else resolutionRightButton.interactable = true;
    }

    public void ToAudioOptions()
    {
        optionsMenu.SetActive(false); 
        audioOptions.SetActive(true); 
        videoOptions.SetActive(false); 
    }

    public void ToVideoOptions()
    {
        optionsMenu.SetActive(false); 
        audioOptions.SetActive(false); 
        videoOptions.SetActive(true); 
    }

    public void ToOptions()
    {
        optionsMenu.SetActive(true); 
        audioOptions.SetActive(false); 
        videoOptions.SetActive(false); 
    }

    public void ToMainMenu()
    {
        optionsMenu.SetActive(false); 
        audioOptions.SetActive(false); 
        videoOptions.SetActive(false); 
        mainMenuCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void NextResolution()
    {
        currentResolutionIndex++;
        resolutionOption.text = resolutionOptions[currentResolutionIndex];

        if(currentResolutionIndex == 0) resolutionLeftButton.interactable = false;
        else if(!resolutionLeftButton.interactable) resolutionLeftButton.interactable = true;

        
        if(currentResolutionIndex == resolutionOptions.Count - 1) resolutionRightButton.interactable = false; 
        else if (!resolutionRightButton.interactable) resolutionRightButton.interactable = true;
        
    }

    public void PreviousResolution()
    {
        currentResolutionIndex--;
        resolutionOption.text = resolutionOptions[currentResolutionIndex];

        if(currentResolutionIndex <= 0) resolutionLeftButton.interactable = false;
        else if(!resolutionLeftButton.interactable) resolutionLeftButton.interactable = true;
        
        if(currentResolutionIndex >= resolutionOptions.Count - 1) resolutionRightButton.interactable = false; 
        else if (!resolutionRightButton.interactable) resolutionRightButton.interactable = true;
    }

    public void OkResolutionOptions()
    {
        Resolution resolution = resolutions[currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        ToOptions();
    }

}
