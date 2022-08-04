using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatedSpikedFloor : MonoBehaviour
{
    [Header("Tile Sprites")]
    [SerializeField] Sprite tile0;
    [SerializeField] Sprite tile1_Left;
    [SerializeField] Sprite tile2Corner;
    [SerializeField] Sprite tile2Parallel;
    [SerializeField] Sprite tile3;

    MatrixManager matrixManager;
    int[,] itemsLayoutMatrix;
    int[,] mechanicsLayoutMatrix;
    int[] matrixCoordinates;
    int objectNumber;

    SpriteRenderer mySpriteRenderer;

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        //Info about the layout and matrix of the level
        matrixManager = FindObjectOfType<MatrixManager>();
        itemsLayoutMatrix = matrixManager.GetItemsLayoutMatrix();
        mechanicsLayoutMatrix = matrixManager.GetMechanicsLayoutMatrix();
        matrixCoordinates = matrixManager.FromWorldToMatrixIndex(transform.position);
        objectNumber = itemsLayoutMatrix[matrixCoordinates[0], matrixCoordinates[1]];

        SelectCorrectSprite();
    }

    void SelectCorrectSprite()
    {
        // 0-up, 1-left, 2-down, 3-right
        bool[] adyacentTiles = new bool[4];

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
                                               matrixCoordinates[0] + i < itemsLayoutMatrix.GetLength(0) && matrixCoordinates[1] + j < itemsLayoutMatrix.GetLength(1);

                //Mathf.Abs(i + j) == 1 used limit the checked tiles to the one at the right/left/up/down
                if (Mathf.Abs(i + j) == 1 && indexInMatrixBoundaries)
                {
                    if (mechanicsLayoutMatrix[matrixCoordinates[0] + i, matrixCoordinates[1] + j] == matrixManager.valueSpikedFloorMechanic)
                    {
                        if ( i == -1 && j ==  0) adyacentTiles[0] =  true;
                        if ( i ==  0 && j == -1) adyacentTiles[1] =  true;
                        if ( i ==  1 && j ==  0) adyacentTiles[2] =  true;
                        if ( i ==  0 && j ==  1) adyacentTiles[3] =  true;
                    }
                }
            }
        }

        for (int index = 0; index < adyacentTiles.Length; index ++)
        {
            if(adyacentTiles[index])adyacentTilesNumber++;
        }


        //Use adyacentTilesNumber to do a first filter to select the sprite based on how many other cloud tiles are connected to this one.
        //Then sprite is rotated or fliped to match the postion
        if(adyacentTilesNumber==0)
        {
            mySpriteRenderer.sprite = tile0;
        } 

        else if(adyacentTilesNumber==1) 
        {
            if(adyacentTiles[0])
            {
                mySpriteRenderer.sprite = tile1_Left;
                transform.Rotate(0,0,-90,Space.Self);
                mySpriteRenderer.flipY = true;
                if(CheckForCornerTile(-1, 0)) mySpriteRenderer.flipY = false;
                //if(CheckAdyacentTiles(-1, 0) == 1) mySpriteRenderer.flipX = false;
                //else mySpriteRenderer.flipX = true;                
            }
            else if(adyacentTiles[1])
            {
                mySpriteRenderer.sprite = tile1_Left;
                if(CheckForCornerTile(0, -1)) mySpriteRenderer.flipY = true;
                /*if(CheckAdyacentTiles(0, -1) == 1)
                {
                    mySpriteRenderer.sprite = tile1_Left;
                    transform.Rotate(0,0,90,Space.Self);
                    //mySpriteRenderer.flipY = true;
                }*/
            }
            else if(adyacentTiles[2])
            {
                mySpriteRenderer.sprite = tile1_Left;
                transform.Rotate(0,0,90,Space.Self);
                //mySpriteRenderer.flipX = true;
            }
            else if(adyacentTiles[3])
            {
                mySpriteRenderer.sprite = tile1_Left;
                transform.Rotate(0,0,180,Space.Self);
                mySpriteRenderer.flipY = true;
                /*if(CheckAdyacentTiles(0, 1) == 1)
                {
                    mySpriteRenderer.sprite = tile1_Left;
                    transform.Rotate(0,0,-90,Space.Self);
                    mySpriteRenderer.flipX = true;
                }*/
            }
        }

        else if(adyacentTilesNumber==2)
        {
            if(adyacentTiles[0] && adyacentTiles[2])
            {
                mySpriteRenderer.sprite = tile2Parallel;
                transform.Rotate(0,0,90,Space.Self);
                if(CheckForCornerTile(-1, 0)) mySpriteRenderer.flipY = true;
            }
            else if(adyacentTiles[1] && adyacentTiles[3])
            {
                mySpriteRenderer.sprite = tile2Parallel;
                if(CheckForCornerTile(0, -1)) mySpriteRenderer.flipY = true;
            }
            else
            {
                mySpriteRenderer.sprite = tile2Corner;

                if     (adyacentTiles[0] && adyacentTiles[1]) transform.Rotate(0,0,180,Space.Self);
                else if(adyacentTiles[1] && adyacentTiles[2]) transform.Rotate(0,0,270,Space.Self);
                //   if(adyacentTiles[2] && adyacentTiles[3]) transform.Rotate(0,0,  0,Space.Self); Initial position
                else if(adyacentTiles[3] && adyacentTiles[0]) transform.Rotate(0,0,90,Space.Self);

            }
        }

        else if(adyacentTilesNumber==3)
        {
            mySpriteRenderer.sprite = tile3;

                if(!adyacentTiles[0]) transform.Rotate( 0, 0, 180,Space.Self);
                else if     (!adyacentTiles[1]) transform.Rotate(0,0, -90,Space.Self);
                //else if(!adyacentTiles[2]) transform.Rotate(0,0,180,Space.Self);
                else if(!adyacentTiles[3]) transform.Rotate(0,0,90,Space.Self);
        }
    
        else if(adyacentTilesNumber==4) mySpriteRenderer.sprite = null;

        else Debug.LogError("Error in adyacent tiles");

    }

    bool[] CheckAdyacentTiles(int u, int k)
    {
        bool[] adyacentTiles = new bool[4] {false, false, false, false};

        if (mechanicsLayoutMatrix[matrixCoordinates[0] + u, matrixCoordinates[1] + k] != matrixManager.valueSpikedFloorMechanic) return adyacentTiles;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //Avoid out of the boundaries of the matrix
                bool indexInMatrixBoundaries = matrixCoordinates[0] + u + i > 0                              && matrixCoordinates[1] + k + j > 0 &&
                                               matrixCoordinates[0] + u + i < itemsLayoutMatrix.GetLength(0) && matrixCoordinates[1] + k + j < itemsLayoutMatrix.GetLength(1);

                //Mathf.Abs(i + j) == 1 used limit the checked tiles to the one at the right/left/up/down
                if (Mathf.Abs(i + j) == 1 && indexInMatrixBoundaries)
                {
                    if (mechanicsLayoutMatrix[matrixCoordinates[0] + u + i, matrixCoordinates[1] + k + j] == matrixManager.valueSpikedFloorMechanic)
                    {
                        if ( i == -1 && j ==  0) adyacentTiles[0] = true;
                        if ( i ==  0 && j == -1) adyacentTiles[1] = true;
                        if ( i ==  1 && j ==  0) adyacentTiles[2] = true;
                        if ( i ==  0 && j ==  1) adyacentTiles[3] = true;
                    }
                }
            }
        }

        return adyacentTiles;
    }


    bool CheckForCornerTile(int u, int k)
    {
        int adyacentTilesNumber = 0;
        bool[] adyacentTiles = CheckAdyacentTiles(u, k);

        for (int i = 0; i < 4; i++)
        {
            if(adyacentTiles[i]) adyacentTilesNumber++;
            //Debug.Log("Adyacent Tile " + i + ": " + adyacentTiles[i]);
        }

       // Debug.Log("Adyacent Tiles Number: " + adyacentTilesNumber);

        if(adyacentTilesNumber == 2)
        {
            if(adyacentTiles[1] && adyacentTiles[2])
            {
                return true;
            }
            else if (adyacentTiles[0] && adyacentTiles[2])
            {
                if(CheckForCornerTile(u - 1, k)) return true;
            }
            else if(adyacentTiles[0] && adyacentTiles[3])
            {
                return true;
            } 
            else if (adyacentTiles[1] && adyacentTiles[3])
            {
                if(CheckForCornerTile(u, k - 1)) return true;
            }
        }

        return false;

    }

    public int GetobjectNumber()
    {
        return objectNumber;
    }
}
