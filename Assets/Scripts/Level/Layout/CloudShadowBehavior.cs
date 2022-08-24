using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CloudShadowBehavior : MonoBehaviour
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

        parentCloud = GetComponentInParent<InstantiatedCloudBehavior>();
        if(parentCloud == null) parentTile = GetComponentInParent<InstantiatedTileBehavior>();

        //Needed to select sprite
        if(parentCloud != null) adyacentTilesForShadow = parentCloud.GetAdyacentTilesForShadow();
        else adyacentTilesForShadow = parentTile.GetAdyacentTilesForShadow();

        SelectCorrectSprite();
    }

    void SelectCorrectSprite()
    {
        //Select and modify sprite to match the position

        Transform parent;

        if(parentCloud != null) parent = parentCloud.transform;
        else parent = parentTile.transform;

        transform.localScale = parent.localScale;
        if (!adyacentTilesForShadow[0] && !adyacentTilesForShadow[1]) mySpriteRenderer.sprite = shadowTile1;
        if (!adyacentTilesForShadow[0] &&  adyacentTilesForShadow[1])
        {
            mySpriteRenderer.sprite = shadowTile2CornerL;
            mySpriteRenderer.flipX = false;
        }
        if (!adyacentTilesForShadow[1] &&  adyacentTilesForShadow[0])
        {
            mySpriteRenderer.sprite = shadowTile2CornerL;
            mySpriteRenderer.flipX = true;
        } 
        if ( adyacentTilesForShadow[1] &&  adyacentTilesForShadow[0]) mySpriteRenderer.sprite = shadowTile2Parallel;
    }
}
