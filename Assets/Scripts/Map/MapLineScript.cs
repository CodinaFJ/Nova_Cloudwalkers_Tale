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

        if(initialLevel2 && GameProgressManager.instance.GetCollectedStarsInGame() >= 14) myImage.sprite = filledLine;

        else if(GameProgressManager.instance.GetLevel(worldNumber, levelNumber).GetLevelCompleted()) myImage.sprite = filledLine;        
    }

}
