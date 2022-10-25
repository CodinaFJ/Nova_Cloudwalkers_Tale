using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] GameObject Hotkeys;
    [SerializeField] GameObject OptionsBackground;
    [SerializeField] Toggle FullscreenToggle;
    [SerializeField] TextMeshProUGUI resolutionOption;
    [SerializeField] Button resolutionRightButton;
    [SerializeField] Button resolutionLeftButton;

    [SerializeField]
    List<OptionsSection> OptionsSectionsList = new List<OptionsSection>();

    GameManager gameManager;
    LevelLoader levelLoader;
    [HideInInspector]
    public PlayerInput pauseMenuInput;

    Resolution[] resolutions;
    bool fullscreenBool;

    List<string> resolutionOptions = new List<string>();
    int currentResolutionIndex;

    OptionsSectionTag pauseMenuToGo = OptionsSectionTag.PauseMenu;

    void Start()
    {
        OptionsBackground.SetActive(true);

        gameManager = FindObjectOfType<GameManager>();
        levelLoader = FindObjectOfType<LevelLoader>();
        pauseMenuInput = GetComponent<PlayerInput>();
        pauseMenuInput.enabled = false;
        pauseMenuInput.DeactivateInput();

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

    public void ToOptions() => LoadOptionsSection(OptionsSectionTag.Options);
    public void ToAudioOptions() => LoadOptionsSection(OptionsSectionTag.OptionsAudio);
    public void ToVideoOptions() => LoadOptionsSection(OptionsSectionTag.OptionsVideo);
    public void ToPauseLevel(){
        LoadOptionsSection(OptionsSectionTag.PauseLevel);
        Hotkeys.SetActive(true);
    } 
    public void ToPauseMap(){
        LoadOptionsSection(OptionsSectionTag.PauseMap);
        Hotkeys.SetActive(false);
        pauseMenuToGo = OptionsSectionTag.PauseMap;
    } 
    public void ToPauseMenu(){
        LoadOptionsSection(OptionsSectionTag.Options);
        Hotkeys.SetActive(false);
        pauseMenuToGo = OptionsSectionTag.PauseMenu;
    } 
    public void ToQuitMenu(){
        LoadOptionsSection(OptionsSectionTag.PauseLevel);
        OptionsBackground.SetActive(false);
        pauseMenuToGo = OptionsSectionTag.PauseLevel;
    }

    public void BackToPause(){
        if(pauseMenuToGo == OptionsSectionTag.PauseMenu){
            OptionsSectionsList.Find(x => x.tag == OptionsSectionTag.PauseMenu).sectionGameObject.SetActive(true);
            ToGame();
        }
        LoadOptionsSection(pauseMenuToGo);
    }


    public void ToGame()
    {
        foreach(OptionsSection section in OptionsSectionsList){
            section.sectionGameObject.SetActive(false);
        }
        OptionsBackground.SetActive(true);
        gameObject.SetActive(false);

        pauseMenuInput.enabled = false;
        pauseMenuInput.DeactivateInput();
        
        if(gameManager != null) gameManager.ResumeGame();
        SFXManager.PlayCloseMenu();
    }

    public void ToMap()
    {
        SFXManager.PlaySelectUI_B();
        if(gameManager != null) gameManager.ToMap();
    }

    public void ToMainMenu()
    {
        levelLoader.LoadLevel("StartMenu");
        SFXManager.PlaySelectUI_B();
    }

    public void Quit()
    {
        SFXManager.PlaySelectUI_B();
        Application.Quit();
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
        SFXManager.PlaySelectUI_F();

        if(currentResolutionIndex == 0) resolutionLeftButton.interactable = false;
        else if(!resolutionLeftButton.interactable) resolutionLeftButton.interactable = true;

        
        if(currentResolutionIndex == resolutionOptions.Count - 1) resolutionRightButton.interactable = false; 
        else if (!resolutionRightButton.interactable) resolutionRightButton.interactable = true;
        
    }

    public void PreviousResolution()
    {
        currentResolutionIndex--;
        resolutionOption.text = resolutionOptions[currentResolutionIndex];
        SFXManager.PlaySelectUI_B();

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

    public void OnResumeGame()
    {
        ToGame();
    }

    public void OnRestart()
    {
        ToGame();
        GameManager.instance.OnRestart();
    }


    public void LoadOptionsSection(OptionsSectionTag optionsSectionTag){
        foreach(OptionsSection section in OptionsSectionsList.FindAll(x => x.tag != optionsSectionTag)){
            section.sectionGameObject.SetActive(false);
        }
        OptionsSectionsList.Find(x => x.tag == optionsSectionTag).sectionGameObject.SetActive(true);
    }
}

[System.Serializable]
public struct OptionsSection {
    public OptionsSectionTag tag;
    public GameObject sectionGameObject;
}

public enum OptionsSectionTag{
    PauseMenu, PauseMap, PauseLevel,
    Options, OptionsAudio, OptionsVideo,
    Credits,
    Quit
}
