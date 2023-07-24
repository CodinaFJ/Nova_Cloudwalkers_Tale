using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TitleMenuController : MonoBehaviour
{
    [SerializeField] GameObject optionCanvas;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject credits;
    LevelLoader levelLoader;

    const string CINEMATIC_1 = "Cinematic1";
    const string LEVEL_SELECTOR = "LevelSelectorMenu";
    const string MAIN_MUSIC = "Main Theme";

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        mainMenu.SetActive(true);
        foreach (Sound sound in AudioManager.instance.musics)
        {
            if(AudioManager.instance.IsPlaying(sound.name))StartCoroutine(AudioManager.instance.FadeOutMusic(sound.name));
        }
        if (!AudioManager.instance.IsPlaying(MAIN_MUSIC)) StartCoroutine(AudioManager.instance.FadeInMusic(MAIN_MUSIC));
        MouseMatrixScript.ReleasePointer();
        GameProgressManager.instance.LoadGameState();
        GameProgressManager.instance.WorldSelection = 0;
    }

    public void StartNewGame()
    {
        if(GameProgressManager.instance != null) Destroy(GameProgressManager.instance.gameObject);
        MouseMatrixScript.BlockPointer();
        levelLoader.LoadLevel(CINEMATIC_1);
    }

    public void PlayButton()
    {
        if (SaveSystem.ExistsSavedGame())
            ToMap();
        else
            StartNewGame();
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ToMap()
    {
        MouseMatrixScript.BlockPointer();
        levelLoader.LoadLevel(LevelLoader.GetLevelContains(LEVEL_SELECTOR));
        if(FindObjectOfType<MusicSelectionManager>() != null) FindObjectOfType<MusicSelectionManager>().FadeOutLevelMusic();
    }

    public void ToOptions()
    {
        optionCanvas.SetActive(true);
        optionCanvas.GetComponent<OptionsMenuController>().ToPauseMenu();
        optionCanvas.GetComponent<PlayerInput>().enabled = true;
        optionCanvas.GetComponent<PlayerInput>().ActivateInput();
        SFXManager.PlayOpenMenu();
        //gameObject.SetActive(false);
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

