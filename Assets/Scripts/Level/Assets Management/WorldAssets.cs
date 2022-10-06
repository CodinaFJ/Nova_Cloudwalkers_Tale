using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldAssets
{
    [Header("World Info")]
    [SerializeField][Range(1,4)]
    public int worldNumber = 1;
    
    [Header("Music")]
    public AudioClip music;
    public AudioClip ambientMusic;

    [Header("Visuals")]
    public Sprite backgroundImage;

    public List<FloorTileSpritesBundle> floorTileSpritesBundles = new List<FloorTileSpritesBundle>(5);
}

public enum SpriteBoundaries{
    None, One, TwoCorner, TwoMiddle, Three, Four, Null
}

[System.Serializable]
public struct OrientedSprite{
    public SpriteBoundaries boundaries;
    public Sprite sprite;
}

public enum FloorTileType{
    Floor, CrystalFloor, SpikedFloor
}

public enum CloudTileType{
    WhiteCloud, GreyCloud, CrystalCloudBot, CrystalCloudTop, ThunderCloud
}

[System.Serializable]
public struct FloorTileSpritesBundle{
    public FloorTileType floorTileType;
    public bool shadowTiles;
    public List<OrientedSprite> spritesList;
}

[System.Serializable]
public struct CloudTileSpritesBundle{
    public CloudTileType cloudTileType;
    public bool shadowTiles;
    public List<OrientedSprite> spritesList;
}
