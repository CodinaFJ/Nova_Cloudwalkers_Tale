using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStateManager : MonoBehaviour
{
    [SerializeField] float undoHoldSpeed = 0.5f;
    PlayerBehavior playerBehavior;
    MatrixManager matrixManager;

    StarBehavior[] stars;

    List<LevelState> levelStateList = new List<LevelState>();
    LevelState specificLevelState = null;

    public bool shortUndo = true;

    public static LevelStateManager instance;

    bool undoHold = false;

    float timeElapsed = 0;

    void Start()
    {
        if(instance == null)
           instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        stars = FindObjectsOfType<StarBehavior>();
        matrixManager = FindObjectOfType<MatrixManager>();
        playerBehavior = FindObjectOfType<PlayerBehavior>(); 
        levelStateList.Clear();       
    }

    private void Update() 
    {
        if(undoHold) UndoHold();
    }


    public void ClearLevelStateList()
    {
        levelStateList.Clear();
    }

    public void SaveLevelState()
    {
        levelStateList.Add(new LevelState(matrixManager, playerBehavior, stars));
        //Debug.Log("State saved number: " + levelStateList.Count);
    }

    //Method to save into a volatile variable a spcific state
    public void CaptureSpecificLevelState()
    {
        specificLevelState = new LevelState(matrixManager, playerBehavior, stars);
    }

    //Method to actually save the Specific State captured in previous method and put it in the List of LevelStates
    public void SaveSpecificLevelState()
    {
        if(specificLevelState != null) levelStateList.Add(specificLevelState);
        specificLevelState = null;
    }

    public void LoadLevelState(int state)
    {
        //Load state of layout
        matrixManager.LoadLevelStateMatrixManager(levelStateList[state].itemsLayoutMatrix, levelStateList[state].mechanicsLayoutMatrix);

        //Load state of player
        playerBehavior.LoadLevelStatePlayer(levelStateList[state].pjCell, levelStateList[state].totalStarsCollected);

        //Load state of stars
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].LoadLevelStateStar(levelStateList[state].starsCollected[i]);
        }
    }

    public void OnUndo()
    {
        LoadToState(levelStateList.Count - 1);
        
        if(levelStateList.Count > 0) levelStateList.Remove(levelStateList[levelStateList.Count - 1]);
    }

    public void LevelRestart()
    {
        SaveLevelState();
        LoadToState(0);
    }

    private void LoadToState(int i)
    {
        if(levelStateList.Count > 0)
        {
            PjInputManager.instance.StopMovement();
            CloudInputManager.instance.StopCloudMovement();
            LoadLevelState(i);
            specificLevelState = null;
        }
    }

    public void OnUndoHold()
    {
        undoHold = true;
        //StartCoroutine(UndoWhileHold());
    }

    public void OnUndoRelease()
    {
        undoHold = false;
    }

    void UndoHold()
    {
        if(timeElapsed > undoHoldSpeed)
        {
            timeElapsed = 0;
            OnUndo();
        }
        timeElapsed += Time.deltaTime;
        
    }

    IEnumerator UndoWhileHold()
    {
        while(true)
        {
            OnUndo();

            if(!undoHold) yield break;
            yield return new WaitForSeconds(undoHoldSpeed);
        }

    }
}
