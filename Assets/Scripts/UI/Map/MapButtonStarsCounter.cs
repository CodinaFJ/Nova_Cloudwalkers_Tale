using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Sets text of each level in level selector: <collectedStars>/<numberOfStars>
/// </summary>
public class MapButtonStarsCounter : MonoBehaviour
{
    TextMeshProUGUI buttonText;
    levelButton     LevelButton;
    Level           level;

    void Start()
    {
        buttonText = GetComponent<TextMeshProUGUI>();
        LevelButton = GetComponentInParent<levelButton>();
        level = GameProgressManager.instance.GetLevel(LevelButton.GetWorldNumber(), LevelButton.GetLevelNumber());
        buttonText.text = level.GetCollectedStars() + "/" + level.GetNumberOfStars();
    }
}
