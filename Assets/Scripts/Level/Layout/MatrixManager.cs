using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MatrixManager : MonoBehaviour
{   
    public static MatrixManager instance;

    static int[,] itemsLayoutMatrix;
    static int[,] mechanicsLayoutMatrix;

    static bool[,] pjMovementMatrix;
    static bool[,] cloudMovementMatrix;

    [HideInInspector]
    public PathNode [,] pathNodesMatrix;

    Vector3 coordinatesOriginMatrix = new Vector3 (0f,0f,0f);

    TilemapsLevelLayout tilemapsLevelLayout;

    [HideInInspector]
    public int valueForBorder = -9, 
               valueForCloud = 0, 
               valueForFloor = 999, 
               valueForItemSpikedFloor = 999, 
               valueForCrystalFloor = 998;
    
    [HideInInspector]
    public int valueWhiteCloudMechanic = 1, 
               valueGreyCloudMechanic = 2, 
               valueCrystalCloudMechanic = 3, 
               valueCrystalFloorMechanic = 5, 
               valueThunderCloudMechanic = -1, 
               valueSpikedFloorMechanic = -999;
    
    bool stateSaved = false;
    

    fromMatrixToGame FromMatrixToGame;

    PlayerBehavior playerBehavior;

    CloudSfxManager cloudSfxManager;

    void Awake()
    {
        if(instance == null)
           instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        tilemapsLevelLayout = FindObjectOfType<TilemapsLevelLayout>();
        CreateItemsLayoutMatrix();
        CreateMechanicsLayoutMatrix();
        CreatePjMovementMatrix();
        CreateCloudsMovementMatrix();
        CreatePathNodesMatrix();

        cloudSfxManager = FindObjectOfType<CloudSfxManager>();
    }

    void Start()
    {
        FromMatrixToGame = FindObjectOfType<fromMatrixToGame>();
        playerBehavior = FindObjectOfType<PlayerBehavior>();
    }




//ITEMS LAYOUT MATRIX: This one uses the same number for every object that must be considered as one.
//0 Air / 1-998 Clouds / 999 Floors/ 998 Crystal Floors / -9 Border / 999 Spiked Floors / 4001 - 4998 Cloud arriving to position / 500X Grey cloud being sticked to normal cloud

    void LoadItemsLayoutMatrix()
    {

    }

    void CreateItemsLayoutMatrix()
    {
        tilemapsLevelLayout.FillCloudTilemaps();

        //tilemapsLevelLayout.borderTilemap.CompressBounds(); //TODO: check if this is really needed. Generates error in mural level

        BoundsInt bounds = tilemapsLevelLayout.borderTilemap.cellBounds;

        //Create level matrix
        int[] levelMatrixDimensions = {bounds.max.y - bounds.min.y, bounds.max.x - bounds.min.x};
        itemsLayoutMatrix = new int[levelMatrixDimensions[0], levelMatrixDimensions[1]];

        coordinatesOriginMatrix = new Vector3(bounds.min.x, bounds.max.y - 1, 0);

        //Fill matrix with air
        for (int x = 0; x < itemsLayoutMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < itemsLayoutMatrix.GetLength(1); y++)
            {
                itemsLayoutMatrix[x,y] = 0;
            }
        }

        itemsLayoutMatrix = AddTilemapToItemsLayoutMatrix(tilemapsLevelLayout.borderTilemap, valueForBorder, itemsLayoutMatrix);
        if(tilemapsLevelLayout.blockTilemap != null) itemsLayoutMatrix = AddTilemapToItemsLayoutMatrix(tilemapsLevelLayout.blockTilemap, valueForBorder, itemsLayoutMatrix);

        for (int index = 0; index < tilemapsLevelLayout.cloudsTilemaps.Length; index++)
        {
            valueForCloud ++;
            itemsLayoutMatrix = AddTilemapToItemsLayoutMatrix(tilemapsLevelLayout.cloudsTilemaps[index], valueForCloud, itemsLayoutMatrix);
        }

        itemsLayoutMatrix = AddTilemapToItemsLayoutMatrix(tilemapsLevelLayout.floorTilemap, valueForFloor, itemsLayoutMatrix);
        itemsLayoutMatrix = AddTilemapToItemsLayoutMatrix(tilemapsLevelLayout.spikedFloorTilemap, valueForItemSpikedFloor, itemsLayoutMatrix);
        

        //itemsLayoutMatrix = ReduceItemLayoutMatrixToBoundaries(itemsLayoutMatrix);
    }

    public int[,] GetItemsLayoutMatrix()
    {
        return itemsLayoutMatrix;
    }

    int[,] AddTilemapToItemsLayoutMatrix (Tilemap tilemap, int valueForItem, int[,] itemsLayoutMatrix)
    {
        tilemap.CompressBounds();
        BoundsInt bounds = tilemap.cellBounds;

        //Loop through all the tiles in the bounds of the tilemap
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x,y,0);
                TileBase tile = tilemap.GetTile(cellPosition);

                //Avoid cells with no tile
                if(tile == null) continue;

                int[] matrixPosition = {(int)(coordinatesOriginMatrix.y - cellPosition.y), (int)(cellPosition.x - coordinatesOriginMatrix.x)};
                if(tile.name == "FloorCrystalRuleTile") itemsLayoutMatrix[matrixPosition[0], matrixPosition[1]] = valueForItem - 1;
                else if(itemsLayoutMatrix[matrixPosition[0], matrixPosition[1]] == valueForBorder) continue;
                else itemsLayoutMatrix[matrixPosition[0], matrixPosition[1]] = valueForItem;
            }
        }
        return itemsLayoutMatrix;
    }

    int[,] ReduceItemLayoutMatrixToBoundaries(int[,] itemsLayoutMatrix)
    {
        bool[] uselessRows = new bool[itemsLayoutMatrix.GetLength(0)];
        bool[] uselessColumns = new bool[itemsLayoutMatrix.GetLength(1)];

        bool thisIsUseless = true;

        int numberOfUselessRows = 0;
        int numberOfUselessColumns = 0;

        int removeRowsOriginIndex = 0;
        int removeColumsOriginIndex = 0;

        //Which rows are useless border
        for (int i = 0; i < itemsLayoutMatrix.GetLength(0); i++)
        {
            thisIsUseless = true;

            for (int j = 0; j < itemsLayoutMatrix.GetLength(1); j++)
            {
                //If row i, i+1 or i-1 has something that is not border, then the row is not useless
                if(itemsLayoutMatrix[i, j] != valueForBorder && thisIsUseless) {thisIsUseless = false;}

                if(i+1 < itemsLayoutMatrix.GetLength(0))
                {
                    if(itemsLayoutMatrix[i+1, j] != valueForBorder && thisIsUseless) {thisIsUseless = false;}
                }

                if(i-1 > 0)
                {
                    if(itemsLayoutMatrix[i-1, j] != valueForBorder && thisIsUseless) {thisIsUseless = false;}
                }
            }

            if (thisIsUseless && i == removeRowsOriginIndex)
            {
                removeRowsOriginIndex++;
            }

            uselessRows[i] = thisIsUseless;
        }

        //Which columns are useless border
        for (int j = 0; j < itemsLayoutMatrix.GetLength(1); j++)
        {
            thisIsUseless = true;

            for (int i = 0; i < itemsLayoutMatrix.GetLength(0); i++)
            {
                //If column i, i+1 or i-1 has something that is not border, then the row is not useless
                if(itemsLayoutMatrix[i, j] != valueForBorder && thisIsUseless) {thisIsUseless = false;}

                if(j+1 < itemsLayoutMatrix.GetLength(1))
                {
                    if(itemsLayoutMatrix[i, j+1] != valueForBorder && thisIsUseless) {thisIsUseless = false;}
                }

                if(j-1 > 0)
                {
                    if(itemsLayoutMatrix[i, j-1] != valueForBorder && thisIsUseless) {thisIsUseless = false;}
                }
            }

            if (thisIsUseless && j == removeColumsOriginIndex)
            {
                removeColumsOriginIndex++;
            }

            uselessColumns[j] = thisIsUseless;
        }

        coordinatesOriginMatrix = new Vector3 ( coordinatesOriginMatrix.x + removeColumsOriginIndex, coordinatesOriginMatrix.y - removeRowsOriginIndex,0f);

        //How many useless columns and rows are there
        for (int i = 0; i < uselessRows.GetLength(0); i ++)
        {
            if (uselessRows[i]) numberOfUselessRows++;
        }
        for (int j = 0; j < uselessColumns.GetLength(0); j ++)
        {
            if (uselessColumns[j]) numberOfUselessColumns++;
        }

        //Create a reduced matrix with only the not useless rows and columns
        int[,] reducedlevelLayoutMatrix = new int [itemsLayoutMatrix.GetLength(0) - numberOfUselessRows, itemsLayoutMatrix.GetLength(1) - numberOfUselessColumns];

        for (int i = 0, j = 0; i < itemsLayoutMatrix.GetLength(0); i++)
        {
            if (uselessRows[i])
                continue;

            for (int k = 0, u = 0; k < itemsLayoutMatrix.GetLength(1); k++)
            {
                if (uselessColumns[k])
                    continue;

                reducedlevelLayoutMatrix[j, u] = itemsLayoutMatrix[i, k];
                u++;
            }
            j++;
        }

        return reducedlevelLayoutMatrix;
    }

    public bool InsideLevelMatrix(int[] matrixCoordinates)
    {
        bool indexInMatrixBoundaries = matrixCoordinates[0] >= 0                        && matrixCoordinates[1]  >= 0 &&
                                       matrixCoordinates[0] < itemsLayoutMatrix.GetLength(0) && matrixCoordinates[1]  < itemsLayoutMatrix.GetLength(1);

        return indexInMatrixBoundaries;
    }

//MECHANICS LAYOUT MATRIX: Shows mechanic of each tile in the game with numbers
//0 Air / 1 White Cloud / 2 Grey Cloud / 3 Cloud Crystal / 4 Stepped Cloud Crystal / 5 Floor Crystal / 6 Stepped Floor Crystal / -1 Thunder Cloud / 999 Floor /-9 Border / -999 Spiked Floor

    void LoadMechanicsLayoutMatrix()
    {

    }

    void CreateMechanicsLayoutMatrix()
    {
        mechanicsLayoutMatrix = new int[itemsLayoutMatrix.GetLength(0), itemsLayoutMatrix.GetLength(1)];

        for (int x = 0; x < mechanicsLayoutMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < mechanicsLayoutMatrix.GetLength(1); y++)
            {
                if(itemsLayoutMatrix[x,y]==-9) mechanicsLayoutMatrix[x,y] =  -9;
                else mechanicsLayoutMatrix[x,y] = 0;
            }
        }

        for (int index = 0; index < tilemapsLevelLayout.cloudsTilemaps.Length; index++)
        {
            valueForCloud ++;
            mechanicsLayoutMatrix = AddTilemapToMechanicsLayoutMatrix(tilemapsLevelLayout.cloudsTilemaps[index], mechanicsLayoutMatrix);
        }

        mechanicsLayoutMatrix = AddTilemapToMechanicsLayoutMatrix(tilemapsLevelLayout.floorTilemap, mechanicsLayoutMatrix);
        mechanicsLayoutMatrix = AddTilemapToMechanicsLayoutMatrix(tilemapsLevelLayout.spikedFloorTilemap, mechanicsLayoutMatrix);
    }

    public int[,] GetMechanicsLayoutMatrix()
    {
        return mechanicsLayoutMatrix;
    }

    int[,] AddTilemapToMechanicsLayoutMatrix (Tilemap tilemapItem, int[,] mechanicsLayoutMatrix)
    {
        tilemapItem.CompressBounds();
        BoundsInt bounds = tilemapItem.cellBounds;

        //Loop through all the tiles in the bounds of the tilemap
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x,y,0);
                TileBase tile = tilemapItem.GetTile(cellPosition);

                //Avoid cells with no tile
                if(tile == null) continue;

                int[] matrixPosition = {(int)(coordinatesOriginMatrix.y - cellPosition.y), (int)(cellPosition.x - coordinatesOriginMatrix.x)};

                if(mechanicsLayoutMatrix[matrixPosition[0], matrixPosition[1]] == valueForBorder) continue;

                     if(tile.name == "WhiteCloudsRuleTile")   mechanicsLayoutMatrix[matrixPosition[0], matrixPosition[1]] = valueWhiteCloudMechanic;
                else if(tile.name == "GreyCloudsRuleTile")    mechanicsLayoutMatrix[matrixPosition[0], matrixPosition[1]] = valueGreyCloudMechanic;
                else if(tile.name == "CrystalRuleTile")       mechanicsLayoutMatrix[matrixPosition[0], matrixPosition[1]] = valueCrystalCloudMechanic;
                else if(tile.name == "FloorCrystalRuleTile")  mechanicsLayoutMatrix[matrixPosition[0], matrixPosition[1]] = valueCrystalFloorMechanic;
                else if(tile.name == "ThunderCloudsRuleTile") mechanicsLayoutMatrix[matrixPosition[0], matrixPosition[1]] = valueThunderCloudMechanic;
                else if(tile.name == "FloorRuleTile")         mechanicsLayoutMatrix[matrixPosition[0], matrixPosition[1]] = valueForFloor;
                else if(tile.name == "SpikedFloorRuleTile")   mechanicsLayoutMatrix[matrixPosition[0], matrixPosition[1]] = valueSpikedFloorMechanic;
                
            }
        }
        return mechanicsLayoutMatrix;
    }



//PJ MOVEMENT MATRIX: Shows wether the character can move to a specific position or not
//FALSE Cannot move / TRUE Can move

    void CreatePjMovementMatrix()
    {

        pjMovementMatrix = new bool[itemsLayoutMatrix.GetLength(0), itemsLayoutMatrix.GetLength(1)];

        for (int x = 0; x < mechanicsLayoutMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < mechanicsLayoutMatrix.GetLength(1); y++)
            {
                if(mechanicsLayoutMatrix[x,y]>0 && itemsLayoutMatrix[x,y]<4000) pjMovementMatrix[x,y] = true;

                else pjMovementMatrix[x,y] = false;
            }
        }
    }

    void RefreshPjMovementMatrix()
    {
        for (int x = 0; x < mechanicsLayoutMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < mechanicsLayoutMatrix.GetLength(1); y++)
            {
                if(mechanicsLayoutMatrix[x,y]>0 && itemsLayoutMatrix[x,y]<4000) pjMovementMatrix[x,y] = true;

                else pjMovementMatrix[x,y] = false;
            }
        }
    }

    public bool[,] GetPjMovementMatrix()
    {
        return pjMovementMatrix;
    }

//CLOUD MOVEMENT MATRIX: Shows wether the clouds can move to a specific position or not
//FALSE Cannot move / TRUE Can move

    void CreateCloudsMovementMatrix()
    {
        cloudMovementMatrix = new bool[itemsLayoutMatrix.GetLength(0), itemsLayoutMatrix.GetLength(1)];

        for (int x = 0; x < itemsLayoutMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < itemsLayoutMatrix.GetLength(1); y++)
            {
                if(mechanicsLayoutMatrix[x,y]==0) cloudMovementMatrix[x,y] = true;

                else cloudMovementMatrix[x,y] = false;
            }
        }
    }
    
    void RefreshCloudsMovementMatrix()
    {
        for (int x = 0; x < itemsLayoutMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < itemsLayoutMatrix.GetLength(1); y++)
            {
                if(mechanicsLayoutMatrix[x,y]==0) cloudMovementMatrix[x,y] = true;

                else cloudMovementMatrix[x,y] = false;
            }
        }
    }

    public bool[,] GetCloudMovementMatrix()
    {
        return cloudMovementMatrix;
    }


//CLOUD MOVING METHODS

    public void StartCloudMovementInMatrix(int[] unitaryCellDrag, int item)
    {
        //Loop through items and mechanics matrixes
        for (int i = 0; i < itemsLayoutMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < itemsLayoutMatrix.GetLength(1); j++)
            {
                int[] nextCell ={i + unitaryCellDrag[0],j + unitaryCellDrag[1]};
                int[] currentCell = {i, j};
                
                //Finds out the first cloud element of the row that would be moved through air and goes back through cloud in that row moving all elements
                if(itemsLayoutMatrix[i,j] == item && itemsLayoutMatrix[nextCell[0], nextCell[1]] != item)
                {
                    while (itemsLayoutMatrix[currentCell[0], currentCell[1]] == item)
                    {
                        mechanicsLayoutMatrix[nextCell[0], nextCell[1]] = mechanicsLayoutMatrix[currentCell[0], currentCell[1]];
                        mechanicsLayoutMatrix[currentCell[0], currentCell[1]] = 0;

                        itemsLayoutMatrix[nextCell[0], nextCell[1]] = item + 4000; //Use this 4000 so the algorythm won't stop on already moved clouds again
                        itemsLayoutMatrix[currentCell[0], currentCell[1]] = 0;

                        nextCell[0]-= unitaryCellDrag[0];
                        nextCell[1]-= unitaryCellDrag[1];

                        currentCell[0]-= unitaryCellDrag[0];
                        currentCell[1]-= unitaryCellDrag[1];
                    }
                }
            }                
        }

        RefreshCloudsMovementMatrix();
        RefreshPjMovementMatrix();
    }

    public void FinishCloudMovementInMatrix(int[] unitaryCellDrag, int item)
    {
        //Back to normal item values
        for (int i = 0; i < itemsLayoutMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < itemsLayoutMatrix.GetLength(1); j++)
            {
                if(itemsLayoutMatrix[i,j] > 4000 && itemsLayoutMatrix[i,j] < 5000)
                {
                    itemsLayoutMatrix[i,j] = itemsLayoutMatrix[i,j] - 4000;
                }
            }
        }

        RefreshCloudsMovementMatrix();
        RefreshPjMovementMatrix();
    }

    public void SearchGreyCloudContact(int item)
    {
        VFXManager.instance.PreInstantiateGreyParticles(item);
        if(AttachGreyCloudInMatrix(item))
        {
            FromMatrixToGame.ReInstantiateItem(item);
        }
    }

    void LoopGreyCloudContact(int item)
    {
        if(AttachGreyCloudInMatrix(item))
        {
            //CloudInputManager.instance.SetCloudMove(false);
            if(!stateSaved)
            {
                LevelStateManager.instance.SaveSpecificLevelState();
                LevelStateManager.instance.CaptureSpecificLevelState();
                stateSaved = true;
            }
            
            //FromMatrixToGame.ReInstantiateItem(item);
            playerBehavior.UpdateItemUnderPj();
            RefreshCloudsMovementMatrix();
            RefreshPjMovementMatrix();
            cloudSfxManager.PlayCloudConnect();

            
            if(mechanicsLayoutMatrix[PlayerBehavior.instance.pjCell[0], PlayerBehavior.instance.pjCell[1]] == valueCrystalCloudMechanic ||
               mechanicsLayoutMatrix[PlayerBehavior.instance.pjCell[0], PlayerBehavior.instance.pjCell[1]] == valueCrystalFloorMechanic)
            {
                mechanicsLayoutMatrix[PlayerBehavior.instance.pjCell[0], PlayerBehavior.instance.pjCell[1]] ++; 
                Debug.Log("Mechanic in tile under PJ: " + mechanicsLayoutMatrix[PlayerBehavior.instance.pjCell[0], PlayerBehavior.instance.pjCell[1]]);
            }
            
        }
        else stateSaved = false;
    }

    bool AttachGreyCloudInMatrix (int item)
    {
        bool attachingCloud = false;
        

        for (int i = 0; i < itemsLayoutMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < itemsLayoutMatrix.GetLength(1); j++)
            {
                if(itemsLayoutMatrix[i,j] == item + 5000)
                {
                    itemsLayoutMatrix[i,j] -= 5000;
                }
            }
        }

        for (int i = 0; i < itemsLayoutMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < itemsLayoutMatrix.GetLength(1); j++)
            {
                if(itemsLayoutMatrix[i,j] == item)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        for (int u = -1; u < 2; u++)
                        {
                            //Avoid out of the boundaries of the matrix
                            bool indexInMatrixBoundaries = i + k > 0                               && j + u > 0 &&
                                                        i + k < mechanicsLayoutMatrix.GetLength(0) && j + u < mechanicsLayoutMatrix.GetLength(1);

                            //Mathf.Abs(i + j) == 1 used limit the checked tiles to the one at the right/left/up/down
                            if (Mathf.Abs(k + u) == 1 && indexInMatrixBoundaries)
                            {
                                if (mechanicsLayoutMatrix[i + k, j + u] == valueGreyCloudMechanic)
                                {
                                    attachingCloud = true;
                                    VFXManager.instance.InstantiateGreyParticles(new Vector2(i,j), new Vector2(k, u));

                                    mechanicsLayoutMatrix[i + k, j + u] = mechanicsLayoutMatrix[i,j];                             

                                    FromMatrixToGame.DeactivateItem(itemsLayoutMatrix[i + k, j + u]);

                                    itemsLayoutMatrix[i + k, j + u] = item + 5000;
                                }
                                else if (itemsLayoutMatrix[i + k, j + u] > 5000)
                                {
                                    if(mechanicsLayoutMatrix[i + k, j + u] == valueWhiteCloudMechanic) continue;
                                    else if(mechanicsLayoutMatrix[i,j] == valueWhiteCloudMechanic) mechanicsLayoutMatrix[i + k, j + u] = mechanicsLayoutMatrix[i,j];
                                    else if(mechanicsLayoutMatrix[i + k, j + u] == valueCrystalCloudMechanic) continue;
                                    else if(mechanicsLayoutMatrix[i + k, j + u] == valueCrystalCloudMechanic + 1) continue;
                                    else if(mechanicsLayoutMatrix[i,j] == valueCrystalCloudMechanic) mechanicsLayoutMatrix[i + k, j + u] = mechanicsLayoutMatrix[i,j];
                                    else if(mechanicsLayoutMatrix[i + k, j + u] == valueThunderCloudMechanic) continue;
                                    else if(mechanicsLayoutMatrix[i,j] == valueThunderCloudMechanic) mechanicsLayoutMatrix[i + k, j + u] = mechanicsLayoutMatrix[i,j];
                                }
                            }
                        }
                    }
                }
            }
        }

        if (attachingCloud)
        {
            FromMatrixToGame.DeactivateItem(item);
            LoopGreyCloudContact(item);
        } 

        return attachingCloud;
    }


//CRYSTAL MECHANICS

    public void CheckForCrystal()
    {
        if(mechanicsLayoutMatrix[playerBehavior.pjCell[0], playerBehavior.pjCell[1]] == 3 || mechanicsLayoutMatrix[playerBehavior.pjCell[0], playerBehavior.pjCell[1]] == 5)
        {
            mechanicsLayoutMatrix[playerBehavior.pjCell[0], playerBehavior.pjCell[1]] ++;
        }
    }

    public void CrackCrystalFloor()
    {
        if(mechanicsLayoutMatrix[playerBehavior.pjCell[0], playerBehavior.pjCell[1]] == 6)
        {

            mechanicsLayoutMatrix[playerBehavior.pjCell[0], playerBehavior.pjCell[1]] = 0;
            itemsLayoutMatrix[playerBehavior.pjCell[0], playerBehavior.pjCell[1]] = 0;

            foreach(TileBehavior tile in FromMatrixToGame.GetFloorParent().GetComponentsInChildren<TileBehavior>()){
                tile.SetCorrectSpritesAfterCrack();
            }

            RefreshPjMovementMatrix();
            RefreshCloudsMovementMatrix();

            TileBehavior crystalTile = Array.Find(FromMatrixToGame.GetFloorParent().GetComponentsInChildren<TileBehavior>(),
                                                  x => x.GetTileCoordinates()[0] == playerBehavior.pjCell[0] && x.GetTileCoordinates()[1] == playerBehavior.pjCell[1]);

            crystalTile.gameObject.SetActive(false);
            Destroy(crystalTile.gameObject);
            
            VFXManager.instance.InstantiateParticles(ParticlesVFXType.CrystalFloorBreak);
        }
    }

    public void CrackCrystalCloud()
    {
        int[] crystalCell = playerBehavior.pjCell;
        if(mechanicsLayoutMatrix[crystalCell[0], crystalCell[1]] == 4)
        {
            int item = itemsLayoutMatrix[crystalCell[0], crystalCell[1]];

            mechanicsLayoutMatrix[crystalCell[0], crystalCell[1]] = 0;
            itemsLayoutMatrix[crystalCell[0], crystalCell[1]] = 0;

            RefreshPjMovementMatrix();
            RefreshCloudsMovementMatrix();
            VFXManager.instance.InstantiateParticles(ParticlesVFXType.CrystalCloudBreak);
            //FromMatrixToGame.DeactivateItem(item);


            DivideSeparatedCloudsInMatrix(crystalCell, item);
        }
    }

    void DivideSeparatedCloudsInMatrix(int[] crystalCell, int item)
    {
        FromMatrixToGame.DeactivateItem(item);
        for (int k = -1; k < 2; k++)
        {
            for (int u = -1; u < 2; u++)
            {
                //Avoid out of the boundaries of the matrix
                bool indexInMatrixBoundaries = crystalCell[0] + k > 0                              && crystalCell[1] + u > 0 &&
                                            crystalCell[0] + k < itemsLayoutMatrix.GetLength(0) && crystalCell[1] + u < itemsLayoutMatrix.GetLength(1);

                //Mathf.Abs(k + u) == 1 used limit the checked tiles to the one at the right/left/up/down
                if (Mathf.Abs(k + u) == 1 && indexInMatrixBoundaries)
                {
                    if (itemsLayoutMatrix[crystalCell[0] + k, crystalCell[1]  + u] == item)
                    {
                        FromMatrixToGame.CreateNewCloud();
                        int numberOfClouds = FromMatrixToGame.GetCloudsParents().Length;
                        int[] cloudCell = {crystalCell[0] + k, crystalCell[1]  + u};

                        ReAssignCloudItem(cloudCell, item, numberOfClouds);
                        FromMatrixToGame.ReInstantiateItem(FromMatrixToGame.GetCloudsParents().Length);
                    }
                }
            }
        }
        
    }

    void ReAssignCloudItem(int[] cloudCell, int item, int newCloudItem)
    {
        itemsLayoutMatrix[cloudCell[0], cloudCell[1]] = newCloudItem;
        
        for (int k = -1; k < 2; k++)
        {
            for (int u = -1; u < 2; u++)
            {
                //Avoid out of the boundaries of the matrix
                bool indexInMatrixBoundaries = cloudCell[0] + k > 0                              && cloudCell[1] + u > 0 &&
                                            cloudCell[0] + k < itemsLayoutMatrix.GetLength(0) && cloudCell[1] + u < itemsLayoutMatrix.GetLength(1);

                //Mathf.Abs(k + u) == 1 used limit the checked tiles to the one at the right/left/up/down
                if (Mathf.Abs(k + u) == 1 && indexInMatrixBoundaries)
                {
                    if (itemsLayoutMatrix[cloudCell[0] + k, cloudCell[1]  + u] == item)
                    {
                        int[] nextCloudCell = {cloudCell[0] + k, cloudCell[1]  + u};
                        ReAssignCloudItem(nextCloudCell, item, newCloudItem);
                    }
                }
            }
        }
    }


//UNDO MECHANIC

public void LoadLevelStateMatrixManager(int[,] _itemsLayoutMatrix, int[,] _mechanicsLayoutMatrix)
{
    itemsLayoutMatrix = (int[,]) _itemsLayoutMatrix.Clone();
    mechanicsLayoutMatrix = (int[,]) _mechanicsLayoutMatrix.Clone();

    RefreshPjMovementMatrix();
    RefreshCloudsMovementMatrix();

    FromMatrixToGame.LoadLevelLayout();
}

//MATRIX UTILITIES

    public Vector3 FromMatrixIndexToWorld(Vector3 x) => FromMatrixIndexToWorld((int)x[0], (int)x[1]);
    public Vector3 FromMatrixIndexToWorld(Vector2 x) => FromMatrixIndexToWorld((int)x[0], (int)x[1]);
    public Vector3 FromMatrixIndexToWorld(float i, float j) => FromMatrixIndexToWorld((int)i, (int)j);
    public Vector3 FromMatrixIndexToWorld(int i, int j)
    {
        Vector3 worldPosition = new Vector3 (coordinatesOriginMatrix.x + j + (0.5f * tilemapsLevelLayout.GetGridCellSize()), coordinatesOriginMatrix.y - i + (0.5f * tilemapsLevelLayout.GetGridCellSize()), 0f);

        return worldPosition;
    }

    public int[] FromWorldToMatrixIndex(Vector3 worldPosition)
    {
        int[] matrixCoordinates = {(int)(coordinatesOriginMatrix.y - (worldPosition.y - (0.5f * tilemapsLevelLayout.GetGridCellSize()))), (int)(-(coordinatesOriginMatrix.x - (worldPosition.x - (0.5f * tilemapsLevelLayout.GetGridCellSize()))))};


        if(matrixCoordinates[0] < 0 || matrixCoordinates[0] > itemsLayoutMatrix.GetLength(0) || matrixCoordinates[1] < 0 || matrixCoordinates[1] > itemsLayoutMatrix.GetLength(1)) return null;
        return matrixCoordinates;
    }

    public int [] AddNumberToArray(int[] arrayToModify, int numberToAdd)
    {
        int[] result = new int[arrayToModify.GetLength(0) + 1];

        if(arrayToModify.GetLength(0) > 0)
        {
            for (int index = 0; index < arrayToModify.GetLength(0); index++)
            {
                result[index] = arrayToModify[index];
            }
        }

        result[arrayToModify.GetLength(0)] = numberToAdd;

        return result;
    }

    public int [] RemoveNumberFromArray(int[] arrayToModify, int numberToRemove)
    {
        if(arrayToModify.GetLength(0) > 0)
        {
            bool numberIsInArray = false;
            for (int index = 0; index < arrayToModify.GetLength(0); index++)
            {
                if(arrayToModify[index] == numberToRemove) numberIsInArray = true;
            }

            if(numberIsInArray)
            {
                int[] result = new int[arrayToModify.GetLength(0) - 1];
                bool numberReached = false;
        
                for (int index = 0; index < arrayToModify.GetLength(0); index++)
                {
                    if (arrayToModify[index] == numberToRemove)
                    {
                        numberReached = true;
                        continue;
                    }
                    if(!numberReached) result[index] = arrayToModify[index];
                    else result[index - 1] = arrayToModify[index];
                }

                return result;
            }
            else
            {
                int[] result = arrayToModify;
                return result;
            }
        }
        else 
        {
            int[] result = arrayToModify;
            return result;
        }
    }

    public GameObject [] AddGameObjectToArray(GameObject[] arrayToModify, GameObject gameObjectToAdd)
    {
        GameObject[] result = new GameObject[arrayToModify.GetLength(0) + 1];

        for (int index = 0; index < arrayToModify.GetLength(0); index++)
        {
            result[index] = arrayToModify[index];
        }

        result[arrayToModify.GetLength(0)] = Instantiate(gameObjectToAdd);

        return result;
    }

    public int [] RemoveFirstNumberFromArray(int[] arrayToModify)
    {
        int[] result = new int[arrayToModify.GetLength(0) - 1];

        if (result.Length > 0)
        {
            for (int index = 1; index < arrayToModify.GetLength(0); index++)
            {
                result[index-1] = arrayToModify[index];
            }
        }
        
        return result;
    }

    public int [] RemoveIndexFromArray(int[] arrayToModify, int removeIndex)
    {
        int[] result = new int[arrayToModify.GetLength(0) - 1];

        if (result.Length > 0)
        {
            for (int index = 0; index < arrayToModify.GetLength(0); index++)
            {
                if      (index <  removeIndex) result[index]     = arrayToModify[index];
                else if (index == removeIndex) continue;
                else if (index >  removeIndex) result[index - 1] = arrayToModify[index];
            }
        }
        
        return result;
    }

    public static int[,] TrimMatrix(int rowToRemove, int columnToRemove, int[,] originalArray)
    {
        int[,] result = new int[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

        for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
        {
            if (i == rowToRemove)
                continue;

            for (int k = 0, u = 0; k < originalArray.GetLength(1); k++)
            {
                if (k == columnToRemove)
                    continue;

                result[j, u] = originalArray[i, k];
                u++;
            }
            j++;
        }

        return result;
    }

    public static int[,] RemoveRowFromMatrix(int rowToRemove, int[,] originalArray)
    {
        int[,] result = new int[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

        for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
        {
            if (i == rowToRemove)
                continue;

            for (int k = 0; k < originalArray.GetLength(1); k++)
            {
                result[j, k] = originalArray[i, k];
            }
            j++;
        }

        return result;
    }

    public static int[,] RemoveColumnFromMatrix(int columnToRemove, int[,] originalArray)
    {
        int[,] result = new int[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

        for (int i = 0; i < originalArray.GetLength(0); i++)
        {
            for (int k = 0; k < originalArray.GetLength(1); k++)
            {
                result[i, k] = originalArray[i, k];
            }
        }

        return result;
    }

    void CreatePathNodesMatrix()
    {
        pathNodesMatrix = new PathNode[itemsLayoutMatrix.GetLength(0), itemsLayoutMatrix.GetLength(1)];

        for (int x = 0; x < itemsLayoutMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < itemsLayoutMatrix.GetLength(1); y++)
            {
                pathNodesMatrix[x,y] = new PathNode(x,y);
            }
        }
    }
}
