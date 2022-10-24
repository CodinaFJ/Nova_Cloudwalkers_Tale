using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelUIController : MonoBehaviour
{
    [SerializeField] GameObject OptionCanvas;
    
    LevelLoader levelLoader;

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        OptionCanvas.SetActive(false);
    }

    public void restartButton()
    {
        GameManager.instance.OnRestart();
    }
    
    public void exitButton()
    {
        OptionCanvas.SetActive(true);
        
        SFXManager.PlayOpenMenu();

        if(GameManager.instance != null) GameManager.instance.PauseGame();

        OptionCanvas.GetComponent<PlayerInput>().enabled = true;
        OptionCanvas.GetComponent<PlayerInput>().ActivateInput();
    }

    public void undoButton()
    {
        LevelStateManager.instance.OnUndo();
    }

}