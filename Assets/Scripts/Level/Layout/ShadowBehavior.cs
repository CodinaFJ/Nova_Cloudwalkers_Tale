using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBehavior : MonoBehaviour
{

    [SerializeField] Sprite shadowTile1;
    [SerializeField] Sprite shadowTile2Parallel;
    [SerializeField] Sprite shadowTile2CornerL;
    [SerializeField] Sprite shadowTile2CornerR;

    InstantiatedTileBehavior parentTile;
    InstantiatedCloudBehavior parentCloud;
    bool tileCloud = false;

    SpriteRenderer mySpriteRenderer;

    bool[] adyacentTilesForShadow;

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        parentTile = GetComponentInParent<InstantiatedTileBehavior>();
        if(parentTile == null)
        {
            tileCloud = true;
            parentCloud = GetComponentInParent<InstantiatedCloudBehavior>();
        } 

        //Needed to select sprite
        if(!tileCloud) adyacentTilesForShadow = parentTile.GetAdyacentTilesForShadow();
        else adyacentTilesForShadow = parentCloud.GetAdyacentTilesForShadow();

        SelectCorrectSprite();
    }

    void SelectCorrectSprite()
    {
        //Select and modify sprite to match the position
        if (!adyacentTilesForShadow[0] && !adyacentTilesForShadow[1]) mySpriteRenderer.sprite = shadowTile1;
        if (!adyacentTilesForShadow[0] &&  adyacentTilesForShadow[1]) mySpriteRenderer.sprite = shadowTile2CornerL;
        if (!adyacentTilesForShadow[1] &&  adyacentTilesForShadow[0])
        {
            mySpriteRenderer.sprite = shadowTile2CornerL;
            mySpriteRenderer.flipX = true;
        } 
        if ( adyacentTilesForShadow[1] &&  adyacentTilesForShadow[0]) mySpriteRenderer.sprite = shadowTile2Parallel;
    }
}
