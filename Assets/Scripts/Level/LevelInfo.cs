using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


/*public class LevelInfo : MonoBehaviour
{
    public static LevelInfo instance;
    private Level activeLevel = new Level();
    private World activeWorld;
    private int collectedStarsInLevel = 0;

    private void Awake() {
        int numGameState = FindObjectsOfType<LevelInfo>().Length;
        if(numGameState > 1)
            Destroy(gameObject);
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        activeLevel = GameProgressManager.instance.GetActivelevel();
        activeWorld = GameProgressManager.instance.GetActiveWorld();
    }

    //Getters
    public int GetLevelNumber() => activeLevel.GetLevelNumber();
    public int GetNumberOfStars() => activeLevel.GetNumberOfStars();
    public int GetCollectedStars() => activeLevel.GetCollectedStars();
    public int GetCollectedStarsInLevel() => collectedStarsInLevel;
    public int GetLevelWorldNumber() => activeWorld.GetLevelWorldNumber();
    public bool GetWallLevel() => activeLevel.GetWallLevel();

    //Setters
    public void SetCollectedStars(int collectedStars){
        activeLevel.SetCollectedStars(collectedStars);
        activeWorld.CalculateCollectedStars();
    }

    public void IncreaseCollectedStarsInLevel() => collectedStarsInLevel++;

    public void SetCollectedStarsWhenFinishedLevel(){
        for(int i = 0; i < collectedStarsInLevel; i++){
            SetCollectedStars(collectedStarsInLevel);
        }
    }
    
}*/

[Serializable]
public class Level
{
    [SerializeField]
    private int levelNumber;
    [SerializeField] 
    private int numberOfStars = 0;
    [SerializeField]
    private bool wallLevel = false;
    [SerializeField]
    private bool unlockedLevel = false;

    private int collectedStars = 0;
    private bool levelCompleted = false;

    //Constructors
    public Level() => new Level(99,false);
    public Level(int levelNumber) => new Level(levelNumber, false);
    public Level(int levelNumber, bool wallLevel){
        this.levelNumber = levelNumber;
        this.wallLevel = wallLevel;
    }
    public Level(int levelNumber, int numberOfStars){
        this.levelNumber = levelNumber;
        this.numberOfStars = numberOfStars;
    }

    //Getters
    public int GetLevelNumber() => levelNumber;
    public int GetNumberOfStars() => numberOfStars;
    public int GetCollectedStars() => collectedStars;
    public bool GetWallLevel() => wallLevel;
    public bool GetLevelCompleted() => levelCompleted;
    public bool GetLevelUnlocked() => unlockedLevel;

    //Setters
    public void SetCollectedStars(int collectedStars) => this.collectedStars = collectedStars;
    public void SetLevelCompleted() => SetLevelCompleted(true);
    public void SetLevelCompleted(bool value) => levelCompleted = value;
    public void SetLevelUnlocked(bool value) => unlockedLevel = value;
}

[Serializable]
public class World
{
    [SerializeField]
    private int worldNumber;
    [SerializeField]
    private List<Level> levelsList = new List<Level>();
    private int collectedStarsInWorld;
    private int totalStarsInWorld;

    //Getters
    public int GetLevelWorldNumber() => worldNumber;
    public List<Level> GetLevelsList() => levelsList;
    public Level GetLevel(int levelNumber) => levelsList.Find(x => x.GetLevelNumber() == levelNumber);
    public int GetCollectedStarsInWorld() => collectedStarsInWorld;
    public int GetTotalStarsInWorld() => totalStarsInWorld;

    public void InitializeTotalStarsInWorld(){
        totalStarsInWorld = 0;
        foreach(Level level in levelsList){
            totalStarsInWorld += level.GetNumberOfStars();
        }
    }

    public void CalculateCollectedStars(){
        collectedStarsInWorld = 0;
        foreach(Level level in levelsList){
            collectedStarsInWorld += level.GetCollectedStars();
        }
    }
}
