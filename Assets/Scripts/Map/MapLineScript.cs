using System.Collections;
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

        if(initialLevel2)
        {
            if(GameState.instance.totalCollectedStars >= 14)
            {
                myImage.sprite = filledLine;
            }
        }
        else if(worldNumber == 1)
        {
            if(GameState.instance.completedLevelsWorld1[levelNumber - 1])
            {
                myImage.sprite = filledLine;
            }
        }
        else if(worldNumber == 2)
        {
            if(GameState.instance.completedLevelsWorld2[levelNumber - 1])
            {
                myImage.sprite = filledLine;
            }
        }
        
    }

}
