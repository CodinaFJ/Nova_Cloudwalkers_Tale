using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelSelectorController : MonoBehaviour
{
    LevelLoader levelLoader;
    [SerializeField] GameObject OptionCanvas;

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();

        int numberOfMusics = 0;

        foreach ( Sound sound in AudioManager.instance.musics)
        {
            if(AudioManager.instance.IsPlaying(sound.name)) numberOfMusics ++;
        }

        if(numberOfMusics > 1)
        {
            foreach ( Sound sound in AudioManager.instance.musics)
            {
                if(AudioManager.instance.IsPlaying(sound.name)) StartCoroutine(AudioManager.instance.FadeOutMusic(sound.name));
            }

            StartCoroutine(AudioManager.instance.FadeInMusic("Main Theme"));
        }

        MouseMatrixScript.ReleasePointer();
    }

    private void Update() 
    {
        if(OptionCanvas.activeSelf)
        {
            GetComponent<PlayerInput>().enabled = false;
            GetComponent<PlayerInput>().DeactivateInput();

            OptionCanvas.GetComponent<PlayerInput>().enabled = true;
            OptionCanvas.GetComponent<PlayerInput>().ActivateInput();
        }
        else
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<PlayerInput>().ActivateInput();

            OptionCanvas.GetComponent<PlayerInput>().enabled = false;
            OptionCanvas.GetComponent<PlayerInput>().DeactivateInput();
        }
    }

    public void LoadLevel(string name)
    {
        SFXManager.PlayEnterLevel();
        MouseMatrixScript.BlockPointer();
        if(AudioManager.instance.IsPlaying("Main Theme")) StartCoroutine(AudioManager.instance.FadeOutMusic("Main Theme"));

        levelLoader.LoadLevel(name);
    }

    public void ExitButton()
    {
        levelLoader.LoadLevel("StartMenu");
    }

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
}