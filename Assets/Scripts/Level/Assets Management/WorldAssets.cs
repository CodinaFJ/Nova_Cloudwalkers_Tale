using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldAssets
{
    [Header("World Info")]
    [SerializeField][Range(1,4)]
    public int worldNumber = 1;
    
    /*[Header("Music")]
    public AudioClip music;
    public AudioClip ambientMusic;*/

    [Header("Visuals")]
    public Sprite backgroundImage;

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

public enum TileType{
    Floor, CrystalFloor, SpikedFloor,
    WhiteCloud, GreyCloud, CrystalCloudBot, CrystalCloudTop, ThunderCloud
}

public enum SpriteBoundaries{
    None, One, TwoCorner, TwoMiddle, Three, Four, Null
}