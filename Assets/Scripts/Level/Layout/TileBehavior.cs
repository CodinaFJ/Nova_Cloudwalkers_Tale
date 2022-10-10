using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    [SerializeField]
    protected TileType tileType;

    [Header("Tile Shadow")]
    [SerializeField] 
    GameObject tileShadow;
    [SerializeField] 
    float shadowToWorldCorrection = 0.85f;

    SpriteRenderer mySpriteRenderer;

    protected int[] matrixCoordinates;

    MatrixManager matrixManager;
    int[,] itemsLayoutMatrix;
    int[,] mechanicsLayoutMatrix;
    int itemNumber;
    int mechanicNumber;

    // 0-up, 1-left, 2-down, 3-right
    bool[] adyacentTiles = new bool[4];
    bool[] adyacentTilesForShadow = new bool[5];

    [SerializeField]
    int adyacentTilesNumber = 0;

    protected TileSpritesBundle tileSpritesBundle;
    protected TileSpritesBundle tileShadowsSpritesBundle;

    private void Start() {
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        InitilizeTileInfo();
        SetCorrectSprites();
    }

    protected void InitilizeTileInfo()
    {
        itemsLayoutMatrix = MatrixManager.instance.GetItemsLayoutMatrix();
        mechanicsLayoutMatrix = MatrixManager.instance.GetMechanicsLayoutMatrix();

        matrixCoordinates = MatrixManager.instance.FromWorldToMatrixIndex(transform.position);

        itemNumber = itemsLayoutMatrix[matrixCoordinates[0], matrixCoordinates[1]];
        mechanicNumber = mechanicsLayoutMatrix[matrixCoordinates[0], matrixCoordinates[1]];

        SetTileSpritesBundles();
    }

    protected void SetCorrectSprites()
    {
        if(tileType != TileType.CrystalFloor){
            SetAdyacentTiles();
            SetAndOrientateSprites();
        }
        else SetCrystalFloorTile();

        SetAdyacentTilesForShadow();
        InstantiateShadow();
    }


    protected virtual void SetAdyacentTiles(){
        // 0-North, 1-West, 2-South, 3-East
        adyacentTiles = new bool[4];

        for (int index = 0; index < adyacentTiles.Length; index ++){
            adyacentTiles[index] = false;
        }

        adyacentTilesNumber = 0;

        int numberToCompare = AssignNumberToCompare();
        
        for (int i = -1; i < 2; i++){
            for (int j = -1; j < 2; j++){
                //Mathf.Abs(i + j) == 1 used to limit the checked tiles to the one at the right/left/up/down
                if (Mathf.Abs(i + j) == 1 && CheckCoordinatesInMatrix(i,j) && AssignMatrixToCompare()[matrixCoordinates[0] + i, matrixCoordinates[1] + j] == numberToCompare)
                    FillAdyacentTiles(i,j);
            }
        }

        for (int index = 0; index < adyacentTiles.Length; index ++)
            if(adyacentTiles[index])adyacentTilesNumber++;
    }

    private void SetAdyacentTilesForShadow(){
        //For the shadows we check the immediate left/right tiles and the three tiles under the left/self/right tile
        //0-left, 1-right, 2-left/down, 3-down, 4-right/down
        for (int index = 0; index < adyacentTilesForShadow.Length; index ++){
            adyacentTilesForShadow[index] = false;
        }

        int numberToCompare = AssignNumberToCompareForShadow();

        for (int i = 0; i < 2; i++){
            for (int j = -1; j < 2; j++){
                if (CheckCoordinatesInMatrix(i,j) && Mathf.Abs(itemsLayoutMatrix[matrixCoordinates[0] + i, matrixCoordinates[1] + j]) == numberToCompare){
                    FillAdyacentShadowTiles(i, j);
                }
            }
        }
    }

    protected int AssignNumberToCompare(){
        if(tileType == TileType.Floor)
            return Mathf.Abs(itemNumber);

        else if(tileType == TileType.SpikedFloor)
            return mechanicNumber;
        
        else
            return itemNumber;
    }

    protected int[,] AssignMatrixToCompare(){
        if(tileType == TileType.SpikedFloor)
            return mechanicsLayoutMatrix;
        
        else
            return itemsLayoutMatrix;
    }

    protected int AssignNumberToCompareForShadow(){
        if(tileType == TileType.Floor || tileType == TileType.SpikedFloor)
            return Mathf.Abs(itemNumber);        
        else
            return itemNumber;
    }

    protected void FillAdyacentTiles(int i, int j){
        if ( i == -1 && j ==  0) adyacentTiles[0] =  true;
        if ( i ==  0 && j == -1) adyacentTiles[1] =  true;
        if ( i ==  1 && j ==  0) adyacentTiles[2] =  true;
        if ( i ==  0 && j ==  1) adyacentTiles[3] =  true;
    }

    protected void FillAdyacentShadowTiles(int i, int j){
             if ( i ==  0 && j == -1) adyacentTilesForShadow[0] =  true;
        else if ( i ==  0 && j ==  1) adyacentTilesForShadow[1] =  true;
        else if ( i ==  1 && j == -1) adyacentTilesForShadow[2] =  true;
        else if ( i ==  1 && j ==  0) adyacentTilesForShadow[3] =  true;
        else if ( i ==  1 && j ==  1) adyacentTilesForShadow[4] =  true;
    }


    void SetCrystalFloorTile(){
        mySpriteRenderer.sprite = tileSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.None).sprite;
        transform.Rotate(0,0,0,Space.Self);
        mySpriteRenderer.flipX = false;
        mySpriteRenderer.flipY = false;
    }

    protected void SetAndOrientateSprites()
    {
        //Use adyacentTilesNumber to do a first filter to select the sprite based on how many other cloud tiles are connected to this one.
        //Then sprite is rotated or fliped to match the postion
        if(adyacentTilesNumber==0){
            mySpriteRenderer.sprite = tileSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.None).sprite;
        } 

        else if(adyacentTilesNumber==1) {
            mySpriteRenderer.sprite = tileSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.One).sprite;

            if(adyacentTiles[0]){
               transform.Rotate(0,0,90,Space.Self);
               mySpriteRenderer.flipY = true;
            }
            else if(adyacentTiles[2]) transform.Rotate(0,0,-90,Space.Self);
            else if(adyacentTiles[1]) mySpriteRenderer.flipX = true;
        }

        else if(adyacentTilesNumber==2){
            if(adyacentTiles[0] && adyacentTiles[2]){
                mySpriteRenderer.sprite = tileSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.TwoMiddle).sprite;
                transform.Rotate(0,0,90,Space.Self);
            }
            else if(adyacentTiles[1] && adyacentTiles[3]) mySpriteRenderer.sprite = tileSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.TwoMiddle).sprite;
            
            else{
                mySpriteRenderer.sprite = tileSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.TwoCorner).sprite;

                if     (adyacentTiles[0] && adyacentTiles[1]) transform.Rotate(0,0,180,Space.Self);
                else if(adyacentTiles[1] && adyacentTiles[2]) mySpriteRenderer.flipX = true;
                //   if(adyacentTiles[2] && adyacentTiles[3]) transform.Rotate(0,0,  0,Space.Self); Initial position
                else if(adyacentTiles[3] && adyacentTiles[0]) mySpriteRenderer.flipY = true;
            }
        }

        else if(adyacentTilesNumber==3){
            mySpriteRenderer.sprite = tileSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.Three).sprite;

                //   if(!adyacentTiles[0]) transform.Rotate(0,0,  0,Space.Self); Initial Position
                if     (!adyacentTiles[1]) transform.Rotate(0,0, 90,Space.Self);
                else if(!adyacentTiles[2]) transform.Rotate(0,0,180,Space.Self);
                else if(!adyacentTiles[3]) transform.Rotate(0,0,-90,Space.Self);
        }
    
        else if(adyacentTilesNumber==4) mySpriteRenderer.sprite = tileSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.Four).sprite;

        else Debug.Log("Error in adyacent tiles");


        if(mySpriteRenderer.sprite == null){
            mySpriteRenderer.sprite = tileSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.None).sprite;
            transform.Rotate(0,0, 0,Space.Self);
            mySpriteRenderer.flipX = false;
            mySpriteRenderer.flipY = false;
        } 
    }

    
    private void InstantiateShadow(){
        if(tileType == TileType.SpikedFloor) return;
        if(tileType == TileType.CrystalCloudTop) return;

        //Instantiate needed shadows
        //Always instantiate if tile just below is empty
        if (!adyacentTilesForShadow[3]) Instantiate(tileShadow, transform.position + new Vector3(0, -shadowToWorldCorrection, 0), Quaternion.identity, gameObject.transform);
        //Always instantiate if there is a cloud tile at the side that does not have a cloud tile below
        else if(adyacentTilesForShadow[0] && !adyacentTilesForShadow[2]) Instantiate(tileShadow, transform.position + new Vector3(0, -shadowToWorldCorrection, 0), Quaternion.identity, gameObject.transform);
        else if(adyacentTilesForShadow[1] && !adyacentTilesForShadow[4]) Instantiate(tileShadow, transform.position + new Vector3(0, -shadowToWorldCorrection, 0), Quaternion.identity, gameObject.transform);
    }
    

    protected bool CheckCoordinatesInMatrix(int i, int j){
        bool indexInMatrixBoundaries = matrixCoordinates[0] + i > 0                              && matrixCoordinates[1] + j > 0 &&
                                       matrixCoordinates[0] + i < itemsLayoutMatrix.GetLength(0) && matrixCoordinates[1] + j < itemsLayoutMatrix.GetLength(1);

        return indexInMatrixBoundaries;
    }

    protected void SetTileSpritesBundles(){
        tileSpritesBundle = AssetsRepository.instance.GetSpritesBundle(tileType, LevelInfo.instance.GetLevelWorldNumber());
        tileShadowsSpritesBundle = AssetsRepository.instance.GetSpritesBundle(tileType, LevelInfo.instance.GetLevelWorldNumber(), true);
    }


    public bool[] GetAdyacentTilesForShadow() => adyacentTilesForShadow;
    public TileSpritesBundle GetTileShadowsSpritesBundle() => tileShadowsSpritesBundle;

}
