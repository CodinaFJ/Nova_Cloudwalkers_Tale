using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [HideInInspector]
    public static GameState instance;

    public List<bool> completedLevelsWorld1 = new List<bool>();
    public List<bool> completedLevelsWorld2 = new List<bool>();

    public List<bool> preCompletedLevelsWorld1 = new List<bool>();
    public List<bool> preCompletedLevelsWorld2 = new List<bool>();

    public List<int> collectedStarsInLevelsWorld1 = new List<int>();
    public List<int> totalStarsInLevelsWorld1 = new List<int>();

    public List<int> collectedStarsInLevelsWorld2 = new List<int>();
    public List<int> totalStarsInLevelsWorld2 = new List<int>();

    [HideInInspector]
    public int totalCollectedStars = 0;
    [HideInInspector]
    public int totalStars = 0;

    public bool endReached = false;


    [HideInInspector]
    public int[] lastLevel = new int[2];
   
    private void Awake() 
    {
        int numGameState = FindObjectsOfType<GameState>().Length;
        if(numGameState > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }      
    }

    private void Update()
    {
        
    }

    public void SaveGameState()
    {
        SaveSystem.SaveGame();
    }

    public void LoadGameState()
    {

        GameSaveData data = SaveSystem.LoadGame();

        if (data != null)
        {
            for (int i = 0; i < completedLevelsWorld1.Count; i++)
            {
                completedLevelsWorld1[i] = data.completedLevelsWorld1[i];
            }
            for (int i = 0; i < preCompletedLevelsWorld1.Count; i++)
            {
                preCompletedLevelsWorld1[i] = data.preCompletedLevelsWorld1[i];
            }
            for (int i = 0; i < collectedStarsInLevelsWorld1.Count; i++)
            {
                collectedStarsInLevelsWorld1[i] = data.collectedStarsInLevelsWorld1[i];
            }
            for (int i = 0; i < totalStarsInLevelsWorld1.Count; i++)
            {
                totalStarsInLevelsWorld1[i] = data.totalStarsInLevelsWorld1[i];
            }

            for (int i = 0; i < completedLevelsWorld2.Count; i++)
            {
                completedLevelsWorld2[i] = data.completedLevelsWorld2[i];
            }
            for (int i = 0; i < preCompletedLevelsWorld2.Count; i++)
            {
                preCompletedLevelsWorld2[i] = data.preCompletedLevelsWorld2[i];
            }
            for (int i = 0; i < collectedStarsInLevelsWorld2.Count; i++)
            {
                collectedStarsInLevelsWorld2[i] = data.collectedStarsInLevelsWorld2[i];
            }
            for (int i = 0; i < totalStarsInLevelsWorld2.Count; i++)
            {
                totalStarsInLevelsWorld2[i] = data.totalStarsInLevelsWorld2[i];
            }

            lastLevel[0] = data.lastLevel[0];
            lastLevel[1] = data.lastLevel[1];
        }
    }

    public bool AllLevelsCompleted()
    {
        bool allLevelsCompleted = true;

        for (int i = 0; i < completedLevelsWorld1.Count; i++)
        {
            if(!completedLevelsWorld1[i]) allLevelsCompleted = false;
        }
        for (int i = 0; i < completedLevelsWorld2.Count; i++)
        {
            if(!completedLevelsWorld2[i]) allLevelsCompleted = false;
        }

        return allLevelsCompleted;
    }

    public void UpdateCollectedStars()
    {
        totalCollectedStars = 0;
        totalStars = 0;

        
        for(int i = 0; i < collectedStarsInLevelsWorld1.Count; i++)
        {
            totalCollectedStars += collectedStarsInLevelsWorld1[i];
        }

        for(int i = 0; i < collectedStarsInLevelsWorld2.Count; i++)
        {
            totalCollectedStars += collectedStarsInLevelsWorld2[i];
        }

        for(int i = 0; i < totalStarsInLevelsWorld1.Count; i++)
        {
            totalStars += totalStarsInLevelsWorld1[i];
        }

        for(int i = 0; i < totalStarsInLevelsWorld2.Count; i++)
        {
            totalStars += totalStarsInLevelsWorld2[i];
        }
    }

}
