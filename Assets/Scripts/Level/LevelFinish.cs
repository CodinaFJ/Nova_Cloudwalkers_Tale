using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] Direction exitDirection;

    GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            try
            {
                Level level = GameProgressManager.instance.GetActiveLevel();
                //Debug.Log("Finished Level: " + level.GetLevelNumber() + " In world: "+ GameProgressManager.instance.GetActiveWorld().GetLevelWorldNumber());

                level.SetLevelCompleted();
                if(level.GetCollectedStars() < GameProgressManager.instance.GetCollectedStarsInLevel()){
                    level.SetCollectedStars(GameProgressManager.instance.GetCollectedStarsInLevel());
                    GameProgressManager.instance.GetActiveWorld().CalculateCollectedStars();
                }

                GameProgressManager.instance.SaveGameState();
            }
            catch(Exception ex)
            {
                Debug.LogWarning("Problem saving: " + ex.Message);
            }
            StartCoroutine(LoadNextLevel());
            GameProgressManager.instance.SetCollectedStarsInLevel(0);
        }
    }

    IEnumerator LoadNextLevel()
    {
        AudioManager.instance.StopAmbients();
        gameManager = GameManager.instance;
        GameManager.instance.PjToExit(exitDirection);
        AudioManager.instance.PlaySound("LevelExit");
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        GameProgressManager.instance.CalculateCollectedStarsInGame();
        
        if(GameProgressManager.instance.GetCollectedStarsInGame() >= GameProgressManager.instance.GetTotalStarsInGame() && !GameProgressManager.instance.GetAllStarsCollected()){
            GameProgressManager.instance.SetAllStarsCollected(true);
            if (!GameProgressManager.instance.GetEndReached())
            {
                GameProgressManager.instance.SetEndReached(true);
                GameManager.instance.ToEndGame();
            }
            else
            {
                GameProgressManager.instance.SetEndReached(true);
                GameManager.instance.ToEndGame100();
            }
        }
        else if(GameProgressManager.instance.GetCompletedLastLevel() && !GameProgressManager.instance.GetEndReached()){
            GameProgressManager.instance.SetEndReached(true);
            GameManager.instance.ToEndGame();
        }
        else GameManager.instance.ToMap();
        
    }
}

public enum Direction
{
    North = 2,
    West = -1,
    South = -2,
    East = 1
}
