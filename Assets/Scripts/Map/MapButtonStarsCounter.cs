using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapButtonStarsCounter : MonoBehaviour
{
    TextMeshProUGUI buttonText;
    levelButton LevelButton;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponent<TextMeshProUGUI>();
        LevelButton = GetComponentInParent<levelButton>();

        if(LevelButton.worldNumber == 1)
        {
            buttonText.text = GameState.instance.collectedStarsInLevelsWorld1[LevelButton.levelNumber - 1] + "/" 
                            + GameState.instance.totalStarsInLevelsWorld1[LevelButton.levelNumber - 1];
        }
        else if(LevelButton.worldNumber == 2)
        {
            buttonText.text = GameState.instance.collectedStarsInLevelsWorld2[LevelButton.levelNumber - 1] + "/" 
                            + GameState.instance.totalStarsInLevelsWorld2[LevelButton.levelNumber - 1];
        }
    }
}
