using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] GameObject Hotkeys;
    [SerializeField] GameObject OptionsBackground;
    [SerializeField] Toggle FullscreenToggle;
    [SerializeField] TextMeshProUGUI resolutionOption;
    [SerializeField] Button resolutionRightButton;
    [SerializeField] Button resolutionLeftButton;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] GameObject MapLevelsBackButton;
    GameObject levelUI;

    [SerializeField]
    List<OptionsSection> OptionsSectionsList = new List<OptionsSection>();

    GameManager gameManager;
    LevelLoader levelLoader;
    LevelSelectorController levelSelectorController;
    [HideInInspector]
    public PlayerInput pauseMenuInput;

    Resolution[] resolutions;
    FullScreenMode fullscreenMode;

    List<string> resolutionOptions = new List<string>();
    int currentResolutionIndex;

    OptionsSectionTag pauseMenuToGo = OptionsSectionTag.PauseMenu;

    void Start()
    {
        OptionsBackground.SetActive(true);
        gameManager = FindObjectOfType<GameManager>();
        levelLoader = FindObjectOfType<LevelLoader>();
        levelSelectorController = FindObjectOfType<LevelSelectorController>();
        pauseMenuInput = GetComponent<PlayerInput>();
        InitializeScreenResolutions();
    }

    public void ToOptions() => LoadOptionsSection(OptionsSectionTag.Options);
    public void ToAudioOptions() => LoadOptionsSection(OptionsSectionTag.OptionsAudio);
    public void ToVideoOptions() => LoadOptionsSection(OptionsSectionTag.OptionsVideo);
    public void ToClearProgress() => LoadOptionsSection(OptionsSectionTag.Quit);
    public void ToLanguageOptions() => LoadOptionsSection(OptionsSectionTag.Languages);

    public void ToPauseLevel()
    {
        levelUI = FindObjectOfType<UIController>().gameObject;
        if (levelUI)
            levelUI.SetActive(false);
        LoadOptionsSection(OptionsSectionTag.PauseLevel);
        OptionsBackground.SetActive(true);
        Hotkeys.SetActive(false);
        pauseMenuToGo = OptionsSectionTag.PauseLevel;
    }

    public void ToPauseMap()
    {
        if (MapContextController.Instance.GetMapState() != MapContextController.Instance.GetWorldsMapState())
            return ;
        levelUI = GameObject.FindGameObjectWithTag("OverlayUI");
        if (levelUI)
        {
            levelUI.SetActive(false);
        }
        LoadOptionsSection(OptionsSectionTag.PauseMap);
        Hotkeys.SetActive(false);
        OptionsBackground.SetActive(true);
        pauseMenuToGo = OptionsSectionTag.PauseMap;
    }

    public void ToPauseMenu()
    {
        levelUI = GameObject.FindGameObjectWithTag("OverlayUI");
        if (levelUI)
        {
            levelUI.SetActive(false);
        }
        LoadOptionsSection(OptionsSectionTag.PauseMenu);
        Hotkeys.SetActive(false);
        OptionsBackground.SetActive(true);
        pauseMenuToGo = OptionsSectionTag.PauseMenu;
    }

    public void ToQuitMenu()
    {
        LoadOptionsSection(OptionsSectionTag.Quit);
        OptionsBackground.SetActive(false);
    }

    public void BackToPause()
    {
        OptionsBackground.SetActive(true);
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
        
        if (levelUI)
        {
            levelUI.SetActive(true);
            if (MapLevelsBackButton != null)
                MapLevelsBackButton.SetActive(false);
        } 
        if (gameManager) gameManager.ResumeGame();
        else if (levelSelectorController) levelSelectorController.OptionsInput(false);
        SFXManager.PlayCloseMenu();
    }

    public void ToMap()
    {
        GameProgressManager.instance.SetCollectedStarsInLevel(0);
        SFXManager.PlaySelectUI_B();
        MouseMatrixScript.BlockPointer();
        if(gameManager != null) gameManager.ToMap();
    }

    public void ToMainMenu()
    {
        MouseMatrixScript.BlockPointer();
        GameProgressManager.instance.SaveGameState();
        levelLoader.LoadLevel(LevelLoader.GetLevelContains("StartMenu"));
        SFXManager.PlaySelectUI_B();
    }

    public void Quit()
    {
        SFXManager.PlaySelectUI_B();
        Application.Quit();
    }

    public void SetFullscreen (bool isFullscreen)
    {
        if (isFullscreen)
            fullscreenMode = FullScreenMode.ExclusiveFullScreen;
        else
            fullscreenMode = FullScreenMode.Windowed;
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
        ScreenVideoManager.instance.Fullscreen = fullscreenMode;
        ScreenVideoManager.instance.ActiveResolution = ScreenVideoManager.instance.Resolutions[currentResolutionIndex];
        ScreenVideoManager.instance.SetResolution();
        SaveSystem.SaveConfigData();

        BackToPause();
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

    public void OnClearProgressConfirm()
    {
        Destroy(GameProgressManager.instance?.gameObject);
        SaveSystem.DestroySavedData();
        SceneManager.LoadScene(LevelLoader.GetLevelContains("StartMenu"));
    }

    public void LoadOptionsSection(OptionsSectionTag optionsSectionTag){
        foreach(OptionsSection section in OptionsSectionsList.FindAll(x => x.tag != optionsSectionTag)){
            section.sectionGameObject.SetActive(false);
        }
        try{OptionsSectionsList.Find(x => x.tag == optionsSectionTag).sectionGameObject.SetActive(true);}
        catch{throw new NullReferenceException("Exception loading: " + optionsSectionTag.ToString());}
    }

    private void InitializeScreenResolutions()
    {
        fullscreenMode = ScreenVideoManager.instance.Fullscreen;
        if (fullscreenMode == FullScreenMode.ExclusiveFullScreen)
            fullscreenToggle.isOn = true;
        else
            fullscreenToggle.isOn = false;

        for (int i = 0; i < ScreenVideoManager.instance.Resolutions.Length ; i++)
        {
            string option = ScreenVideoManager.instance.Resolutions[i].width + "x" + ScreenVideoManager.instance.Resolutions[i].height;
            resolutionOptions.Add(option);
            if (ScreenVideoManager.instance.Resolutions[i].width == ScreenVideoManager.instance.ActiveResolution.width &&
                ScreenVideoManager.instance.Resolutions[i].height == ScreenVideoManager.instance.ActiveResolution.height)
                    currentResolutionIndex = i;
        }

        resolutionOption.text = resolutionOptions[currentResolutionIndex];
        if(currentResolutionIndex <= 0) resolutionLeftButton.interactable = false;
        else resolutionLeftButton.interactable = true;

        if(currentResolutionIndex >= resolutionOptions.Count - 1) resolutionRightButton.interactable = false; 
        else resolutionRightButton.interactable = true;
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
    Languages,
    Quit
}
