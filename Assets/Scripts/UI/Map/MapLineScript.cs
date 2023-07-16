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

    const float LENGHT = 644;
    const float PADDING = 48;

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
            // Uncomment to get the line to extend size when levels locked
            // TODO: Lines should get short length when previous level is unlocked, not current.
            
            // else
            // {
            //     IncreaseScaleToWholeLine();
            // }
        }catch{
            Debug.LogWarning("Error importing level on line for level: " + worldNumber + ", " + levelNumber);
            return;
        }   
    }

    private void IncreaseScaleToWholeLine()
    {
        float scaleModificator;
        Vector3 newScale;

        scaleModificator = (2 * PADDING) / LENGHT;
        newScale = new Vector3 (this.transform.localScale.x + scaleModificator, this.transform.localScale.y, this.transform.localScale.z);
        this.transform.localScale = newScale;
        Debug.Log("Increased");
    }

}

