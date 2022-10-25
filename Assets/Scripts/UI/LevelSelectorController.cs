using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelSelectorController : MonoBehaviour
{
    LevelLoader levelLoader;
    [SerializeField] GameObject optioncanvas;

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
    }

    private void Update() 
    {
        if(optioncanvas.activeSelf)
        {
            GetComponent<PlayerInput>().enabled = false;
            GetComponent<PlayerInput>().DeactivateInput();

            optioncanvas.GetComponent<PlayerInput>().enabled = true;
            optioncanvas.GetComponent<PlayerInput>().ActivateInput();
        }
        else
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<PlayerInput>().ActivateInput();

            optioncanvas.GetComponent<PlayerInput>().enabled = false;
            optioncanvas.GetComponent<PlayerInput>().DeactivateInput();
        }
    }

    public void LoadLevel(string name)
    {
        SFXManager.PlayEnterLevel();
        if(AudioManager.instance.IsPlaying("Main Theme")) StartCoroutine(AudioManager.instance.FadeOutMusic("Main Theme"));

        levelLoader.LoadLevel(name);
    }

    public void ExitButton()
    {
        levelLoader.LoadLevel("StartMenu");
    }

    public void OnPause()
    {
        if(!optioncanvas.activeSelf)
        {
            optioncanvas.SetActive(true);
            SFXManager.PlayOpenMenu();
            if(FindObjectOfType<GameManager>() != null) FindObjectOfType<GameManager>().PauseGame();
        }
        
    }
}