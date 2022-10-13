using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUIController : MonoBehaviour
{
    [SerializeField] GameObject OptionCanvas;
    GameManager gameManager;
    
    LevelLoader levelLoader;

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        gameManager = FindObjectOfType<GameManager>();
        OptionCanvas.SetActive(false);
    }

    public void restartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void exitButton()
    {
        Debug.Log("ExitButton");
        OptionCanvas.SetActive(true);

        /*OptionsMenuController optionsMenuController = FindObjectOfType<OptionsMenuController>();

        optionsMenuController.pauseMenuInput.enabled = true;
        optionsMenuController.pauseMenuInput.ActivateInput();*/

        if(gameManager != null) gameManager.PauseGame();
    }

    public void undoButton()
    {
        LevelStateManager.instance.OnUndo();
    }

}