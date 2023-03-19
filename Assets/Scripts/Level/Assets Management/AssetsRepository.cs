using System.Collections.Generic;
using UnityEngine;

public class AssetsRepository : MonoBehaviour
{
    public static AssetsRepository instance;

    [SerializeField]
    List<WorldAssets> worldAssetsList = new List<WorldAssets>(4);

    [SerializeField]
    List<TileSpritesBundle> cloudTileSpritesBundles;

    [SerializeField]
    List<ParticlesVFX> particleSystemList;

    private void Awake() {
        if(instance == null)
           instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public TileSpritesBundle GetSpritesBundle(TileType tileType, int worldNumber) => GetSpritesBundle(tileType, worldNumber, false);
    public TileSpritesBundle GetSpritesBundle(TileType tileType, int worldNumber, bool shadowTiles){
        if(tileType == TileType.Floor || tileType == TileType.SpikedFloor || tileType == TileType.CrystalFloor)
        {
            if (worldNumber == 0)
                worldNumber = 1;
            return worldAssetsList.Find(x => x.worldNumber == worldNumber).floorTileSpritesBundles.Find(x => x.tileType == tileType && x.shadowTiles == shadowTiles);
        }

        else
            return cloudTileSpritesBundles.Find(x => x.tileType == tileType && x.shadowTiles == shadowTiles);
    }

    public Sprite GetBackgroundImage(int worldNumber){
        try{return worldAssetsList.Find(x => x.worldNumber == worldNumber).backgroundImage;}
        catch{
            Debug.LogWarning("Error retrieving background image");
            return worldAssetsList.Find(x => x.worldNumber == 1).backgroundImage;
        }
    }

    public ParticlesVFX GetParticlesVFX(ParticlesVFXType type) => particleSystemList.Find(x => x.type == type);
}
