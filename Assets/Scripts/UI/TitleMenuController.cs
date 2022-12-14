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
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject credits;
    LevelLoader levelLoader;

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        quitMenu.SetActive(false);
        mainMenu.SetActive(true);
        foreach (Sound sound in AudioManager.instance.musics)
        {
            if(AudioManager.instance.IsPlaying(sound.name))StartCoroutine(AudioManager.instance.FadeOutMusic(sound.name));
        }
        if (!AudioManager.instance.IsPlaying("Main Theme")) StartCoroutine(AudioManager.instance.FadeInMusic("Main Theme"));
        MouseMatrixScript.ReleasePointer();
    }

    public void StartNewGame()
    {
        if(GameProgressManager.instance != null) Destroy(GameProgressManager.instance.gameObject);
        MouseMatrixScript.BlockPointer();
        levelLoader.LoadLevel("Cinematic1");
    }

    public void PlayButton()
    {
        if (!(File.Exists(SaveSystem.FilePath)))
        {
            StartNewGame();
            return ;
        }
        GameProgressManager.instance.LoadGameState();
        ToMap();
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ToMap()
    {
        MouseMatrixScript.BlockPointer();
        levelLoader.LoadLevel(LevelLoader.GetLevelContains("LevelSelectorMenu"));
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

