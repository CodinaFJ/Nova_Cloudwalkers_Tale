using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject OptionCanvas;
    [SerializeField] GameObject levelsUIGO;
    [SerializeField] GameObject worldsUIGO;

    public static UIController instance;
    
    LevelLoader levelLoader;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        OptionCanvas.SetActive(false);
        MouseMatrixScript.ReleasePointer();
    }

    public void restartButton()
    {
        GameManager.instance.OnRestart();
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

    public void ToWorlds(){
        worldsUIGO.GetComponent<Animator>().Play("UI_fadeIn");
        levelsUIGO.GetComponent<Animator>().Play("UI_fadeOut");
    }

    public void ToLevels(){
        worldsUIGO.GetComponent<Animator>().Play("UI_fadeOut");
        levelsUIGO.GetComponent<Animator>().Play("UI_fadeIn");
    }

}