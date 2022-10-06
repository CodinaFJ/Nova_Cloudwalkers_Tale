using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    [SerializeField]
    protected TileType tileType;
    protected enum TileType{
        Floor, CrystalFloor, SpikedFloor,
        WhiteCloud, GreyCloud, CrystalCloudBot, CrystalCloudTop, ThunderCloud
    }

    SpriteRenderer mySpriteRenderer;

    protected int[] matrixCoordinates;

    MatrixManager matrixManager;
    int[,] itemsLayoutMatrix;
    int[,] mechanicsLayoutMatrix;
    int itemNumber;
    int mechanicNumber;

    // 0-up, 1-left, 2-down, 3-right
    bool[] adyacentTiles = new bool[4];
    int adyacentTilesNumber = 0;

    private void Start() {
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        InitilizeTileInfo();
        SelectCorrectSprite();
    }

    protected void InitilizeTileInfo()
    {
        itemsLayoutMatrix = MatrixManager.instance.GetItemsLayoutMatrix();
        mechanicsLayoutMatrix = MatrixManager.instance.GetMechanicsLayoutMatrix();
        matrixCoordinates = MatrixManager.instance.FromWorldToMatrixIndex(transform.position);
        itemNumber = itemsLayoutMatrix[matrixCoordinates[0], matrixCoordinates[1]];
        mechanicNumber = mechanicsLayoutMatrix[matrixCoordinates[0], matrixCoordinates[1]];
    }

    protected void SelectCorrectSprite()
    {
        CalculateAdyacentTiles();

        SetAndOrientateSprites();

        //Once the tile is correctly instatiated, the proper shadow is instantiated
        InstantiateShadow();
    }


    protected virtual void CalculateAdyacentTiles()
    {
        // 0-up, 1-left, 2-down, 3-right
        adyacentTiles = new bool[4];

        for (int index = 0; index < adyacentTiles.Length; index ++)
        {
            adyacentTiles[index] = false;
        }

        adyacentTilesNumber = 0;
        
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //Mathf.Abs(i + j) == 1 used to limit the checked tiles to the one at the right/left/up/down
                if (Mathf.Abs(i + j) == 1 && CheckCoordinatesInMatrix(i,j))
                {
                    if(tileType == TileType.Floor)
                    {
                        if (Mathf.Abs(itemsLayoutMatrix[matrixCoordinates[0] + i, matrixCoordinates[1] + j]) == Mathf.Abs(itemNumber))
                        {
                            if ( i == -1 && j ==  0) adyacentTiles[0] =  true;
                            if ( i ==  0 && j == -1) adyacentTiles[1] =  true;
                            if ( i ==  1 && j ==  0) adyacentTiles[2] =  true;
                            if ( i ==  0 && j ==  1) adyacentTiles[3] =  true;
                        }
                    }

                    else if(tileType == TileType.SpikedFloor)
                    {
                        if (mechanicsLayoutMatrix[matrixCoordinates[0] + i, matrixCoordinates[1] + j] == mechanicNumber)
                        {
                            if ( i == -1 && j ==  0) adyacentTiles[0] =  true;
                            if ( i ==  0 && j == -1) adyacentTiles[1] =  true;
                            if ( i ==  1 && j ==  0) adyacentTiles[2] =  true;
                            if ( i ==  0 && j ==  1) adyacentTiles[3] =  true;
                            
                        }
                    }
                    else
                    {
                        if (itemsLayoutMatrix[matrixCoordinates[0] + i, matrixCoordinates[1] + j] == itemNumber)
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
    }

    protected void SetAndOrientateSprites()
    {
        //Use adyacentTilesNumber to do a first filter to select the sprite based on how many other cloud tiles are connected to this one.
        //Then sprite is rotated or fliped to match the postion
        if(adyacentTilesNumber==0)
        {
            mySpriteRenderer.sprite = tile0;
        } 

        else if(adyacentTilesNumber==1) 
        {
            mySpriteRenderer.sprite = tile1;

            if(adyacentTiles[0])
            {
               transform.Rotate(0,0,90,Space.Self);
               mySpriteRenderer.flipY = true;
            }
            else if(adyacentTiles[2])
            {
                transform.Rotate(0,0,-90,Space.Self);
            }
            else if(adyacentTiles[1])
            {
                mySpriteRenderer.flipX = true;
            }

        }

        else if(adyacentTilesNumber==2)
        {
            if(adyacentTiles[0] && adyacentTiles[2])
            {
                mySpriteRenderer.sprite = tile2Parallel;
                transform.Rotate(0,0,90,Space.Self);
            }
            else if(adyacentTiles[1] && adyacentTiles[3])
            {
                mySpriteRenderer.sprite = tile2Parallel;
            }
            else
            {
                mySpriteRenderer.sprite = tile2Corner;

                if     (adyacentTiles[0] && adyacentTiles[1]) transform.Rotate(0,0,180,Space.Self);
                else if(adyacentTiles[1] && adyacentTiles[2]) mySpriteRenderer.flipX = true;
                //   if(adyacentTiles[2] && adyacentTiles[3]) transform.Rotate(0,0,  0,Space.Self); Initial position
                else if(adyacentTiles[3] && adyacentTiles[0]) mySpriteRenderer.flipY = true;

            }
        }

        else if(adyacentTilesNumber==3)
        {
            mySpriteRenderer.sprite = tile3;

                //   if(!adyacentTiles[0]) transform.Rotate(0,0,  0,Space.Self); Initial Position
                if     (!adyacentTiles[1]) transform.Rotate(0,0, 90,Space.Self);
                else if(!adyacentTiles[2]) transform.Rotate(0,0,180,Space.Self);
                else if(!adyacentTiles[3]) transform.Rotate(0,0,-90,Space.Self);
        }
    
        else if(adyacentTilesNumber==4) mySpriteRenderer.sprite = tile4;

        else Debug.Log("Error in adyacent tiles");
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
                if (CheckCoordinatesInMatrix(i,j))
                {
                    if(objectNumber == -999 || objectNumber == 999)
                    {
                        if (Mathf.Abs(itemsLayoutMatrix[matrixCoordinates[0] + i, matrixCoordinates[1] + j]) == Mathf.Abs(objectNumber))
                        {
                                if ( i ==  0 && j == -1) adyacentTilesForShadow[0] =  true;
                            else if ( i ==  0 && j ==  1) adyacentTilesForShadow[1] =  true;
                            else if ( i ==  1 && j == -1) adyacentTilesForShadow[2] =  true;
                            else if ( i ==  1 && j ==  0) adyacentTilesForShadow[3] =  true;
                            else if ( i ==  1 && j ==  1) adyacentTilesForShadow[4] =  true;
                        }
                    }
                    else if (itemsLayoutMatrix[matrixCoordinates[0] + i, matrixCoordinates[1] + j] == objectNumber)
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

    protected bool CheckCoordinatesInMatrix(int i, int j){
        bool indexInMatrixBoundaries = matrixCoordinates[0] + i > 0                              && matrixCoordinates[1] + j > 0 &&
                                        matrixCoordinates[0] + i < itemsLayoutMatrix.GetLength(0) && matrixCoordinates[1] + j < itemsLayoutMatrix.GetLength(1);

        return indexInMatrixBoundaries;
    }

}
