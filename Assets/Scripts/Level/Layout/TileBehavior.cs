using UnityEngine;
using System.Collections.Generic;

public class TileBehavior : MonoBehaviour
{
    [SerializeField]
    protected TileType tileType;

    [Header("Tile Shadow")]
    [SerializeField] 
    GameObject tileShadow;
    [SerializeField] 
    float shadowToWorldCorrection = 0.85f;
    [SerializeField]
    GameObject movementParticlesPrefab;
    [SerializeField]
    GameObject joinParticlesPrefab;

    [Header("Cloud union")]
    [SerializeField] GameObject MixedCloudPrefab;
    [SerializeField] List<UnionSprite> ThunderUnionSprites;
    [SerializeField] List<UnionSprite> CrystalUnionSprites;
    [SerializeField] GameObject ThunderUnionShadowSprite;
    [SerializeField] GameObject CrystalUnionShadowSprite;
    
    GameObject myMovementParticles;
    GameObject myJoinParticles;

    SpriteRenderer mySpriteRenderer;

    protected int[] matrixCoordinates;

    MatrixManager matrixManager;
    int[,] itemsLayoutMatrix;
    int[,] mechanicsLayoutMatrix;
    int itemNumber;
    int mechanicNumber;

    // 0-up, 1-left, 2-down, 3-right
    bool[] adyacentTiles = new bool[8];
    bool[] adyacentTilesForShadow = new bool[5];

    [SerializeField]
    int adyacentTilesNumber = 0;

    protected TileSpritesBundle tileSpritesBundle;
    protected TileSpritesBundle tileShadowsSpritesBundle;

    struct LinkTransform{
        public Vector2 position;
        public int rotation;
        public bool flipY;
    }

    [System.Serializable]
    struct UnionSprite
    {
        public Sprite sprite;
        public UnionSpriteTile unionSpriteTile;
    }

    enum UnionSpriteTile
    {
        Middle_W50_O50,
        Side_W50_O50,
        Solo_W50_O50,
        Corner_W75_O25,
        Corner_W25_O75
    }

    private void Start() {
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        myMovementParticles = Instantiate(movementParticlesPrefab, transform.position, Quaternion.identity, this.transform);
        myMovementParticles.GetComponent<ParticleSystem>().Play();

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

    private void SetCorrectSprites()
    {
        if(tileType != TileType.CrystalFloor){
            SetAdyacentTiles();
            SetAndOrientateSprites();
        }
        else SetCrystalFloorTile();

        SetAdyacentTilesForShadow();
        InstantiateShadow();
        InstantiateMixedCloudJoint(adyacentTiles);
    }

    public void SetCorrectSpritesAfterCrack(){
        foreach(Transform childShadow in this.transform){
            if(childShadow.GetComponent<ParticleSystem>() != null) continue;
            childShadow.gameObject.SetActive(false);
            Destroy(childShadow.gameObject);
        }

        SetAdyacentTilesForShadow();
        InstantiateShadow();
        InstantiateMixedCloudJoint(adyacentTiles);
    }


    protected virtual void SetAdyacentTiles(){
        // 0-North, 1-West, 2-South, 3-East
        adyacentTiles = new bool[8];

        for (int index = 0; index < adyacentTiles.Length; index ++){
            adyacentTiles[index] = false;
        }

        adyacentTilesNumber = 0;

        int numberToCompare = AssignNumberToCompare();
        
        for (int i = -1; i < 2; i++){
            for (int j = -1; j < 2; j++){
                if (Mathf.Abs(i) + Mathf.Abs(j) != 0 && CheckCoordinatesInMatrix(i,j) && AssignMatrixToCompare()[matrixCoordinates[0] + i, matrixCoordinates[1] + j] == numberToCompare)
                    FillAdyacentTiles(i,j);
            }
        }

        for (int index = 0; index < 4; index ++)
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
        if ( i == -1 && j ==  0) adyacentTiles[(int) adyacentTile.N] =  true;
        if ( i ==  0 && j == -1) adyacentTiles[(int) adyacentTile.W] =  true;
        if ( i ==  1 && j ==  0) adyacentTiles[(int) adyacentTile.S] =  true;
        if ( i ==  0 && j ==  1) adyacentTiles[(int) adyacentTile.E] =  true;
        if ( i == -1 && j == -1) adyacentTiles[(int) adyacentTile.NW] =  true;
        if ( i ==  1 && j == -1) adyacentTiles[(int) adyacentTile.SW] =  true;
        if ( i == -1 && j ==  1) adyacentTiles[(int) adyacentTile.NE] =  true;
        if ( i ==  1 && j ==  1) adyacentTiles[(int) adyacentTile.SE] =  true;
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
               transform.Rotate(0,0,-90,Space.Self);
               mySpriteRenderer.flipY = true;
            }
            else if(adyacentTiles[2]) transform.Rotate(0,0,90,Space.Self);
            else if(adyacentTiles[3]) mySpriteRenderer.flipX = true;
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

                if(!adyacentTiles[0]) transform.Rotate(0,0,  180,Space.Self);
                else if (!adyacentTiles[1]) transform.Rotate(0,0,-90,Space.Self);
                else if (!adyacentTiles[2]) transform.Rotate(0,0, 0,Space.Self);
                else if (!adyacentTiles[3]) transform.Rotate(0,0, 90,Space.Self);
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
        if (!adyacentTilesForShadow[3] || tileType == TileType.CrystalFloor) Instantiate(tileShadow, transform.position + new Vector3(0, -shadowToWorldCorrection, 0), Quaternion.identity, gameObject.transform);
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
        tileSpritesBundle = AssetsRepository.instance.GetSpritesBundle(tileType, GameProgressManager.instance.GetActiveWorld().GetLevelWorldNumber());
        tileShadowsSpritesBundle = AssetsRepository.instance.GetSpritesBundle(tileType, GameProgressManager.instance.GetActiveWorld().GetLevelWorldNumber(), true);
    }


    public int[] GetTileCoordinates() => matrixCoordinates;
    public bool[] GetAdyacentTilesForShadow() => adyacentTilesForShadow;
    public TileSpritesBundle GetTileShadowsSpritesBundle() => tileShadowsSpritesBundle;

    public static TileType MechanicNumberToTileType(int mechanicNumber){
        switch(mechanicNumber){
            case 1:
                return TileType.WhiteCloud;
            
            case 2:
                break;

            case 3:
                return TileType.CrystalCloudTop;

            case 4:
                return TileType.CrystalCloudTop;

            case -1:
                return TileType.ThunderCloud;

            default:
                return TileType.WhiteCloud;
        }
        return TileType.WhiteCloud;
    }

    private void InstantiateMixedCloudJoint(bool[] adyacentTiles)
    {
        LinkTransform linkTransform;
        int adyacentTileMechanic;
        List<UnionSprite> unionSpritesSet;
        GameObject instantiatedUnion;
        SpriteRenderer unionSpriteRenderer;

        if (tileType != TileType.WhiteCloud) return;
        for (int i = 0; i < 4; i++)
        {
            linkTransform = FromAdyacentIndexToTranform(i);
            if (!adyacentTiles[i])
                continue;
            adyacentTileMechanic = mechanicsLayoutMatrix[matrixCoordinates[0] - (int) linkTransform.position.y, matrixCoordinates[1] + (int) linkTransform.position.x];
            unionSpritesSet = new List<UnionSprite>(SelectUnionCloudSpritesType(adyacentTileMechanic));
            if (unionSpritesSet.Count == 0)
                continue;
            instantiatedUnion = Instantiate(MixedCloudPrefab, (linkTransform.position * 0.5f) + (Vector2) transform.position, Quaternion.identity, gameObject.transform);
            instantiatedUnion.transform.Rotate(0, 0, linkTransform.rotation);
            unionSpriteRenderer = instantiatedUnion.GetComponent<SpriteRenderer>();
            unionSpriteRenderer.sprite = SelectCorrectUnionSprite_50_50(i, unionSpritesSet);
            unionSpriteRenderer.flipY = linkTransform.flipY;
            InstantiateUnionShadow(adyacentTileMechanic, i);
        }
    }

    private void InstantiateUnionShadow(int adyacentTileMechanic, int i)
    {
        GameObject shadowSprite;
        GameObject instantiated;
        if (adyacentTileMechanic == -1)
            shadowSprite = ThunderUnionShadowSprite;
        else if (adyacentTileMechanic == 3 || adyacentTileMechanic == 4)
            shadowSprite = CrystalUnionShadowSprite;
        else
            return;

        if (i == (int) adyacentTile.E)
        {
            instantiated = Instantiate(shadowSprite, transform.position + new Vector3(0.5f, -shadowToWorldCorrection, 0), Quaternion.identity, gameObject.transform);
        }
        else if (i == (int) adyacentTile.W)
        {
            instantiated = Instantiate(shadowSprite, transform.position + new Vector3(-0.5f, -shadowToWorldCorrection, 0), Quaternion.identity, gameObject.transform);
            instantiated.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private LinkTransform FromAdyacentIndexToTranform(int i)
    {
        LinkTransform linkTransform = new LinkTransform();

        switch (i)
        {
            case (int) adyacentTile.N:
            linkTransform.position.x = 0;
            linkTransform.position.y = 1;
            linkTransform.rotation = 90;
            if (!adyacentTiles[(int) adyacentTile.NE] && adyacentTiles[(int) adyacentTile.NW])
                linkTransform.flipY = true;
            else
                linkTransform.flipY = false;
            break;

            case (int) adyacentTile.W:
            linkTransform.position.x = -1;
            linkTransform.position.y = 0;
            linkTransform.rotation = 180;
            if (!adyacentTiles[(int) adyacentTile.NW] && adyacentTiles[(int) adyacentTile.SW])
                linkTransform.flipY = true;
            else
                linkTransform.flipY = false;
            break;

            case (int) adyacentTile.S:
            linkTransform.position.x = 0;
            linkTransform.position.y = -1;
            linkTransform.rotation = -90;
            if (adyacentTiles[(int) adyacentTile.SE] && !adyacentTiles[(int) adyacentTile.SW])
                linkTransform.flipY = true;
            else
                linkTransform.flipY = false;
            break;

            case (int) adyacentTile.E:
            linkTransform.position.x = 1;
            linkTransform.position.y = 0;
            linkTransform.rotation = 0;
            if (adyacentTiles[(int) adyacentTile.NE] && !adyacentTiles[(int) adyacentTile.SE])
                linkTransform.flipY = true;
            else
                linkTransform.flipY = false;
            break;
        }
        return linkTransform;
    }

    private List<UnionSprite> SelectUnionCloudSpritesType(int adyacentTileMechanic)
    {
        if (adyacentTileMechanic == -1)
            return ThunderUnionSprites;
        else if (adyacentTileMechanic == 3 || adyacentTileMechanic == 4)
            return CrystalUnionSprites;
        return new List<UnionSprite>();
    }

    private Sprite SelectCorrectUnionSprite_50_50(int adyacentIndex, List<UnionSprite> unionSpritesSet)
    {
        for (int i = 0; i < 8; i++)
        {
            Debug.Log("i(" + i + ") " + adyacentTiles[i]);
        }
        if (unionSpritesSet == null) return null;
        if (adyacentIndex == (int)adyacentTile.N)
        {
            if (adyacentTiles[(int) adyacentTile.NW] && adyacentTiles[(int) adyacentTile.NE])
                return unionSpritesSet.Find(x => x.unionSpriteTile == UnionSpriteTile.Middle_W50_O50).sprite;
            else if (adyacentTiles[(int) adyacentTile.NW] || adyacentTiles[(int) adyacentTile.NE])
                return unionSpritesSet.Find(x => x.unionSpriteTile == UnionSpriteTile.Side_W50_O50).sprite;
        }
        else if (adyacentIndex == (int)adyacentTile.E)
        {
            if (adyacentTiles[(int) adyacentTile.SE] && adyacentTiles[(int) adyacentTile.NE])
                return unionSpritesSet.Find(x => x.unionSpriteTile == UnionSpriteTile.Middle_W50_O50).sprite;
            else if (adyacentTiles[(int) adyacentTile.SE] || adyacentTiles[(int) adyacentTile.NE])
                return unionSpritesSet.Find(x => x.unionSpriteTile == UnionSpriteTile.Side_W50_O50).sprite;
        }
        else if (adyacentIndex == (int)adyacentTile.S)
        {
            if (adyacentTiles[(int) adyacentTile.SW] && adyacentTiles[(int) adyacentTile.SE])
                return unionSpritesSet.Find(x => x.unionSpriteTile == UnionSpriteTile.Middle_W50_O50).sprite;
            else if (adyacentTiles[(int) adyacentTile.SW] || adyacentTiles[(int) adyacentTile.SE])
                return unionSpritesSet.Find(x => x.unionSpriteTile == UnionSpriteTile.Side_W50_O50).sprite;
        }
        else if (adyacentIndex == (int)adyacentTile.W)
        {
            if (adyacentTiles[(int) adyacentTile.NW] && adyacentTiles[(int) adyacentTile.SW])
                return unionSpritesSet.Find(x => x.unionSpriteTile == UnionSpriteTile.Middle_W50_O50).sprite;
            else if (adyacentTiles[(int) adyacentTile.NW] || adyacentTiles[(int) adyacentTile.SW])
                return unionSpritesSet.Find(x => x.unionSpriteTile == UnionSpriteTile.Side_W50_O50).sprite;
        }
        return unionSpritesSet.Find(x => x.unionSpriteTile == UnionSpriteTile.Solo_W50_O50).sprite;
    }

    private Sprite SelectCorrectUnionSprite_Corners(int adyacentIndex)
    {
        return null;
    }
}

// 0-North, 1-West, 2-South, 3-East
public enum adyacentTile{
    N = 0,
    W = 1,
    S = 2,
    E = 3,
    NE = 4,
    SE = 5,
    SW = 6,
    NW = 7
}
