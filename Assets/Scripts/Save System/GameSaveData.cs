using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public bool[] completedLevelsWorld1;
    public bool[] preCompletedLevelsWorld1;
    public int[] collectedStarsInLevelsWorld1;
    public int[] totalStarsInLevelsWorld1;

    public bool[] completedLevelsWorld2;
    public bool[] preCompletedLevelsWorld2;
    public int[] collectedStarsInLevelsWorld2;
    public int[] totalStarsInLevelsWorld2;

    public int[] lastLevel;

    public GameSaveData (GameState gameState)
    {
        completedLevelsWorld1 = new bool[gameState.completedLevelsWorld1.Count];
        preCompletedLevelsWorld1 = new bool[gameState.preCompletedLevelsWorld1.Count];
        collectedStarsInLevelsWorld1 = new int[gameState.completedLevelsWorld1.Count];
        totalStarsInLevelsWorld1 = new int[gameState.completedLevelsWorld1.Count];

        completedLevelsWorld2 = new bool[gameState.completedLevelsWorld2.Count];
        preCompletedLevelsWorld2 = new bool[gameState.preCompletedLevelsWorld2.Count];
        collectedStarsInLevelsWorld2 = new int[gameState.completedLevelsWorld2.Count];
        totalStarsInLevelsWorld2 = new int[gameState.completedLevelsWorld2.Count];

        lastLevel = new int[2];

        for (int i = 0; i < completedLevelsWorld1.Length; i++)
        {
            completedLevelsWorld1[i] = gameState.completedLevelsWorld1[i];
        }
        for (int i = 0; i < preCompletedLevelsWorld1.Length; i++)
        {
            preCompletedLevelsWorld1[i] = gameState.preCompletedLevelsWorld1[i];
        }
        for (int i = 0; i < collectedStarsInLevelsWorld1.Length; i++)
        {
            collectedStarsInLevelsWorld1[i] = gameState.collectedStarsInLevelsWorld1[i];
        }
        for (int i = 0; i < totalStarsInLevelsWorld1.Length; i++)
        {
            totalStarsInLevelsWorld1[i] = gameState.totalStarsInLevelsWorld1[i];
        }

        for (int i = 0; i < completedLevelsWorld2.Length; i++)
        {
            completedLevelsWorld2[i] = gameState.completedLevelsWorld2[i];
        }
        for (int i = 0; i < preCompletedLevelsWorld2.Length; i++)
        {
            preCompletedLevelsWorld2[i] = gameState.preCompletedLevelsWorld2[i];
        }
        for (int i = 0; i < collectedStarsInLevelsWorld2.Length; i++)
        {
            collectedStarsInLevelsWorld2[i] = gameState.collectedStarsInLevelsWorld2[i];
        }
        for (int i = 0; i < totalStarsInLevelsWorld2.Length; i++)
        {
            totalStarsInLevelsWorld2[i] = gameState.totalStarsInLevelsWorld2[i];
        }

        lastLevel[0] = gameState.lastLevel[0];
        lastLevel[1] = gameState.lastLevel[1];

    }

}
