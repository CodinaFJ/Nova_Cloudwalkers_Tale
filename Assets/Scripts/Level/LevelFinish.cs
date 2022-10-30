using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            try{
                Level level = GameProgressManager.instance.GetActiveLevel();
                Debug.Log("Finished Level: " + level.GetLevelNumber() + " In world: "+ GameProgressManager.instance.GetActiveWorld().GetLevelWorldNumber());

                level.SetLevelCompleted();
                if(level.GetCollectedStars() < GameProgressManager.instance.GetCollectedStarsInLevel()){
                    level.SetCollectedStars(GameProgressManager.instance.GetCollectedStarsInLevel());
                    GameProgressManager.instance.GetActiveWorld().CalculateCollectedStars();
                }

                GameProgressManager.instance.SaveGameState();
            }catch(Exception ex){
                Debug.LogWarning("Problem saving: " + ex.Message);
            }
            GameProgressManager.instance.SetCollectedStarsInLevel(0);
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        gameManager = GameManager.instance;
        GameManager.instance.PjToExit();
        AudioManager.instance.PlaySound("LevelExit");
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        GameProgressManager.instance.CalculateCollectedStarsInGame();

        
        if(GameProgressManager.instance.GetCollectedStarsInGame() >= GameProgressManager.instance.GetTotalStarsInGame() && !GameProgressManager.instance.GetAllStarsCollected()){
            GameProgressManager.instance.SetAllStarsCollected(true);
            GameProgressManager.instance.SetEndReached(true);
            GameManager.instance.ToEndDemo();
        }
        else if(GameProgressManager.instance.AllLevelsCompleted() && !GameProgressManager.instance.GetEndReached()){
            GameProgressManager.instance.SetEndReached(true);
            GameManager.instance.ToEndDemo();
        }
        else GameManager.instance.ToMap();
        
    }
}
