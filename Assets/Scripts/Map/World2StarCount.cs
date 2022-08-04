using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class World2StarCount : MonoBehaviour
{
    [SerializeField] Sprite openedLock;
    [SerializeField] Image world2Lock;
    [SerializeField] GameObject counter;
    TextMeshProUGUI countText;

    void Start()
    {
        countText = GetComponent<TextMeshProUGUI>();

        string starCollectedNumberText;

        starCollectedNumberText = GameState.instance.totalCollectedStars.ToString();/*(Mathf.Clamp(14 - GameState.instance.totalCollectedStars, 0, 14)).ToString();*/

        countText.text = starCollectedNumberText + "/14";

        if(GameState.instance.totalCollectedStars >= 14)
        {
            world2Lock.sprite = openedLock;
            counter.SetActive(false);
        }
    }
}