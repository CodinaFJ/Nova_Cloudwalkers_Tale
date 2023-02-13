using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapLineScript : MonoBehaviour
{
    [SerializeField] Sprite filledLine;
    [SerializeField] int worldNumber;
    [SerializeField] int levelNumber;
    [SerializeField] bool initialLevel2 = false;

    Image myImage;

    void Start()
    {
        myImage = GetComponent<Image>();
        Level level;
        try{
            level = GameProgressManager.instance.GetLevel(worldNumber, levelNumber);
            //if(initialLevel2 && GameProgressManager.instance.GetCollectedStarsInGame() >= 14) myImage.sprite = filledLine;
            if(GameProgressManager.instance.GetLevel(worldNumber, levelNumber).GetLevelCompleted())
            {
                myImage.sprite = filledLine;
            }  
        }catch{
            Debug.LogWarning("Error importing level on line for level: " + worldNumber + ", " + levelNumber);
            return;
        }   
    }

}

