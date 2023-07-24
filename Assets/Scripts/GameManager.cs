using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] LevelLoader levelLoader;

    public bool gamePaused = false;

    [SerializeField]
    PlayerInput playerInput;
    [HideInInspector]
    public PlayerBehavior playerBehavior;

    const string END_CINEMATIC = "CinematicEnd";
    const string END_COMPLETE_GAME = "EndGame100";

    private void Awake() {
        if(instance == null)
           instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void OnRestart()
    {
        StartCoroutine(RestartLevelWithCrossfade());
    }

    IEnumerator RestartLevelWithCrossfade()
    {
        levelLoader.FadeOut();

        yield return new WaitForSeconds(1f);

        LevelStateManager.instance.LevelRestart();
        AudioManager.instance.PlaySound("ResetComplete");

        levelLoader.FadeIn();
    }

    public void PauseGame() => PauseGame(false);
    public void PauseGame(bool endLevel)
    {
        gamePaused = true;
        playerInput.enabled = false;
        if(!endLevel)
            EnhancedTouchSupport.Disable();
        playerInput.DeactivateInput();
    }

    public void ResumeGame()
    {
        InputSystemUIInputModule inputModule = FindObjectOfType<InputSystemUIInputModule>();
        inputModule.enabled = false;
        gamePaused = false;
        playerInput.enabled = true;
        EnhancedTouchSupport.Enable();
        playerInput.ActivateInput();
        inputModule.enabled = true;
    }

    public void ToMap()
    {
        GameProgressManager.instance.UpdateStarsInGame();
        GameProgressManager.instance.WorldSelection = GameProgressManager.instance.GetActiveWorld().GetLevelWorldNumber();
        levelLoader.LoadLevel(LevelLoader.GetLevelContains("LevelSelectorMenu"));
    }

    public void PjToExit(Direction exitDirection)
    {
        MouseMatrixScript.BlockPointer();
        PauseGame(true);
        SFXManager.instance.StopCloudSwipeLoop();
        FindObjectOfType<PlayerBehavior>().ExitThroughDoor(exitDirection);
    }

    public void ToEndGame()
    {
        levelLoader.LoadLevel(LevelLoader.GetLevelContains(END_CINEMATIC));
    }
    public void ToEndGame100()
    {
        levelLoader.LoadLevel(LevelLoader.GetLevelContains(END_COMPLETE_GAME));
    }
    public void OnPause()
    {
        FindObjectOfType<UIController>().ToOptionsLevel();
    }

}
