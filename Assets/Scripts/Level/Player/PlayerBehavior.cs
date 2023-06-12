using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public static PlayerBehavior instance;

    //Cell the player occupies
    public int[] pjCell = new int[2];

    //Layout under the player
    [SerializeField]
    List<int> itemsUnderPj;
    int mechanicUnderPj;

    //How many stars have you callected in this level?
    public int starsCollected = 0;

    //Used by PjInputManager and CloudInputManager to know if the clik action is going to be to move the PJ or to move a cloud - I know is not the best thing you've ever seen
    [HideInInspector]
    public bool clickIsForCloud = false;

    //State variables
    [HideInInspector]
    public bool running = false, runningUp, runningDown, runningLeft, runningRight;
    [HideInInspector]
    public bool sitting, sleeping;
    [HideInInspector]
    public int lastMovement = 1;
    
    private void Awake() 
    {
        instance = this; // static reference
    }
        
    void Start()
    {
        SnapPjToGrid();
        AddItemUnderPj();
    }

    private void OnDisable() {
        instance = null;
    }

    /// <summary>
    /// Places PJ in the correct centered position within the cell it is.
    /// </summary>
    void SnapPjToGrid()
    {
        //Offset displacements so the exact cell where the PJ is can be calculated
        Vector3 playerPosition = transform.position - new Vector3 (0, 0.5f, 0);
        playerPosition = new Vector3 (Mathf.FloorToInt(playerPosition.x), Mathf.FloorToInt(playerPosition.y), Mathf.FloorToInt(playerPosition.z)) + new Vector3(0.5f, 0.5f, 0f);
        pjCell = MatrixManager.instance.FromWorldToMatrixIndex(playerPosition);

        //Correction so PJ sprite looks centered
        playerPosition = playerPosition + new Vector3(0, 0.65f, 0);

        transform.position = playerPosition;
    }

    /// <summary>
    /// Updates the info about the layout under the PJ
    /// </summary>
    public void AddItemUnderPj()
    {
        int item = MatrixManager.instance.GetItemsLayoutMatrix()[pjCell[0], pjCell[1]];
        if (IsItemUnderPj(item))
            return;
        if (item > 4000)
        {
            // Item > 4000 means cloud is reaching the point. This way we block the cloud before a glitch can happen and PJ can be left on air.
            itemsUnderPj.Add(item - 4000);
        }
        else 
        {
            itemsUnderPj.Add(item);
        }
        if(MatrixManager.instance.GetMechanicsLayoutMatrix()[pjCell[0], pjCell[1]] == 5)
            MatrixManager.instance.GetMechanicsLayoutMatrix()[pjCell[0], pjCell[1]]++;
    }

    public void RemoveItemUnderPj(int item)
    {
        if (!IsItemUnderPj(item))
            return;    
        itemsUnderPj.Remove(item);
    }

    public bool IsItemUnderPj(int value)
    {
        foreach(var item in itemsUnderPj)
        {
            if (item == value)
                return true;
        }
        return false;
    }

    public bool GetRunningState() => running;

    public int GetStarsCollected() => starsCollected;

    //public int GetItemUnderPj() => itemUnderPj;

    public int GetMechanicUnderPj() => MatrixManager.instance.GetMechanicsLayoutMatrix()[pjCell[0], pjCell[1]];

    /// <summary>
    /// On level finished we need the PJ to keep walking towards the exit
    /// </summary>
    public void ExitThroughDoor(Direction exitDirection) => PjInputManager.instance.KeepMoving(exitDirection);

    /// <summary>
    /// States saved while solving a puzzle. Ued for the UNDO button
    /// </summary>
    /// <param name="_pjCell"></param>
    /// <param name="_starsCollected"></param>
    public void LoadLevelStatePlayer(int[] _pjCell, int _starsCollected)
    {     
        //Update pj position - cell & transform
        PjInputManager.instance.pjMoving = false;
        pjCell = (int[])_pjCell.Clone();
        transform.position = MatrixManager.instance.FromMatrixIndexToWorld(pjCell[0], pjCell[1]) + new Vector3(0, 0.65f, 0);
        itemsUnderPj.Clear();
        AddItemUnderPj();

        //Recover the total stars count in the game - This might not be needed anymore since it is calculateTotalStarsInGame is called in levelFinished.cs
        //GameProgressManager.instance. -= (starsCollected - _starsCollected);

        //Recover stars collected on saved state
        starsCollected = _starsCollected;
        GameProgressManager.instance.SetCollectedStarsInLevel(_starsCollected);// SetCollectedStars(_starsCollected);

        //Set idle state
        running = false;
        sitting = false;
        sleeping = false;
    }
}
