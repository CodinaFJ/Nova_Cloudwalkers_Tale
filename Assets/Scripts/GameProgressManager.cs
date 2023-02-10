using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance;
    
    [SerializeField]
    private List<World> worldsWithLevels = new List<World>();

    //TODO: these should not be serialized
    [Header("Game Progress (DBG)")]
    [SerializeField] private List<bool> playedCinematics;
    [SerializeField] private List<bool> unlockedWorlds;
    [Header("Active Level/World (DBG)")]
    [SerializeField] private Level activeLevel;
    [SerializeField] private World activeWorld;
    [Header("Global Info (DBG)")]
    [SerializeField] private int collectedStarsInLevel = 0;
    [SerializeField] private int collectedStarsInGame = 0;

    private int totalStarsInGame = 0;
    private bool endReached = false;
    private bool allStarsCollected = false;
    private int worldSelection = 0;

    /*Properties*/

    public int WorldSelection 
    {
        get=>worldSelection;
        set=> worldSelection = value;
    }
    
    private void Awake() 
    {
        EnsureSingleton();
        ParseActiveLevel();      
    }

    private void Start() {
        CalculateCollectedStarsInGame();
        CalculateTotalStarsInGame();
        try{FindObjectOfType<TotalStarsCounter>().UpdateCounter();}catch{}
    }

    /**************************************************************************************************
    Msc
    **************************************************************************************************/ 

    private void EnsureSingleton()
    {
        int numInstances = FindObjectsOfType<GameProgressManager>().Length;
        if(numInstances > 1)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /**************************************************************************************************
    Game State Management
    **************************************************************************************************/    

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

    /**************************************************************************************************
    Levels Management
    **************************************************************************************************/
    
    /// <summary>
    /// Set active level based on scene name.
    /// </summary>
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
            Debug.Log("Not a puzle level: " + e.Message);
        }
        catch(Exception e)
        {
            Debug.Log("Error reading level name: " + e.Message);
        }

        SetActiveLevel(level, world);
    }

    public void UnlockAllLevels(){
        foreach(World world in worldsWithLevels){
            foreach(Level level in world.GetLevelsList()){
                level.SetLevelUnlocked(true);
            }
        }
    }

    /// <summary>
    /// Check if all levels are completed. All stars may not be collected.
    /// </summary>
    /// <returns> True if there is no uncompleted level </returns>
    public bool AllLevelsCompleted()
    {
        return !worldsWithLevels.Exists(x => x.GetLevelsList().Exists(x => x.GetLevelCompleted() == false) == true);
    }

    /**************************************************************************************************
    Worlds Management
    **************************************************************************************************/

    /// <summary>
    /// Check if a world is unlocked
    /// </summary>
    /// <param name="n"> World number </param>
    /// <returns> True if world is unlocked </returns>
    public bool GetUnlockedWorld(int n)
    {
        return unlockedWorlds[n - 1];
    }

    /// <summary>
    /// Load with world as unlocked
    /// </summary>
    /// <param name="n"> World number </param>
    public void LoadWithUnlockWorld(int n)
    {
        MapContextController.Instance.SetLoadWithUnlockWorld(n);
    }

    /// <summary>
    /// Set world as unlocked
    /// </summary>
    /// <param name="n"> World number </param>
    public void SetUnlockWorld(int n)
    {
        unlockedWorlds[n - 1] = true;
        MapContextController.Instance.UpdateLocksState();
    }

    /**************************************************************************************************
    Stars Management
    **************************************************************************************************/

    public void UpdateStarsInGame()
    {
        CalculateCollectedStarsInGame();
        CalculateTotalStarsInGame();
    }

    public void IncreaseCollectedStarsInLevel() => collectedStarsInLevel++;

    /// <summary>
    /// Calculate collectedStarsInGame adding up all collected stars in each level of each world.
    /// </summary>
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

    /// <summary>
    /// Calculate totalStarsInGame adding up all stars in each level of each world.
    /// </summary>
    public void CalculateTotalStarsInGame(){
        totalStarsInGame = 0;
        foreach(World world in worldsWithLevels){
            foreach(Level level in world.GetLevelsList())
            {
                totalStarsInGame += level.GetNumberOfStars();
            }
        }
    }

    /**************************************************************************************************
    Setters
    **************************************************************************************************/

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

    /**************************************************************************************************
    Getters
    **************************************************************************************************/

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
}
