using System.Collections.Generic;
using UnityEngine;

public class AssetsRepository : MonoBehaviour
{
    public static AssetsRepository instance;

    [SerializeField]
    List<WorldAssets> worldAssetsList = new List<WorldAssets>(4);

    [SerializeField]
    List<TileSpritesBundle> cloudTileSpritesBundles;

    private void Awake() {
        if(instance == null)
           instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public TileSpritesBundle GetSpritesBundle(TileType tileType, int worldNumber) => GetSpritesBundle(tileType, worldNumber, false);
    public TileSpritesBundle GetSpritesBundle(TileType tileType, int worldNumber, bool shadowTiles){
        if(tileType == TileType.Floor || tileType == TileType.SpikedFloor || tileType == TileType.CrystalFloor)
            return worldAssetsList.Find(x => x.worldNumber == worldNumber).floorTileSpritesBundles.Find(x => x.tileType == tileType && x.shadowTiles == shadowTiles);

        else
            return cloudTileSpritesBundles.Find(x => x.tileType == tileType && x.shadowTiles == shadowTiles);
    }
}
