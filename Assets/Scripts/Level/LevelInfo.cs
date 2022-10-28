using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[Serializable]
public class Level
{
    [SerializeField]
    private int levelNumber;
    [SerializeField] 
    private int numberOfStars = 0;
    [SerializeField]
    private int collectedStars = 0;
    [SerializeField]
    private bool wallLevel = false;
    [SerializeField]
    private bool unlockedLevel = false;
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
    public Level(Level level){
        this.levelNumber = level.GetLevelNumber();
        this.numberOfStars = level.GetNumberOfStars();
        this.wallLevel = level.GetWallLevel();
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
