using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTileBehavior : MonoBehaviour
{
    TileBehavior parentTile;
    SpriteRenderer mySpriteRenderer;

    bool[] adyacentTilesForShadow;
    protected TileSpritesBundle tileShadowsSpritesBundle;

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        parentTile = GetComponentInParent<TileBehavior>();
        
        adyacentTilesForShadow = parentTile.GetAdyacentTilesForShadow();
        tileShadowsSpritesBundle = parentTile.GetTileShadowsSpritesBundle();

        SelectCorrectSprite();
    }

    void SelectCorrectSprite()
    {
        //Select and modify sprite to match the position
        if (!adyacentTilesForShadow[0] && !adyacentTilesForShadow[1]) mySpriteRenderer.sprite = tileShadowsSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.None).sprite;
        if (!adyacentTilesForShadow[0] &&  adyacentTilesForShadow[1]) mySpriteRenderer.sprite = tileShadowsSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.One).sprite;
        if (!adyacentTilesForShadow[1] &&  adyacentTilesForShadow[0]){
            mySpriteRenderer.sprite = tileShadowsSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.One).sprite;
            mySpriteRenderer.flipX = true;
        } 
        if ( adyacentTilesForShadow[1] &&  adyacentTilesForShadow[0]) mySpriteRenderer.sprite = tileShadowsSpritesBundle.spritesList.Find(x => x.boundaries == SpriteBoundaries.TwoMiddle).sprite;
    }
}
