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
    int itemUnderPj;
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
        //In editor the PJ may not be perfectly placed
        SnapPjToGrid();

        //Get starting layout under PJ
        UpdateItemUnderPj();
    }

    private void OnDisable() {
        instance = null;
    }

    //Places PJ in the correct centered position within the cell it is
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

    //Updates the info about the layout under the PJ
    public void UpdateItemUnderPj()
    {
        if (MatrixManager.instance.GetItemsLayoutMatrix()[pjCell[0], pjCell[1]] > 4000)
        {
            // Item > 4000 means cloud is reaching the point. This way we block the cloud before a glitch can happen and PJ can be left on air.
            itemUnderPj = MatrixManager.instance.GetItemsLayoutMatrix()[pjCell[0], pjCell[1]] - 4000;
        }
        else 
        {
            itemUnderPj = MatrixManager.instance.GetItemsLayoutMatrix()[pjCell[0], pjCell[1]];
        }
        if(MatrixManager.instance.GetMechanicsLayoutMatrix()[pjCell[0], pjCell[1]] == 5 ||
           MatrixManager.instance.GetMechanicsLayoutMatrix()[pjCell[0], pjCell[1]] == 5)
            MatrixManager.instance.GetMechanicsLayoutMatrix()[pjCell[0], pjCell[1]]++;
    }

    public bool GetRunningState() => running;

    public int GetStarsCollected() => starsCollected;

    public int GetItemUnderPj() => itemUnderPj;

    public int GetMechanicUnderPj() => MatrixManager.instance.GetMechanicsLayoutMatrix()[pjCell[0], pjCell[1]];

    //On level finished we need the PJ to keep walking towards the exit
    public void ExitThroughDoor() => PjInputManager.instance.KeepMoving(lastMovement);

    //States saved while solving a puzzle. Ued for the UNDO button
    public void LoadLevelStatePlayer(int[] _pjCell, int _starsCollected)
    {     
        //Update pj position - cell & transform
        PjInputManager.instance.pjMoving = false;
        pjCell = (int[])_pjCell.Clone();
        transform.position = MatrixManager.instance.FromMatrixIndexToWorld(pjCell[0], pjCell[1]) + new Vector3(0, 0.65f, 0);
        UpdateItemUnderPj();

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
