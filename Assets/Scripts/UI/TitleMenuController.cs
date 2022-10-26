using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMenuController : MonoBehaviour
{
    [SerializeField] GameObject optionCanvas;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject quitMenu;
    [SerializeField] GameObject mainMenuNewGame;
    [SerializeField] GameObject mainMenuContinue;
    [SerializeField] GameObject credits;
    LevelLoader levelLoader;

    GameObject mainMenu;


    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        quitMenu.SetActive(false);

        string path = Path.Combine(Application.persistentDataPath, "game.bin");
        if (!File.Exists(path))
        {
            mainMenuNewGame.SetActive(true);
            mainMenuContinue.SetActive(false);

            mainMenu = mainMenuNewGame;
        } 
        else
        {
            mainMenuNewGame.SetActive(false);
            mainMenuContinue.SetActive(true);

            mainMenu = mainMenuContinue;
        }

        foreach ( Sound sound in AudioManager.instance.musics)
        {
            if(AudioManager.instance.IsPlaying(sound.name))StartCoroutine(AudioManager.instance.FadeOutMusic(sound.name));
        }

        if(!AudioManager.instance.IsPlaying("Main Theme")) StartCoroutine(AudioManager.instance.FadeInMusic("Main Theme"));
    }


    public void StartButton()
    {
        //StartCoroutine(AudioManager.instance.FadeOutMusic("Main Theme"));
        if(GameProgressManager.instance != null) Destroy(GameProgressManager.instance.gameObject);
        levelLoader.LoadLevel("Cinematic1");
    }

    public void ContinueButton()
    {
        GameProgressManager.instance.LoadGameState();
        ToMap();
    }

    public void ControlsButton()
    {
        levelLoader.LoadLevel("ControlsMenu");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ToMap()
    {
        
        levelLoader.LoadLevel("LevelSelectorMenu_IDD");
        if(FindObjectOfType<MusicSelectionManager>() != null) FindObjectOfType<MusicSelectionManager>().FadeOutLevelMusic();
    }

    public void ToOptions()
    {
        optionCanvas.SetActive(true);
        optionCanvas.GetComponent<OptionsMenuController>().ToPauseMenu();
        SFXManager.PlayOpenMenu();
        gameObject.SetActive(false);
    }

    public void ToMainMenu()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ToCredits()
    {
        gameObject.SetActive(false);
        credits.SetActive(true);
    }

}

