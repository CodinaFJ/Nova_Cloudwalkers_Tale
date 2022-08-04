using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] LevelLoader levelLoader;

    public bool gamePaused = false;

    PlayerInput playerInput;
    [HideInInspector]
    public PlayerBehavior playerBehavior;


    private void Start() 
    {
        playerInput = GetComponent<PlayerInput>();
        //levelLoader = FindObjectOfType<LevelLoader>();
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AudioManager.instance.PlaySound("ResetComplete");
    }

    public void PauseGame()
    {
        gamePaused = true;
        playerInput.enabled = false;
        playerInput.DeactivateInput();
    }

    public void ResumeGame()
    {
        gamePaused = false;
        playerInput.enabled = true;
        playerInput.ActivateInput();
    }

    public void ToMap()
    {
        levelLoader.LoadLevel("LevelSelectorMenu");
        //FindObjectOfType<MusicSelectionManager>().FadeOutLevelMusic();
    }

    public void PjToExit()
    {
        PauseGame();
        FindObjectOfType<PlayerBehavior>().ExitThroughDoor();
    }

    public void ToEndDemo()
    {
        levelLoader.LoadLevel("Finaldemo");
        FindObjectOfType<MusicSelectionManager>().FadeOutLevelMusic();
    }

}
