using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsRepository : MonoBehaviour
{
    public static AssetsRepository instance;

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

    [SerializeField]
    List<WorldAssets> worldAssetsList = new List<WorldAssets>(4);

    [SerializeField]
    List<TileSpritesBundle> cloudTileSpritesBundles;

    public TileSpritesBundle GetSpritesBundle(TileType tileType, int worldNumber){
        return worldAssetsList.Find(x => x.worldNumber == worldNumber).floorTileSpritesBundles.Find(x => x.tileType == tileType && x.shadowTiles == false);
    }
}
