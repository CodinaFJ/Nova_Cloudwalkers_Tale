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
    
    public void Pause()
    {
        

        
    }

    public void ToOptionsLevel(){
        SFXManager.PlayOpenMenu();
        if(GameManager.instance != null) GameManager.instance.PauseGame();
        OptionCanvas.SetActive(true);
        OptionCanvas.GetComponent<PlayerInput>().enabled = true;
        OptionCanvas.GetComponent<PlayerInput>().ActivateInput();
        OptionCanvas.GetComponent<OptionsMenuController>().ToPauseLevel();
        
    }
    public void ToOptionsMap(){
        OptionCanvas.SetActive(true);
        OptionCanvas.GetComponent<OptionsMenuController>().ToPauseMap();
        SFXManager.PlayOpenMenu();
    }

    public void undoButton()
    {
        LevelStateManager.instance.OnUndo();
    }

}