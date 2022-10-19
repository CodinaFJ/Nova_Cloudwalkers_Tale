using System;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance;

    [SerializeField]
    private List<World> worldsWithLevels = new List<World>();
    private List<World> worldsWithLevelsPrevious = new List<World>();

    private Level activeLevel;
    private World activeWorld;

    private int collectedStarsInLevel = 0;
    private int collectedStarsInGame = 0;

    private int totalStarsIngame = 0;

    private bool endReached = false;
    
    private void Awake() {
        int numInstances = FindObjectsOfType<GameProgressManager>().Length;
        if(numInstances > 1)
            Destroy(gameObject);
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }      
    }

    private void Start() {
        SetActiveLevel(1, 1);
        worldsWithLevelsPrevious = worldsWithLevels;

        CalculateCollectedStarsInGame();
        CalculateTotalStarsInGame();
    }

    public Level GetActiveLevel() => activeLevel;
    public World GetActiveWorld() => activeWorld;
    public List<World> GetWorldsWithLevels() => worldsWithLevels;
    public List<World> GetWorldsWithLevelsPre() => worldsWithLevelsPrevious;

    public Level GetLevel(int worldNumber ,int levelNumber) => worldsWithLevels.Find(x => x.GetLevelWorldNumber() == worldNumber).GetLevelsList().Find(x => x.GetLevelNumber() == levelNumber);
    public Level GetLevelPrevious(int worldNumber ,int levelNumber) => worldsWithLevelsPrevious.Find(x => x.GetLevelWorldNumber() == worldNumber).GetLevelsList().Find(x => x.GetLevelNumber() == levelNumber);
    public int GetCollectedStarsInLevel() => this.collectedStarsInLevel;
    public int GetCollectedStarsInGame() => this.collectedStarsInGame;
    public int GetTotalStarsInGame() => this.totalStarsIngame;
    public bool GetEndReached() => endReached;

    public void SetEndReached(bool value) => endReached = value;

    public void SetActiveLevel(int worldNumber, int levelNumber){
        try{
            activeWorld = worldsWithLevels.Find(x => x.GetLevelWorldNumber() == worldNumber);
            activeLevel = activeWorld.GetLevelsList().Find(x => x.GetLevelNumber() == levelNumber);
        }catch(Exception ex){
            Debug.LogWarning("Error setting active level: " + ex.Message);
            activeLevel = new Level();
            activeWorld = new World();
        }
    }
    public void SetCollectedStarsInLevel(int collectedStarsInLevel) => this.collectedStarsInLevel = collectedStarsInLevel;

    public void CopyStateToPreviousState() => worldsWithLevelsPrevious = worldsWithLevels;

    public void IncreaseCollectedStarsInLevel() => collectedStarsInLevel++;

    public void CalculateCollectedStarsInGame(){
        collectedStarsInGame = 0;
        foreach(World world in worldsWithLevels){
            foreach(Level level in world.GetLevelsList())
            {
                collectedStarsInGame += level.GetCollectedStars();
            }
        }
    }

    public void CalculateTotalStarsInGame(){
        collectedStarsInGame = 0;
        foreach(World world in worldsWithLevels){
            foreach(Level level in world.GetLevelsList())
            {
                collectedStarsInGame += level.GetNumberOfStars();
            }
        }
    }

    public bool AllLevelsCompleted() => !worldsWithLevels.Exists(x => x.GetLevelsList().Exists(x => x.GetLevelCompleted() == false) == true);

    public void SaveGameState()
    {
        SaveSystem.SaveGame();
    }

    public void LoadGameState()
    {
        GameSaveData data = SaveSystem.LoadGame();
        if (data != null){
            this.worldsWithLevels = data.worldsWithLevels;
            this.worldsWithLevelsPrevious = data.worldsWithLevelsPrevious;
            this.activeLevel = data.activeLevel;
            this.activeWorld = data.activeWorld;
        }
        CalculateCollectedStarsInGame();
    }


}