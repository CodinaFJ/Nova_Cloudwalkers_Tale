using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImageSelector : MonoBehaviour
{
    SpriteRenderer mySpriteRenderer;

    void Start(){
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        mySpriteRenderer.sprite = AssetsRepository.instance.GetBackgroundImage(LevelInfo.instance.GetLevelWorldNumber());
    }
}
