using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    GameManager gameManager;
    LevelInfo levelInfo;
    GameState gameState;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelInfo = FindObjectOfType<LevelInfo>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger enter");
        if(other.tag == "Player")
        {
            if(levelInfo == null) return;
            int levelNumber = levelInfo.GetLevelNumber();
            int worldNumber = levelInfo.GetLevelWorldNumber();
            Debug.Log("Finished Level: " + levelNumber + " In world: "+ worldNumber);
            if(worldNumber == 1)
            {
                GameState.instance.completedLevelsWorld1[levelNumber - 1] = true;

                if(GameState.instance.collectedStarsInLevelsWorld1[levelNumber - 1] < FindObjectOfType<LevelInfo>().collectedStars)
                {
                    GameState.instance.collectedStarsInLevelsWorld1[levelNumber - 1] = FindObjectOfType<LevelInfo>().collectedStars;
                }
            }
            else if(worldNumber == 2)
            {
                GameState.instance.completedLevelsWorld2[levelNumber - 1] = true;

                if(GameState.instance.collectedStarsInLevelsWorld2[levelNumber - 1] < FindObjectOfType<LevelInfo>().collectedStars)
                {
                    GameState.instance.collectedStarsInLevelsWorld2[levelNumber - 1] = FindObjectOfType<LevelInfo>().collectedStars;
                }
            }

            GameState.instance.lastLevel[0] = worldNumber;
            GameState.instance.lastLevel[1] = levelNumber;

            GameState.instance.SaveGameState();
            gameManager.PjToExit();
            //StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        AudioManager.instance.PlaySound("LevelExit");
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        if(GameState.instance.AllLevelsCompleted())
        {
            GameState.instance.UpdateCollectedStars();
            if(!GameState.instance.endReached || GameState.instance.totalCollectedStars == GameState.instance.totalStars)
            {
                gameManager.ToEndDemo();
                GameState.instance.endReached = true;
            }
            else gameManager.ToMap();
        }
        else gameManager.ToMap();

        yield break;
        
    }
}
