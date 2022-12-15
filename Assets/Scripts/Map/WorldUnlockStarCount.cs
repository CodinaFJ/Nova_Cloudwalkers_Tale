using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldUnlockStarCount : MonoBehaviour
{
    [SerializeField] Sprite openedLock;
    [SerializeField] Image world2Lock;
    [SerializeField] GameObject counter;
    [SerializeField] int starsToOpen;
    [SerializeField] levelButton levelToUnlock;
    [SerializeField] int worldToUnlock;
    TextMeshProUGUI countText;
    bool cinematicPlayed = false;

    bool locked = true;

    void Start()
    {
        countText = GetComponent<TextMeshProUGUI>();

        string starCollectedNumberText;

        starCollectedNumberText = GameProgressManager.instance.GetCollectedStarsInGame().ToString();

        countText.text = starCollectedNumberText + "/" + starsToOpen;

        if(GameProgressManager.instance.GetCollectedStarsInGame() >= starsToOpen)
        {
            levelToUnlock.UnlockLevel();
            world2Lock.sprite = openedLock;
            counter.SetActive(false);
        }
    }
}