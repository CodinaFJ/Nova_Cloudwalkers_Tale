using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapButtonStarsCounter : MonoBehaviour
{
    TextMeshProUGUI buttonText;
    levelButton LevelButton;

    Level level;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponent<TextMeshProUGUI>();
        LevelButton = GetComponentInParent<levelButton>();

        level = GameProgressManager.instance.GetLevel(LevelButton.GetWorldNumber(), LevelButton.GetLevelNumber());

        buttonText.text = level.GetCollectedStars() + "/" + level.GetNumberOfStars();
    }
}
