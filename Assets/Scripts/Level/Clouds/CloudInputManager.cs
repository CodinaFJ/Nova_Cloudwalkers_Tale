using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloudInputManager : MonoBehaviour
{
    public static CloudInputManager instance;

    [SerializeField] float cloudMovementTime = 20f;
    [SerializeField] float resistanceWhileMoving = 0f;
    [SerializeField] float sensitivityToMove = 0f;
    [SerializeField] float scaleTweenTime = 0.5f;
    [SerializeField] float moveTweenCorrection = 0.5f;
    [SerializeField] float linealSpeedAdjustmentf = 0.02f;

    CloudAnimationManager cloudAnimationManager;
    [Header("Easing")]
    [SerializeField] bool easeCloudMovement = true;
    [SerializeField] bool easingSelector = true;

    LTDescr LTCloudMovement = null;

    //Things from other objects
    MatrixManager matrixManager;
    int[,] itemsLayoutMatrix;
    int[,] mechanicsLayoutMatrix;
    bool[,] pjMovementMatrix;
    bool[,] cloudMovementMatrix;

    int item;
    int mechanic;
    bool pjMovement;
    bool cloudMovement;

    PlayerBehavior playerBehavior;

    fromMatrixToGame fromMatrixToGame;
    GameObject[] cloudsParents;

    CloudSfxManager cloudSfxManager;
    float timeForWrongAction = 2f;

    float cellSize = 1f;
    float easingValue = 0;

    //Variables shared between methods
    int[] mouseMovements = new int[0];
    int[] unitaryMovement;
    int movementDoneIndex;
    int sizeMouseMovementsPreviousLoop = 0;

    Vector3 previousCellCenter;
    Vector3 mouseOffset;

    [HideInInspector]
    public bool doingMagic = false;
    bool cloudIsMoving = false;
    bool startCloudMovement = false;
    bool isSelecting = false;
    bool wrongActionPlayed = false;
    bool acceleratedCloud = false;

    bool cloudMoved = false;

    bool stopMovement = false;

    void Awake()
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
        matrixManager = FindObjectOfType<MatrixManager>();
        itemsLayoutMatrix = matrixManager.GetItemsLayoutMatrix();
        mechanicsLayoutMatrix = matrixManager.GetMechanicsLayoutMatrix();
        pjMovementMatrix = matrixManager.GetPjMovementMatrix();
        cloudMovementMatrix = matrixManager.GetCloudMovementMatrix();

        fromMatrixToGame = FindObjectOfType<fromMatrixToGame>();
        cloudsParents = fromMatrixToGame.GetCloudsParents();

        playerBehavior = FindObjectOfType<PlayerBehavior>();

        cloudSfxManager = FindObjectOfType<CloudSfxManager>();

        cloudAnimationManager = FindObjectOfType<CloudAnimationManager>();
    }


    void FixedUpdate()
    {
        //Start mouse movement recording algorithm only if selection is done and selected object is cloud
        if( isSelecting && (mechanic == -1 || mechanic == 1 || mechanic ==3))
        {
            FillMousePath();
        }

        //Start moving cloud if there is are movements done
        if(mouseMovements.Length > 0)
        {
            cloudsParents = fromMatrixToGame.GetCloudsParents();
            doingMagic = true;
            moveCloud();

            playerBehavior.clickIsForCloud = true;
        }
        else
        {
            doingMagic = false;
        }
    }


    void OnSelectCloud()
    {
        playerBehavior.clickIsForCloud = false;

        //Initial mouse and cloud values needed for cloud movement algorythm
        Vector3 onClickMouseWorldPos = MouseMatrixScript.GetMouseWorldPos();
        Vector3 onClickCellCenter = new Vector3(Mathf.FloorToInt(onClickMouseWorldPos.x), Mathf.FloorToInt(onClickMouseWorldPos.y), 0f) + new Vector3(0.5f, 0.5f, 0f);
        mouseOffset = onClickMouseWorldPos - onClickCellCenter;
        int[] onClickMatrixCoor = MouseMatrixScript.GetMouseMatrixIndex();

        //Update cloud parents in case it was changed due to crystals or grey clouds
        cloudsParents = fromMatrixToGame.GetCloudsParents();
        
        if(onClickMatrixCoor == null) return;

        if(matrixManager.InsideLevelMatrix(onClickMatrixCoor) && !cloudIsMoving)
        {
            //Needed for movement algorythm
            previousCellCenter = onClickCellCenter;

            itemsLayoutMatrix = matrixManager.GetItemsLayoutMatrix();
            mechanicsLayoutMatrix = matrixManager.GetMechanicsLayoutMatrix();
            pjMovementMatrix = matrixManager.GetPjMovementMatrix();
            cloudMovementMatrix = matrixManager.GetCloudMovementMatrix();
        
            //Retrieve value in all matrixes
            item = itemsLayoutMatrix[onClickMatrixCoor[0],onClickMatrixCoor[1]];
            mechanic = mechanicsLayoutMatrix[onClickMatrixCoor[0],onClickMatrixCoor[1]];
            pjMovement = pjMovementMatrix[onClickMatrixCoor[0],onClickMatrixCoor[1]];
            cloudMovement = cloudMovementMatrix[onClickMatrixCoor[0],onClickMatrixCoor[1]];

            Debug.Log("Item: " + item + ". Mechanic: " + mechanic + "\nCharacter movement: " + pjMovement + ". Cloud movement: " + cloudMovement);

            if((mechanic == -1 || mechanic == 1 || mechanic ==3) && !isSelecting)
            {
                
                if(cloudsParents[item - 1] != null)
                cloudsParents[item - 1].GetComponent<ParentCloudScript>().PlayClickParticles(onClickMouseWorldPos);

                int tapNumber = (int)UnityEngine.Random.Range(1,4);
                switch(tapNumber)
                {
                    case 1:
                    AudioManager.instance.PlaySound("CloudSwipe_Tap1");
                    break;

                    case 2:
                    AudioManager.instance.PlaySound("CloudSwipe_Tap2");
                    break;

                    case 3:
                    AudioManager.instance.PlaySound("CloudSwipe_Tap3");
                    break;

                    default:
                    AudioManager.instance.PlaySound("CloudSwipe_Tap1");
                    break;
                }
                AudioManager.instance.PlaySound("CloudSwipe_Loop");

                if(PlayerHand.instance != null) PlayerHand.instance.PutHandOut = true;

                //TweenCloudScaleOnSelect();
            }
            else if(mechanic == 2)
            {
                AudioManager.instance.PlaySound("WrongAction");
            }
            cloudMoved = false;

            isSelecting = true;

            //Debug.Log("Initial Cell Coor: ( " + onClickMatrixCoor[0] + ", " + onClickMatrixCoor[1] + ")");
        }
    }

    void FillMousePath()
    {
        Vector3 mouseWorldPos = MouseMatrixScript.GetMouseWorldPos();

        int movementToAdd = 0;

        int diffX;
        int diffY;
    
        diffX = (int)(((mouseWorldPos.x - mouseOffset.x) - previousCellCenter.x)/(cellSize - sensitivityToMove));
        diffY = (int)(((mouseWorldPos.y - mouseOffset.y) - previousCellCenter.y)/(cellSize - sensitivityToMove));

        //Adds as many movements as needed to the mouse movements array, so the cloud wil go to the pointer
        if (Mathf.Abs(diffX) > 0)
        {
            for (int absDiffX = Mathf.Abs(diffX) ; absDiffX > 0; absDiffX--)
            {
                movementToAdd = (int)Mathf.Sign(diffX);
                mouseMovements = matrixManager.AddNumberToArray(mouseMovements, movementToAdd);
                previousCellCenter.x = previousCellCenter.x + (movementToAdd*cellSize);
            }
        }
        if (Mathf.Abs(diffY) > 0)
        {
            for (int absDiffY = Mathf.Abs(diffY) ; absDiffY > 0; absDiffY--)
            {
                movementToAdd = 2 * (int)Mathf.Sign(diffY);
                mouseMovements = matrixManager.AddNumberToArray(mouseMovements, movementToAdd);
                previousCellCenter.y = previousCellCenter.y + ((movementToAdd/2)*cellSize);
            }
        }
        //If there are movements that contradict themselves (i.e. right and left) both are removed
        CompesateMouseMovements();
    }

    void CompesateMouseMovements()
    {
        if(mouseMovements.Length > 0)
        {
            int lastMovement = mouseMovements[mouseMovements.Length - 1];

            for (int i = 0; i < mouseMovements.Length; i++)
            {
                if (lastMovement == -mouseMovements[i])
                {
                    mouseMovements = matrixManager.RemoveIndexFromArray(mouseMovements, i);
                    mouseMovements = matrixManager.RemoveIndexFromArray(mouseMovements, mouseMovements.Length - 1);
                    break;
                }
            }
        }
    }

    IEnumerator MoveCloudOneStep()
    {
        if(LevelStateManager.instance.shortUndo) LevelStateManager.instance.SaveLevelState();
        else if(!cloudMoved) LevelStateManager.instance.SaveLevelState();

        cloudMoved = true;
        cloudIsMoving = true;
        stopMovement = false;
        float finishTime = cloudMovementTime;
        float currentTime = 0f;

        startCloudMovement = false;

        Vector3 cloudParentMovement = new Vector3(unitaryMovement[1], -unitaryMovement[0], 0);

        Vector3 cloudInitialPosition = cloudsParents[item-1].transform.position;
        Vector3 cloudFinalPosition = cloudInitialPosition + cloudParentMovement;

        int movementDone = mouseMovements[movementDoneIndex];

        if (mouseMovements.Length > movementDoneIndex) mouseMovements = matrixManager.RemoveIndexFromArray(mouseMovements, movementDoneIndex);

        //First item in matrix is 400X so we can identify a cloud that is moving towards a specific position
        matrixManager.StartCloudMovementInMatrix(unitaryMovement, item);

        if(!easeCloudMovement)
        {   
            easingValue = 0;
            

            LTDescr TweenCloudMovement =  LeanTween.value(0f, 1f, cloudMovementTime + cloudMovementTime*moveTweenCorrection).setEaseLinear().setOnUpdate(SetEasingValue);
        
            
            while (currentTime < cloudMovementTime)
            {
                currentTime += Time.deltaTime;

                cloudsParents[item-1].transform.position = cloudInitialPosition + ((cloudFinalPosition - cloudInitialPosition) * easingValue);

                if(stopMovement) yield break;
                
                yield return null;
            }
        }

        else if(easeCloudMovement)
        {

            float waitTime = cloudMovementTime;

            if(sizeMouseMovementsPreviousLoop == 0)
            {
                if(mouseMovements.Length == 0)
                {
                    //bouncy
                    LeanTween.cancel(cloudsParents[item - 1].gameObject);
                    LTDescr lean  = LeanTween.move(cloudsParents[item-1].gameObject, cloudFinalPosition, cloudMovementTime).setEaseInQuart();
                }
                else if(mouseMovements.Length > 0)
                {
                    //acc
                    LeanTween.cancel(cloudsParents[item - 1].gameObject);
                    LTDescr lean  = LeanTween.move(cloudsParents[item-1].gameObject, cloudFinalPosition, cloudMovementTime).setEaseInQuart();
                }
            }
            else if(sizeMouseMovementsPreviousLoop > 0)
            {
                if(mouseMovements.Length == 0)
                {
                    //frenar
                    LeanTween.cancel(cloudsParents[item - 1].gameObject);
                    LTDescr lean  =  LeanTween.move(cloudsParents[item-1].gameObject, cloudFinalPosition, cloudMovementTime).setEaseLinear();
                }
                else if(mouseMovements.Length > 0)
                {
                    //lineal
                    LeanTween.cancel(cloudsParents[item - 1].gameObject);
                    waitTime = cloudMovementTime - (linealSpeedAdjustmentf*cloudMovementTime);
                    LTDescr lean  = LeanTween.move(cloudsParents[item-1].gameObject, cloudFinalPosition, waitTime).setEaseLinear();
                }
            }

            
            yield return new WaitForSeconds(waitTime);
            sizeMouseMovementsPreviousLoop = mouseMovements.Length;

        }
        
        if(cloudsParents[item-1] != null)
        cloudsParents[item-1].transform.position = cloudFinalPosition;

        cloudIsMoving = false;
        
        //When item arrives it gets its original number back
        //matrixManager.StartCloudMovementInMatrix(unitaryMovement, item);

        matrixManager.FinishCloudMovementInMatrix(unitaryMovement, item);

        matrixManager.SearchGreyCloudContact(item);   

        if(!isSelecting) mouseMovements = new int[0];

        wrongActionPlayed = false;
    }


    void moveCloud()
    {
        if(!cloudIsMoving && playerBehavior.GetItemUnderPj() != item)
        {
            for(int index = 0; index < mouseMovements.Length ; index++)
            {
                if( Mathf.Abs(mouseMovements[index]) == 1 && CloudCanMove(mouseMovements[index]))
                {
                    unitaryMovement =new int[] {0, (int)Mathf.Sign(mouseMovements[index])};
                    Vector3 cloudParentMovement = new Vector3(unitaryMovement[1], -unitaryMovement[0], 0);

                    startCloudMovement = true;
                    movementDoneIndex = index;
                    break;
                }

                else if( Mathf.Abs(mouseMovements[index]) == 2 && CloudCanMove(mouseMovements[index]))
                {
                    unitaryMovement = new int[] {-(int)Mathf.Sign(mouseMovements[index]), 0};
                    Vector3 cloudParentMovement = new Vector3(unitaryMovement[1], -unitaryMovement[0], 0);

                    startCloudMovement = true;
                    movementDoneIndex = index;
                    break;
                }

                else if(!CloudCanMove(mouseMovements[index]) && !wrongActionPlayed && !cloudMoved)
                {
                    wrongActionPlayed = true;
                    AudioManager.instance.PlaySound("WrongAction");
                    StartCoroutine(RestartWrongActionSound());
                }
            }
        }
        else if(playerBehavior.GetItemUnderPj() == item && !wrongActionPlayed)
        {
            wrongActionPlayed = true;
            AudioManager.instance.PlaySound("WrongAction");
            StartCoroutine(RestartWrongActionSound());
        }
        if(startCloudMovement)
        {
            StartCoroutine(MoveCloudOneStep());
        }
    }

    IEnumerator RestartWrongActionSound()
    {
        float currentTime = 0;

        while (currentTime < timeForWrongAction)
        {
            currentTime += Time.deltaTime;

            if(!wrongActionPlayed)
            {
                yield break;
            }

            yield return null;
        }

        wrongActionPlayed = false;

        yield break;
    }

    public void OnReleaseLeftClick()
    {
        if((mechanic == -1 || mechanic == 1 || mechanic ==3) && isSelecting)
        {
            AudioManager.instance.PlaySound("CloudSwipe_Release");
            AudioManager.instance.Stop("CloudSwipe_Loop");
            if(PlayerHand.instance != null) PlayerHand.instance.PutHandOut = false;
        }
        wrongActionPlayed = false;
        isSelecting = false;
        if(!cloudIsMoving) mouseMovements = new int[0];
    }

    public void StopCloudMovement()
    {
        wrongActionPlayed = false;
        isSelecting = false;
        mouseMovements = new int[0];
        stopMovement = true;
    }

    public bool GetIsSelecting()
    {
        return isSelecting;
    }

    bool CloudCanMove(int nextMovement)
    {
        bool canMove = true;

        if(Mathf.Abs(nextMovement) == 1)
        {
            for (int i = 0; i < itemsLayoutMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < itemsLayoutMatrix.GetLength(1); j++)
                {
                    if(itemsLayoutMatrix[i,j] == item)
                    {
                        if(itemsLayoutMatrix[i, j + (int)(Mathf.Sign(nextMovement))] == item) continue;

                        canMove = cloudMovementMatrix[i, j  + (int)(Mathf.Sign(nextMovement))];
                        if(!canMove) return canMove;
                    }
                }
            }
        }
        else if(Mathf.Abs(nextMovement) == 2)
        {
            for (int i = 0; i < itemsLayoutMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < itemsLayoutMatrix.GetLength(1); j++)
                {
                    if(itemsLayoutMatrix[i,j] == item)
                    {
                        if(itemsLayoutMatrix[i - (int)(Mathf.Sign(nextMovement)), j] == item) continue;

                        canMove = cloudMovementMatrix[i - (int)(Mathf.Sign(nextMovement)), j];
                        if(!canMove) return canMove;
                    }
                }
            }
        }
        
        //if(!canMove) cloudSfxManager.PlayCloudCollision();
        return canMove;
    }


    float NormalizedEasedMovement (float x)
    {
        if(easingSelector)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return 1 + (c3 * Mathf.Pow(x - 1,3)) + (c1 * Mathf.Pow(x - 1, 2));
        }
        else
        {
            float c4 = (2 * Mathf.PI) / 3;

            if(x == 0) return 0;
            else if(x == 1) return 1;
            else
            {
                return Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1;
            }
        }
    }

    void TweenCloudScaleOnSelect()
    {
        foreach(InstantiatedCloudBehavior cloudTile in cloudsParents[item-1].GetComponentsInChildren<InstantiatedCloudBehavior>())
        {
            LeanTween.cancel(cloudTile.gameObject);
            LeanTween.scale(cloudTile.gameObject, new Vector3 (Mathf.Sign(cloudTile.transform.localScale.x) * 1f,Mathf.Sign(cloudTile.transform.localScale.y) *1f, 1), scaleTweenTime).setEaseOutExpo();
            //LeanTween.scale(cloudTile.gameObject, new Vector3 (Mathf.Sign(cloudTile.transform.localScale.x) * 1.05f,Mathf.Sign(cloudTile.transform.localScale.y) * 1.05f, 1), scaleTweenTime).setEaseOutElastic();
        }
        
    }

    void TweenCloudScaleOnRelease()
    {
        foreach(InstantiatedCloudBehavior cloudTile in cloudsParents[item-1].GetComponentsInChildren<InstantiatedCloudBehavior>())
        {
           //LeanTween.scale(cloudTile.gameObject, new Vector3 (Mathf.Sign(cloudTile.transform.localScale.x) * 1f,Mathf.Sign(cloudTile.transform.localScale.y) *1f, 1), scaleTweenTime).setEaseOutExpo();
           LeanTween.scale(cloudTile.gameObject, new Vector3 (Mathf.Sign(cloudTile.transform.localScale.x) * 1.05f,Mathf.Sign(cloudTile.transform.localScale.y) * 1.05f, 1), scaleTweenTime).setEasePunch(); 
        }
    }

    public void SetEasingValue(float value)
    {
        easingValue = value;
    }

    public void SetCloudMove(bool value) => cloudMoved = value;





}
