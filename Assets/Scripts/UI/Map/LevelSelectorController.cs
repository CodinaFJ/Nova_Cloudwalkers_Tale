using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSelectorController : MonoBehaviour
{
    LevelLoader levelLoader;
    PlayerInput mapPlayerInput;
    PlayerInput optionPlayerInput;
    bool ctrlPressed = false;
    [SerializeField] GameObject OptionCanvas;

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        mapPlayerInput = GetComponent<PlayerInput>();
        optionPlayerInput = OptionCanvas.GetComponent<PlayerInput>();
        //TODO: This musics management should be in a music manager class
        int numberOfMusics = 0;
        foreach ( Sound sound in AudioManager.instance.musics)
            if(AudioManager.instance.IsPlaying(sound.name)) numberOfMusics ++;
        if(numberOfMusics > 1)
        {
            foreach ( Sound sound in AudioManager.instance.musics)
                if(AudioManager.instance.IsPlaying(sound.name)) StartCoroutine(AudioManager.instance.FadeOutMusic(sound.name));
            StartCoroutine(AudioManager.instance.FadeInMusic("Main Theme"));
        }
        MouseMatrixScript.ReleasePointer();//! This is also in UI controller. Redundant?
    }

    private void Update() 
    {
        //TODO: This should not be in update
        if(OptionCanvas.activeSelf)
        {
            mapPlayerInput.enabled = false;
            mapPlayerInput.DeactivateInput();

            optionPlayerInput.enabled = true;
            optionPlayerInput.ActivateInput();
        }
        else
        {
            mapPlayerInput.enabled = true;
            mapPlayerInput.ActivateInput();

            optionPlayerInput.enabled = false;
            optionPlayerInput.DeactivateInput();
        }
    }

    /// <summary>
    /// Load level when level selected. Music fade out.
    /// </summary>
    /// <param name="name"> Name of level selected </param>
    public void LoadLevel(string name)
    {
        SFXManager.PlayEnterLevel();
        MouseMatrixScript.BlockPointer();
        if(AudioManager.instance.IsPlaying("Main Theme")) StartCoroutine(AudioManager.instance.FadeOutMusic("Main Theme"));
        levelLoader.LoadLevel(name);
    }

    /**************************************************************************************************
    UI ACTIONS
    **************************************************************************************************/

    //! I am not sure if we are still using this
    public void ExitButton()
    {
        levelLoader.LoadLevel("StartMenu");
    }

    /// <summary>
    /// Open option canvas with SFX
    /// </summary>
    //! I am also doing stuff with option canvas in UI controller WTF
    public void OnPause()
    {
        if(!OptionCanvas.activeSelf)
        {
            OptionCanvas.SetActive(true);
            OptionCanvas.GetComponent<OptionsMenuController>().ToPauseMap();
            SFXManager.PlayOpenMenu();
            //if(FindObjectOfType<GameManager>() != null) FindObjectOfType<GameManager>().PauseGame();
        }
    }

    /**************************************************************************************************
    DEV CHEATS
    **************************************************************************************************/

    public void OnCtrlPress() => ctrlPressed = true;
    public void OnCtrlRelease() => ctrlPressed = false;

    /// <summary>
    /// Cheat to unlock all levels
    /// </summary>
    public void OnUnlockAllLevels()
    {
        if(ctrlPressed)
        {
            GameProgressManager.instance.UnlockAllLevels();
            SceneManager.LoadScene(LevelLoader.GetLevelContains("LevelSelector"));
        }
    }
}