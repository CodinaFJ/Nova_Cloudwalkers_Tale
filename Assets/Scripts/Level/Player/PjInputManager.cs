using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PjInputManager : MonoBehaviour
{
    /*********************************************************************
    PjInputManager.cs

    Description:
        Read keyboard for PJ movement (WASD and arrows).
        Values from keyboard are translated into commands and stored in
        command movements arrays (pjMovementsPress and pjMovementsHold)
        Those commands execute movement coroutines which update the position
        of the Player object.
        Player states are defined and controlled for animations.

    Check also:

        PjAnimationManager.cs
        PlayerBehavior.cs

        MatrixManager.cs

    **********************************************************************/
    [SerializeField] float PjMovementTime;
    [SerializeField] float sittingTime;
    [SerializeField] float sleepingTime;
    [SerializeField] int movementsMemory = 2;
    [SerializeField] float releaseMouseTolerance = 1f;
    [SerializeField] bool wallLevel = false;
    
    public static PjInputManager instance;

    bool holdUp = false;
    bool holdDown = false;
    bool holdRight = false;
    bool holdLeft = false;

    PlayerBehavior playerBehavior;
    MatrixManager matrixManager;
    int[,] itemsLayoutMatrix;
    int[,] mechanicsLayoutMatrix;
    bool[,] pjMovementMatrix;

    int[] pjMovementsPress = new int[0];
    int[] pjMovementsHold  = new int[0];

    //Coroutine bools
    public bool pjMoving = false;
    bool pjIdle = false;

    PjAnimationManager pjAnimationManager;

    Vector3 onClickMouseWorldPos;

    private void Awake() 
    {
        if(instance == null)
           instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        //Initialize all matrixes
        matrixManager = MatrixManager.instance;
        itemsLayoutMatrix = matrixManager.GetItemsLayoutMatrix();
        mechanicsLayoutMatrix = matrixManager.GetMechanicsLayoutMatrix();
        pjMovementMatrix = matrixManager.GetPjMovementMatrix();

        playerBehavior = FindObjectOfType<PlayerBehavior>();
        pjAnimationManager = FindObjectOfType<PjAnimationManager>();

        playerBehavior.sitting = false;
        playerBehavior.sleeping = false;
    }

    void FixedUpdate()
    {
        IdlePlayer();
        MovePlayer();
    }

    void IdlePlayer()
    {
        if(!pjIdle)
        {
            pjIdle = true;
            StartCoroutine(WaitForSittingPJ());
            StartCoroutine(WaitForSleepingPJ());
        }
    }

    void MovePlayer()
    {
        if(pjMovementsPress == null) pjMovementsPress = new int[0];
        if(pjMovementsPress.Length > 0)
        {
            itemsLayoutMatrix = matrixManager.GetItemsLayoutMatrix();
            mechanicsLayoutMatrix = matrixManager.GetMechanicsLayoutMatrix();
            pjMovementMatrix = matrixManager.GetPjMovementMatrix();
            if(!pjMoving && CanMove(pjMovementsPress[0]))
            {
                playerBehavior.running = true;
                StartCoroutine(MovePjOneStep(true, pjMovementsPress[0]));
            }
            else if(!pjMoving)
            {
                playerBehavior.lastMovement = pjMovementsPress[0];
                pjMovementsPress = matrixManager.RemoveFirstNumberFromArray(pjMovementsPress);
            }
        }
        else if (pjMovementsHold.Length > 0)
        {
            if(!pjMoving && CanMove(pjMovementsHold[pjMovementsHold.Length - 1]))
            {
                playerBehavior.running = true;
                StartCoroutine(MovePjOneStep(false, pjMovementsHold[pjMovementsHold.Length-1]));
            }
            else if(!CanMove(pjMovementsHold[pjMovementsHold.Length - 1]) && !pjMoving)
            {
                playerBehavior.lastMovement = pjMovementsHold[pjMovementsHold.Length - 1];
                playerBehavior.running = false;
            }
        }
        else if(pjMovementsHold.Length == 0 && pjMovementsPress.Length == 0 && !pjMoving) 
        {
            playerBehavior.running = false;
        }
    }    

    IEnumerator MovePjOneStep(bool movementPress, int movementDone)// Movement done in the array
    {
        if(LevelStateManager.instance.shortUndo) LevelStateManager.instance.SaveLevelState();

        pjMoving = true;// Bool to control if coroutine is active
        UpdateMovementState(movementDone);
        
        //Variables for the position calculations
        Vector3 pjInitialPosition = playerBehavior.gameObject.transform.position;
        Vector3 pjMovementDirection = new Vector3 (0, 0, 0);

        //If we are starting movement from a crystal it must break
        matrixManager.CrackCrystalFloor();
        matrixManager.CrackCrystalCloud();

        //Translate movement value to a direction vector 
        if(Mathf.Abs(movementDone)==1)
        {
            pjMovementDirection = new Vector3 (Mathf.Sign(movementDone), 0, 0f);
            playerBehavior.pjCell[1] += (int)Mathf.Sign(movementDone);
        }
        else if(Mathf.Abs(movementDone)==2)
        {
            pjMovementDirection = new Vector3 (0, Mathf.Sign(movementDone), 0f);
            playerBehavior.pjCell[0] -= (int)Mathf.Sign(movementDone);
        }

        //Movement finished, command can be removed from array
        if(movementPress) pjMovementsPress = matrixManager.RemoveFirstNumberFromArray(pjMovementsPress);

        //Tile under PJ is updated BEFORE movement is started
        playerBehavior.UpdateItemUnderPj();

        Vector3 stepFinalPosition = pjInitialPosition + pjMovementDirection;
        float currentTime = 0;

        while (!Vector3.Equals(playerBehavior.gameObject.transform.position, stepFinalPosition) && pjMoving)
        {
            currentTime += Time.deltaTime;
            playerBehavior.gameObject.transform.position = Vector3.Lerp(pjInitialPosition, stepFinalPosition, Mathf.Clamp(currentTime / PjMovementTime, 0f, 1f));
            yield return null;
        }

        //If stepped on crystal, prepare it to break when leaving the tile
        matrixManager.CheckForCrystal();

        playerBehavior.lastMovement = movementDone;
      
        pjMoving = false;// Bool to control if coroutine is active

        if(pjMovementsPress.Length > 0 || pjMovementsHold.Length > 0)
        {
            MovePlayer();
        }
    }

    IEnumerator WaitForSittingPJ()
    {
        float currentTime = 0f;
        playerBehavior.sitting = false;
        while(currentTime < sittingTime)
        {
            currentTime += Time.deltaTime;
            if(!pjIdle)
            {
                yield break;
            }   
            yield return null;
        }
        playerBehavior.sitting = true;
    }

    IEnumerator WaitForSleepingPJ()
    {
        float currentTime = 0f;
        playerBehavior.sleeping = false;
        while(currentTime < sleepingTime)
        {
            currentTime += Time.deltaTime;
            if(!pjIdle)
            {
                yield break;
            }   
            yield return null;
        }
        playerBehavior.sleeping = true;
    }

    void UpdateMovementState(int movement)
    {
        playerBehavior.runningRight = false;
        playerBehavior.runningLeft = false;
        playerBehavior.runningDown = false;
        playerBehavior.runningUp = false;

        if(playerBehavior.running)
        {
            OnNoIdle();
            if(movement == 1) playerBehavior.runningRight = true;
            else if(movement == -1) playerBehavior.runningLeft = true;
            else if(movement == 2) playerBehavior.runningUp = true;
            else if(movement == -2) playerBehavior.runningDown = true;
        }
    }

    bool CanMove(int movement)
    {
        //pjMovementMatrix has TRUE is pj can move to target tile and FALSE if pj cannot move to target tile. 
        //canMove stores the value from said matrix in target tile
        bool canMove = false;

        if(Mathf.Abs(movement) == 1)
        {
            if(playerBehavior.pjCell[1] + (int)Mathf.Sign(movement) < 0 || playerBehavior.pjCell[1] + (int)Mathf.Sign(movement) > pjMovementMatrix.GetLength(1) - 1)
            {
                canMove = false;
                return canMove;
            }
            canMove = pjMovementMatrix[playerBehavior.pjCell[0], playerBehavior.pjCell[1] + (int)Mathf.Sign(movement)];
        } 
        else if(Mathf.Abs(movement) == 2)
        {
            if(playerBehavior.pjCell[0] - (int)Mathf.Sign(movement) < 0 || playerBehavior.pjCell[0] - (int)Mathf.Sign(movement) > pjMovementMatrix.GetLength(0))
            {
                canMove = false;
                return canMove;
            }
            canMove = pjMovementMatrix[playerBehavior.pjCell[0] - (int)Mathf.Sign(movement), playerBehavior.pjCell[1]];
        }

        return canMove;
    }

    int[] AddMovement(int[] pjMovements ,int movementToAdd)
    {
        if(pjMovements.Length < movementsMemory)
        {
            pjMovements = matrixManager.AddNumberToArray(pjMovements, movementToAdd);
        }
        return pjMovements;
    }

    public void KeepMoving(int lastMovement)
    {
        int movementToAdd;
        if(itemsLayoutMatrix[playerBehavior.pjCell[0], playerBehavior.pjCell[1] + 1] == 999)
        {
            movementToAdd = 1;
        }
        else
        {
            movementToAdd = 2;
        }

        pjMovementsPress = new int[10];
        for (int i = 0; i < 10; i++)
        {
            pjMovementsPress[i] = movementToAdd;
        }
    }

    public void StopMovement()
    {
        pjMovementsPress = new int[0];
        pjMovementsHold  = new int[0];

        pjMoving = false;
        pjIdle = true;
    }



    //WHOLE CODE TO ACCEPT WASD CONTROL
    /*void OnUp()
    {
        pjMovementsPress = AddMovement(pjMovementsPress ,2);
    }
    void OnDown()
    {
        pjMovementsPress = AddMovement(pjMovementsPress ,-2);
    }
    void OnLeft()
    {
        pjMovementsPress = AddMovement(pjMovementsPress ,-1);
    }
    void OnRight()
    {
        pjMovementsPress = AddMovement(pjMovementsPress ,1);
    }

    void OnHoldUp()
    {
        holdUp = true;
        pjMovementsHold = AddMovement(pjMovementsHold ,2);
    }
    void OnHoldDown()
    {
        holdDown = true;
        pjMovementsHold = AddMovement(pjMovementsHold ,-2);
    }
    void OnHoldLeft()
    {
        holdLeft = true;
        pjMovementsHold = AddMovement(pjMovementsHold ,-1);
    }
    void OnHoldRight()
    {
        holdRight = true;
        pjMovementsHold = AddMovement(pjMovementsHold ,1);
    }

    void OnReleaseUp()
    {
        if(holdUp)
        {
            pjMovementsHold = matrixManager.RemoveNumberFromArray(pjMovementsHold, 2);
        } 
        holdUp = false;
    }
    void OnReleaseDown()
    {
        if(holdDown)
        {
            pjMovementsHold = matrixManager.RemoveNumberFromArray(pjMovementsHold, -2);
        }
        holdDown = false;
    }
    void OnReleaseLeft()
    {
        if(holdLeft)
        {
            pjMovementsHold = matrixManager.RemoveNumberFromArray(pjMovementsHold, -1);
        }
        holdLeft = false;
    }
    void OnReleaseRight()
    {
        if(holdRight)
        {
            pjMovementsHold = matrixManager.RemoveNumberFromArray(pjMovementsHold, 1);
        }
        holdRight = false;
    }*/

    void OnNoIdle()
    {
        pjIdle = false;  
    }

    void OnFindPath()
    {
        //Initial mouse and cloud values needed for cloud movement algorythm
        Vector3 mouseWorldPos = GetMouseWorldPos();
        Vector3 mouseCellCenter = new Vector3(Mathf.FloorToInt(mouseWorldPos.x), Mathf.FloorToInt(mouseWorldPos.y), 0f) + new Vector3(0.5f, 0.5f, 0f);
        int[] onClickMatrixCoor = GetMouseMatrixIndex();
        if(onClickMatrixCoor == null) return;

        if(matrixManager.InsideLevelMatrix(onClickMatrixCoor) && matrixManager.GetPjMovementMatrix()[onClickMatrixCoor[0], onClickMatrixCoor[1]])
        {
            //if(!LevelStateManager.instance.shortUndo) LevelStateManager.instance.SaveLevelState();

            Pathfinding pathfinding = new Pathfinding();
            int[] pjMovementsArray = new int[0];

            if(playerBehavior.pjCell[0] == onClickMatrixCoor[0] && playerBehavior.pjCell[1] == onClickMatrixCoor[1]) return;
            pjMovementsArray = pathfinding.CalculatePathMovement(playerBehavior.pjCell[0], playerBehavior.pjCell[1], onClickMatrixCoor[0], onClickMatrixCoor[1]);
            
            if(pjMovementsArray != null)
            {
                pjMovementsPress = (int[])pjMovementsArray.Clone();
                if(!LevelStateManager.instance.shortUndo) LevelStateManager.instance.SaveLevelState();
            } 

            pjAnimationManager.PjClickAnimation(mouseCellCenter);

            if(FindObjectOfType<WASD_TutorialScript>() != null) FindObjectOfType<WASD_TutorialScript>().FadeOutMouse();
        }
    }


    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        //mousePos.z = Camera.main.farClipPlane * .5f;  I leave this here just in case I want to come back some day for 3D algorythm

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        worldPoint.z = 0;

        return worldPoint;
    }

    int[] GetMouseMatrixIndex()
    {
        int[] mouseMatrixIndex = new int[2];

        //Truncate position and add (0.5, 0.5, 0) to match cell center
        Vector3 mouseWorldPosTruncated = new Vector3(Mathf.FloorToInt(GetMouseWorldPos().x), Mathf.FloorToInt(GetMouseWorldPos().y), 0f);

        mouseMatrixIndex = matrixManager.FromWorldToMatrixIndex(mouseWorldPosTruncated + new Vector3(0.5f, 0.5f, 0f));
        if(mouseMatrixIndex == null) Debug.LogWarning("PjInputManager: Out of matrix");

        return mouseMatrixIndex;
    }

    public void OnPause()
    {
        FindObjectOfType<LevelUIController>().exitButton();
    }

    public void OnReleaseLeftClick()
    {
        if(Vector3.Magnitude(GetMouseWorldPos() - onClickMouseWorldPos) > releaseMouseTolerance && !LevelInfo.instance.wallLevel) return;

        if(!playerBehavior.clickIsForCloud)
        OnFindPath();
    }

    public void OnLeftClick()
    {
        playerBehavior.clickIsForCloud = false;

        onClickMouseWorldPos = GetMouseWorldPos();
    }

    
}
