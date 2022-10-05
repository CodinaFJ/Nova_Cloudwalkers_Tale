using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InstantiatedCloudBehavior : MonoBehaviour
{
    [SerializeField] GameObject tileShadow;

    [Header("Tile Sprites")]
    [SerializeField] Sprite tile0;
    [SerializeField] Sprite tile1;
    [SerializeField] Sprite tile2Corner;
    [SerializeField] Sprite tile2Parallel;
    [SerializeField] Sprite tile3;
    [SerializeField] Sprite tile4;

    [SerializeField] GameObject movementParticlesPrefab;
    [HideInInspector]
    public GameObject myMovementParticles;

    //Animator myAnimator;
    private string currentState;

    //Animations names
    const string CLOUD_0 = "Cloud_0_Idle";
    const string CLOUD_1LEFT = "Cloud_1Left_Idle";
    const string CLOUD_2PARALLEL = "Cloud_2Parallel_Idle";
    const string CLOUD_2CORNER = "Cloud_2Corner_Idle";
    const string CLOUD_3DOWN = "Cloud_3_idle";

    //1 White Cloud / 2 Grey Cloud / 3 Cloud Crystal /  5 Floor Crystal / -1 Thunder Cloud / 999 Floor / -999 Spiked Floor
    [SerializeField] int tileType;

    MatrixManager matrixManager;
    int[,] itemsLayoutMatrix;
    int[,] mechanicsLayoutMatrix;
    int[] matrixCoordinates;
    int objectNumber;
    int mechanicNumber;
    [SerializeField] float shadowToWorldCorrection = 0.85f;

    SpriteRenderer mySpriteRenderer;

    bool[] adyacentTilesForShadow = new bool[5];



    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        //Info about the layout and matrix of the level
        //matrixManager = FindObjectOfType<MatrixManager>();
        //itemsLayoutMatrix = matrixManager.GetItemsLayoutMatrix();
        //mechanicsLayoutMatrix = matrixManager.GetMechanicsLayoutMatrix();
        /*matrixCoordinates = MatrixManager.instance.FromWorldToMatrixIndex(transform.position);
        objectNumber = MatrixManager.instance.GetItemsLayoutMatrix()[matrixCoordinates[0], matrixCoordinates[1]];
        mechanicNumber = MatrixManager.instance.GetMechanicsLayoutMatrix()[matrixCoordinates[0], matrixCoordinates[1]];*/

        //myAnimator = GetComponent<Animator>();

        SelectCorrectSprite();

        myMovementParticles = Instantiate(movementParticlesPrefab, transform.position, Quaternion.identity);
        myMovementParticles.GetComponent<ParticleSystem>().Play();
    }

    public void SelectCorrectSprite()
    {
        // 0-up, 1-left, 2-down, 3-right
        bool[] adyacentTiles = new bool[4];
        matrixCoordinates = MatrixManager.instance.FromWorldToMatrixIndex(transform.position);
        objectNumber = MatrixManager.instance.GetItemsLayoutMatrix()[matrixCoordinates[0], matrixCoordinates[1]];
        mechanicNumber = MatrixManager.instance.GetMechanicsLayoutMatrix()[matrixCoordinates[0], matrixCoordinates[1]];

        for (int index = 0; index < adyacentTiles.Length; index ++)
        {
            adyacentTiles[index] = false;
        }

        int adyacentTilesNumber = 0;
        
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //Avoid out of the boundaries of the matrix
                bool indexInMatrixBoundaries = matrixCoordinates[0] + i > 0                              && matrixCoordinates[1] + j > 0 &&
                                               matrixCoordinates[0] + i < MatrixManager.instance.GetItemsLayoutMatrix().GetLength(0) && matrixCoordinates[1] + j < MatrixManager.instance.GetItemsLayoutMatrix().GetLength(1);

                //Mathf.Abs(i + j) == 1 used limit the checked tiles to the one at the right/left/up/down
                if (Mathf.Abs(i + j) == 1 && indexInMatrixBoundaries)
                {
                    /*if(mechanicNumber == 5)
                    {
                       if (mechanicsLayoutMatrix[matrixCoordinates[0] + i, matrixCoordinates[1] + j] == mechanicNumber)
                        {
                            if ( i == -1 && j ==  0) adyacentTiles[0] =  true;
                            if ( i ==  0 && j == -1) adyacentTiles[1] =  true;
                            if ( i ==  1 && j ==  0) adyacentTiles[2] =  true;
                            if ( i ==  0 && j ==  1) adyacentTiles[3] =  true;
                            
                        } 
                    }*/
                    if(tileType == 999)
                    {
                        //If this tile is floor and check if adyacent are floor or spiked floor (USELES FOR CLOUDS)
                        if (Mathf.Abs(MatrixManager.instance.GetItemsLayoutMatrix()[matrixCoordinates[0] + i, matrixCoordinates[1] + j]) == Mathf.Abs(objectNumber))
                        {
                            if ( i == -1 && j ==  0) adyacentTiles[0] =  true;
                            if ( i ==  0 && j == -1) adyacentTiles[1] =  true;
                            if ( i ==  1 && j ==  0) adyacentTiles[2] =  true;
                            if ( i ==  0 && j ==  1) adyacentTiles[3] =  true;
                        }
                    }

                    else if(tileType == -999)
                    {
                        //If this tile is spikes and check if adyacent spikes (USELES FOR CLOUDS)
                        if (MatrixManager.instance.GetMechanicsLayoutMatrix()[matrixCoordinates[0] + i, matrixCoordinates[1] + j] == mechanicNumber)
                        {
                            if ( i == -1 && j ==  0) adyacentTiles[0] =  true;
                            if ( i ==  0 && j == -1) adyacentTiles[1] =  true;
                            if ( i ==  1 && j ==  0) adyacentTiles[2] =  true;
                            if ( i ==  0 && j ==  1) adyacentTiles[3] =  true;
                            
                        }
                    }
                    else
                    {
                        //If this tile is cloud and check if adyacent are clouds
                        if (MatrixManager.instance.GetItemsLayoutMatrix()[matrixCoordinates[0] + i, matrixCoordinates[1] + j] == objectNumber /*|| itemsLayoutMatrix[matrixCoordinates[0] + i, matrixCoordinates[1] + j] == objectNumber + 4000*/)
                        {
                            if ( i == -1 && j ==  0) adyacentTiles[0] =  true;
                            if ( i ==  0 && j == -1) adyacentTiles[1] =  true;
                            if ( i ==  1 && j ==  0) adyacentTiles[2] =  true;
                            if ( i ==  0 && j ==  1) adyacentTiles[3] =  true;
                        }
                    }
                }

            }
        }

        for (int index = 0; index < adyacentTiles.Length; index ++)
        {
            if(adyacentTiles[index])adyacentTilesNumber++;
        }

        //Initialize to default values
        mySpriteRenderer.flipX = false;
        mySpriteRenderer.flipY = false;
        transform.Rotate(0,0,0,Space.Self);
        transform.localScale = new Vector3 (1, 1, 1);


        //Use adyacentTilesNumber to do a first filter to select the sprite based on how many other cloud tiles are connected to this one.
        //Then sprite is rotated or fliped to match the postion
        if(adyacentTilesNumber==0)
        {
            mySpriteRenderer.sprite = tile0;
            ChangeAnimationState(CLOUD_0);
        } 

        else if(adyacentTilesNumber==1) 
        {
            mySpriteRenderer.sprite = tile1;
            ChangeAnimationState(CLOUD_1LEFT);

            if(adyacentTiles[0])
            {
               transform.Rotate(0,0,-90,Space.Self);
               if(tileType == -1) transform.Rotate(0,0,-90,Space.Self);
               //mySpriteRenderer.flipY = true;
               transform.localScale = new Vector3 (1, 1, 1);
            }
            else if(adyacentTiles[2])
            {
                transform.Rotate(0,0,90,Space.Self);
                if(tileType == -1) transform.Rotate(0,0,-90,Space.Self);
                transform.localScale = new Vector3 (1, 1, 1);
            }
            else if(adyacentTiles[1])
            {
                //mySpriteRenderer.flipX = true;
                if(tileType == -1) transform.Rotate(0,0,-90,Space.Self);
                transform.localScale = new Vector3 (1, 1, 1);
            }
            else if(adyacentTiles[3])
            {
                //mySpriteRenderer.flipX = true;
                if(tileType == -1) transform.Rotate(0,0,90,Space.Self);
                transform.Rotate(0,180,0,Space.Self);
            }

        }

        else if(adyacentTilesNumber==2)
        {
            if(adyacentTiles[0] && adyacentTiles[2])
            {
                mySpriteRenderer.sprite = tile2Parallel;
                ChangeAnimationState(CLOUD_2PARALLEL);
                transform.Rotate(0,0,90,Space.Self);
            }
            else if(adyacentTiles[1] && adyacentTiles[3])
            {
                mySpriteRenderer.sprite = tile2Parallel;
                ChangeAnimationState(CLOUD_2PARALLEL);
            }
            else
            {
                mySpriteRenderer.sprite = tile2Corner;
                ChangeAnimationState(CLOUD_2CORNER);

                if     (adyacentTiles[0] && adyacentTiles[1]) transform.Rotate(0,0,180,Space.Self);
                else if(adyacentTiles[1] && adyacentTiles[2])  transform.Rotate(0,180,0,Space.Self);
                //   if(adyacentTiles[2] && adyacentTiles[3]) transform.Rotate(0,0,  0,Space.Self); Initial position
                else if(adyacentTiles[3] && adyacentTiles[0])  transform.Rotate(180,0,0,Space.Self);

            }
        }

        else if(adyacentTilesNumber==3)
        {
            mySpriteRenderer.sprite = tile3;
            ChangeAnimationState(CLOUD_3DOWN);

                //   if(!adyacentTiles[0]) transform.Rotate(0,0,  0,Space.Self); Initial Position
                if     (!adyacentTiles[1]) transform.Rotate(0,0, 90,Space.Self);
                else if(!adyacentTiles[2]) transform.Rotate(0,0,180,Space.Self);
                else if(!adyacentTiles[3]) transform.Rotate(0,0,-90,Space.Self);
        }
    
        else if(adyacentTilesNumber==4)
        {
            //myAnimator.enabled = false;
            mySpriteRenderer.sprite = tile4;

        } 

        else Debug.Log("Error in adyacent tiles");

        //Once the tile is correctly instatiated, the proper shadow is instantiated
        InstantiateShadow();
    }

    private void InstantiateShadow()
    {
        //For the shadows we check the immediate left/right tiles and the three tiles under the left/self/right tile
        //0-left, 1-right, 2-left/down, 3-down, 4-right/down
        for (int index = 0; index < adyacentTilesForShadow.Length; index ++)
        {
            adyacentTilesForShadow[index] = false;
        }

        for (int i = 0; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //Avoid out of the boundaries of the matrix
                bool indexInMatrixBoundaries = matrixCoordinates[0] + i > 0                        && matrixCoordinates[1] + j > 0 &&
                                               matrixCoordinates[0] + i < MatrixManager.instance.GetItemsLayoutMatrix().GetLength(0) && matrixCoordinates[1] + j < MatrixManager.instance.GetItemsLayoutMatrix().GetLength(1);

                if (indexInMatrixBoundaries)
                {
                    if(objectNumber == -999 || objectNumber == 999)
                    {
                        if (Mathf.Abs(MatrixManager.instance.GetItemsLayoutMatrix()[matrixCoordinates[0] + i, matrixCoordinates[1] + j]) == Mathf.Abs(objectNumber))
                        {
                                if  ( i ==  0 && j == -1) adyacentTilesForShadow[0] =  true;
                            else if ( i ==  0 && j ==  1) adyacentTilesForShadow[1] =  true;
                            else if ( i ==  1 && j == -1) adyacentTilesForShadow[2] =  true;
                            else if ( i ==  1 && j ==  0) adyacentTilesForShadow[3] =  true;
                            else if ( i ==  1 && j ==  1) adyacentTilesForShadow[4] =  true;
                        }
                    }
                    else if (MatrixManager.instance.GetItemsLayoutMatrix()[matrixCoordinates[0] + i, matrixCoordinates[1] + j] == objectNumber)
                    {
                             if ( i ==  0 && j == -1) adyacentTilesForShadow[0] =  true;
                        else if ( i ==  0 && j ==  1) adyacentTilesForShadow[1] =  true;
                        else if ( i ==  1 && j == -1) adyacentTilesForShadow[2] =  true;
                        else if ( i ==  1 && j ==  0) adyacentTilesForShadow[3] =  true;
                        else if ( i ==  1 && j ==  1) adyacentTilesForShadow[4] =  true;
                    }
                }

            }
        }

        //Instantiate needed shadows
        //Always instantiate if tile just below is empty
        if (!adyacentTilesForShadow[3]) Instantiate(tileShadow, transform.position + new Vector3(0, -shadowToWorldCorrection, 0), Quaternion.identity, gameObject.transform);
        //Always instantiate if there is a cloud tile at the left/right that it doesn't have a cloud tile below
        else if(adyacentTilesForShadow[0] && !adyacentTilesForShadow[2]) Instantiate(tileShadow, transform.position + new Vector3(0, -shadowToWorldCorrection, 0), Quaternion.identity, gameObject.transform);
        else if(adyacentTilesForShadow[1] && !adyacentTilesForShadow[4]) Instantiate(tileShadow, transform.position + new Vector3(0, -shadowToWorldCorrection, 0), Quaternion.identity, gameObject.transform);
    }

    public int GetObjectNumber()
    {
        return objectNumber;
    }

    public bool[] GetAdyacentTilesForShadow()
    {
        return adyacentTilesForShadow;
    }

    public void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        /*if(currentState == newState) return;

        //play the animation
        myAnimator.Play(newState);

        //reassign the current state
        currentState = newState;*/
    }
}
