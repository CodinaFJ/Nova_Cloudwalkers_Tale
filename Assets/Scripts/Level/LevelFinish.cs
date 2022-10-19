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
        Debug.Log("Trigger enter");
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
                Debug.Log("Problem saving: " + ex.Message);
            }
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        gameManager.PjToExit();
        //yield return new WaitForSecondsRealtime(0.5f);
        AudioManager.instance.PlaySound("LevelExit");
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        if(GameProgressManager.instance.AllLevelsCompleted())
        {
            GameProgressManager.instance.CalculateCollectedStarsInGame();
            if(!GameProgressManager.instance.GetEndReached() || GameProgressManager.instance.GetCollectedStarsInGame() == GameProgressManager.instance.GetTotalStarsInGame())
            {
                gameManager.ToEndDemo();
                GameProgressManager.instance.SetEndReached(true);
            }
            else gameManager.ToMap();
        }
        else gameManager.ToMap();
        
    }
}
