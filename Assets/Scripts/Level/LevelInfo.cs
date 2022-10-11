using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelInfo : MonoBehaviour
{
    public static LevelInfo instance;

    public bool wallLevel = false;
    [HideInInspector]
    public int numberOfStars = 0;
    [HideInInspector]
    public int collectedStars = 0;
    int level;
    [SerializeField]
    int world;
    public bool levelCompleted = false;

    private void Awake()
    {
        int numGameObjects = FindObjectsOfType<LevelInfo>().Length;
        if(numGameObjects > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
        }  
    }

    private void Start() 
    {
        numberOfStars = GameObject.FindGameObjectsWithTag("Star").Length;
        GetLevelandWorldNumber();
    }

    void GetLevelandWorldNumber()
    {
        string LevelName = SceneManager.GetActiveScene().name;

        string[] levelNameParts = LevelName.Split(new char[] {'-'});

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

        
    }

    public int GetLevelNumber()
    {
        return level;
    }

    public int GetLevelWorldNumber()
    {
        return world;
    }
}
