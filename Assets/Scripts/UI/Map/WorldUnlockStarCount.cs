using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldUnlockStarCount : MonoBehaviour
{
    [SerializeField] int starsToOpen;
    [SerializeField] levelButton levelToUnlock;
    [SerializeField] int worldToUnlock;
    [SerializeField] TextMeshProUGUI countText;
    bool locked = true;

    void Start()
    {
        if (GameProgressManager.instance.CheckUnlockedWorld(worldToUnlock))
        {
            Debug.Log("World is unlocked");
            GetComponentInParent<Button>().interactable = true;
            gameObject.SetActive(false);
            return ;
        }
        else
        {
            GetComponentInParent<Button>().interactable = false;
        }

        string starCollectedNumberText;
        starCollectedNumberText = GameProgressManager.instance.GetCollectedStarsInGame().ToString();
        countText.text = starCollectedNumberText + "/" + starsToOpen;
        if (GameProgressManager.instance.GetCollectedStarsInGame() >= starsToOpen)
        {
            GetComponentInParent<Button>().interactable = true;
            GameProgressManager.instance.UnlockWorld(worldToUnlock);
            levelToUnlock.UnlockLevel();
        }
    }

    private void Update() {
        if (GameProgressManager.instance.CheckUnlockedWorld(worldToUnlock))
        {
            Debug.Log("World is unlocked");
            GetComponentInParent<Button>().interactable = true;
            //gameObject.SetActive(false);
            return ;
        }
        else
        {
            GetComponentInParent<Button>().interactable = false;
        }
    }
    
}