using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldAssets
{
    [Header("World Info")]
    [SerializeField][Range(1,4)]
    public int worldNumber = 1;

    [Header("Visuals")]
    public Sprite backgroundImage;
    public Sprite mainMenuImage;

    public List<TileSpritesBundle> floorTileSpritesBundles = new List<TileSpritesBundle>(5);
}

[System.Serializable]
public struct TileSpritesBundle{
    public TileType tileType;
    public bool shadowTiles;
    public List<TileableSprite> spritesList;
}

[System.Serializable]
public struct TileableSprite{
    public SpriteBoundaries boundaries;
    public Sprite sprite;
}

[System.Serializable]
public struct ParticlesVFX{
    public ParticlesVFXType type;
    public GameObject particlesPrefab;
}

public enum TileType{
    Floor, CrystalFloor, SpikedFloor,
    WhiteCloud, GreyCloud, CrystalCloudBot, CrystalCloudTop, ThunderCloud
}

public enum SpriteBoundaries{
    None, One, TwoCorner, TwoMiddle, Three, Four, Null
}

public enum ParticlesVFXType{
    CrystalFloorBreak, CrystalCloudBreak,
    ThundersInCloud,
    GreyCloudJoin
}