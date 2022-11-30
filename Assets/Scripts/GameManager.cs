using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] LevelLoader levelLoader;

    public bool gamePaused = false;

    [SerializeField]
    PlayerInput playerInput;
    [HideInInspector]
    public PlayerBehavior playerBehavior;

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
        
        levelLoader.LoadLevel(LevelLoader.GetLevelContains("LevelSelectorMenu"));
    }

    public void PjToExit()
    {
        MouseMatrixScript.BlockPointer();
        PauseGame(true);
        SFXManager.instance.StopCloudSwipeLoop();
        FindObjectOfType<PlayerBehavior>().ExitThroughDoor();
    }

    public void ToEndDemo()
    {
        levelLoader.LoadLevel(LevelLoader.GetLevelContains("FinalDemo"));
        //FindObjectOfType<MusicSelectionManager>().FadeOutLevelMusic();
    }
    public void OnPause()
    {
        FindObjectOfType<LevelUIController>().ToOptionsLevel();
    }

}
