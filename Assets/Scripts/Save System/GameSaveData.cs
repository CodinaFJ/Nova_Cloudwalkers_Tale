using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public List<World> worldsWithLevels = new List<World>();
    public List<World> worldsWithLevelsPrevious = new List<World>();

    public Level activeLevel;
    public World activeWorld;

    public GameSaveData ()
    {
        worldsWithLevels = GameProgressManager.instance.GetWorldsWithLevels();
        worldsWithLevelsPrevious = GameProgressManager.instance.GetWorldsWithLevelsPre();

        activeLevel = GameProgressManager.instance.GetActiveLevel();
        activeWorld = GameProgressManager.instance.GetActiveWorld();
    }

}
