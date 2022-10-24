using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] GameObject AudioOptions;
    [SerializeField] GameObject VideoOptions;
    [SerializeField] GameObject QuitMenu;
    [SerializeField] GameObject OptionsBackground;
    [SerializeField] Toggle FullscreenToggle;
    [SerializeField] TextMeshProUGUI resolutionOption;
    [SerializeField] Button resolutionRightButton;
    [SerializeField] Button resolutionLeftButton;

    GameManager gameManager;
    LevelLoader levelLoader;
    [HideInInspector]
    public PlayerInput pauseMenuInput;

    Resolution[] resolutions;
    bool fullscreenBool;

    List<string> resolutionOptions = new List<string>();
    int currentResolutionIndex;

    void Start()
    {
        PauseMenu.SetActive(true);
        AudioOptions.SetActive(false);
        VideoOptions.SetActive(false);
        OptionsMenu.SetActive(false);
        QuitMenu.SetActive(false);
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

    public void ToOptions()
    {
        PauseMenu.SetActive(false);
        AudioOptions.SetActive(false);
        VideoOptions.SetActive(false);
        OptionsMenu.SetActive(true);
        QuitMenu.SetActive(false);
        OptionsBackground.SetActive(true);
        SFXManager.PlaySelectUI_F();
    }

    public void ToAudioOptions()
    {
        PauseMenu.SetActive(false);
        AudioOptions.SetActive(true);
        VideoOptions.SetActive(false);
        OptionsMenu.SetActive(false);
        QuitMenu.SetActive(false);
        OptionsBackground.SetActive(true);
        SFXManager.PlaySelectUI_F();
    }

    public void ToVideoOptions()
    {
        PauseMenu.SetActive(false);
        AudioOptions.SetActive(false);
        VideoOptions.SetActive(true);
        OptionsMenu.SetActive(false);
        QuitMenu.SetActive(false);
        OptionsBackground.SetActive(true);
        SFXManager.PlaySelectUI_F();
    }

    public void ToPauseMenu()
    {
        PauseMenu.SetActive(true);
        AudioOptions.SetActive(false);
        VideoOptions.SetActive(false);
        OptionsMenu.SetActive(false);
        QuitMenu.SetActive(false);
        OptionsBackground.SetActive(true);
        SFXManager.PlaySelectUI_B();
    }

    public void ToGame()
    {
        PauseMenu.SetActive(true);
        AudioOptions.SetActive(false);
        VideoOptions.SetActive(false);
        OptionsMenu.SetActive(false);
        QuitMenu.SetActive(false);
        OptionsBackground.SetActive(true);
        gameObject.SetActive(false);

        pauseMenuInput.enabled = false;
        pauseMenuInput.DeactivateInput();
        
        if(gameManager != null) gameManager.ResumeGame();
        SFXManager.PlayCloseMenu();
    }

    public void ToMap()
    {
        GameState.instance.lastLevel[0] = LevelInfo.instance.GetLevelWorldNumber();
        GameState.instance.lastLevel[1] = LevelInfo.instance.GetLevelNumber();
        SFXManager.PlaySelectUI_B();
        if(gameManager != null) gameManager.ToMap();
    }

    public void ToMainMenu()
    {
        levelLoader.LoadLevel("StartMenu");
        SFXManager.PlaySelectUI_B();
    }

    public void ToQuitMenu()
    {
        PauseMenu.SetActive(false);
        AudioOptions.SetActive(false);
        VideoOptions.SetActive(false);
        OptionsMenu.SetActive(false);
        QuitMenu.SetActive(true);
        OptionsBackground.SetActive(false);
        SFXManager.PlaySelectUI_F();
    }

    public void Quit()
    {
        SFXManager.PlaySelectUI_B();
        Application.Quit();
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        SFXManager.PlayHoverUI();
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

    
}
