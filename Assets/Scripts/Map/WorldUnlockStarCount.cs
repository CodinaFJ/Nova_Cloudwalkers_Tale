using System.Collections;
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
    TextMeshProUGUI countText;

    void Start()
    {
        countText = GetComponent<TextMeshProUGUI>();

        string starCollectedNumberText;

        starCollectedNumberText = GameProgressManager.instance.GetCollectedStarsInGame().ToString();/*(Mathf.Clamp(14 - GameState.instance.totalCollectedStars, 0, 14)).ToString();*/

        countText.text = starCollectedNumberText + "/" + starsToOpen;

        if(GameProgressManager.instance.GetCollectedStarsInGame() >= starsToOpen)
        {
            levelToUnlock.UnlockLevel();
            world2Lock.sprite = openedLock;
            counter.SetActive(false);
        }
    }
}