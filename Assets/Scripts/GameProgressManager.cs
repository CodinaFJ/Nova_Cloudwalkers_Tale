using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance;

    [SerializeField]
    private List<World> worldsWithLevels = new List<World>();

    [SerializeField] //FOR DEBUG
    private Level activeLevel;
    [SerializeField] //FOR DEBUG
    private World activeWorld;

    [Header("Global Info")]
    [SerializeField]//FOR DEBUG
    private int collectedStarsInLevel = 0;
    [SerializeField]//FOR DEBUG
    private int collectedStarsInGame = 0;

    [SerializeField]
    private List<bool> playedCinematics;

    private int totalStarsInGame = 0;

    private bool endReached = false;
    private bool allStarsCollected = false;

    private int worldSelection = 0;
    public int WorldSelection { get=>worldSelection; set=> worldSelection = value;}
    
    private void Awake() {
        int numInstances = FindObjectsOfType<GameProgressManager>().Length;
        if(numInstances > 1)
            Destroy(gameObject);
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        ParseActiveLevel();      
    }

    private void Start() {
        //ParseActiveLevel();
        CalculateCollectedStarsInGame();
        CalculateTotalStarsInGame();
        try{FindObjectOfType<TotalStarsCounter>().UpdateCounter();}catch{}
    }

    public void UpdateStarsInGame(){
        CalculateCollectedStarsInGame();
        CalculateTotalStarsInGame();
    }

    private void ParseActiveLevel(){
        string LevelName = SceneManager.GetActiveScene().name;

        string[] levelNameParts = LevelName.Split(new char[] {'-'});

        int level = 1;
        int world = 1;

        try
        {
            level = int.Parse(levelNameParts[0]);
            world = int.Parse(levelNameParts[1]);
            Debug.Log("Loaded level: " + level.ToString() + " In world: " + world.ToString());
        }
        catch(FormatException e)
        {
            Debug.Log("Level Name is not correct. Exception message: " + e.Message);
        }
        catch(Exception e)
        {
            Debug.Log("Error reading level name: " + e.Message);
        }

        SetActiveLevel(1,1);
    }

    public Level GetActiveLevel() => activeLevel;
    public World GetActiveWorld() => activeWorld;
    public List<World> GetWorldsWithLevels() => worldsWithLevels;

    public Level GetLevel(int worldNumber ,int levelNumber) => worldsWithLevels.Find(x => x.GetLevelWorldNumber() == worldNumber).GetLevelsList().Find(x => x.GetLevelNumber() == levelNumber);
    public int GetCollectedStarsInLevel() => this.collectedStarsInLevel;
    public int GetCollectedStarsInGame() => this.collectedStarsInGame;
    public int GetTotalStarsInGame() => this.totalStarsInGame;
    public bool GetEndReached() => endReached;
    public bool GetAllStarsCollected() => allStarsCollected;
    public bool GetPlayedCinematic(int cinematic) => playedCinematics[cinematic - 1];

    public void SetEndReached(bool value) => endReached = value;
    public void SetAllStarsCollected(bool value) => allStarsCollected = value;

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
    public bool SetPlayedCinematic(int cinematic) => playedCinematics[cinematic - 1] = true;

    public void IncreaseCollectedStarsInLevel() => collectedStarsInLevel++;

    public void CalculateCollectedStarsInGame(){
        collectedStarsInGame = 0;
        foreach(World world in worldsWithLevels){
            foreach(Level level in world.GetLevelsList())
            {
                collectedStarsInGame += level.GetCollectedStars();
            }
        }
        Debug.Log("CollectedStars: " + collectedStarsInGame);
    }

    public void CalculateTotalStarsInGame(){
        totalStarsInGame = 0;
        foreach(World world in worldsWithLevels){
            foreach(Level level in world.GetLevelsList())
            {
                totalStarsInGame += level.GetNumberOfStars();
            }
        }
    }

    public void UnlockAllLevels(){
        foreach(World world in worldsWithLevels){
            foreach(Level level in world.GetLevelsList()){
                level.SetLevelUnlocked(true);
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
            this.worldsWithLevels = new List<World>(data.worldsWithLevels);
            this.activeLevel = data.activeLevel;
            this.activeWorld = data.activeWorld;
        }
        CalculateCollectedStarsInGame();
    }


}
