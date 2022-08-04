using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalStarsCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI counterText;

    // Start is called before the first frame update
    void Start()
    {
        GameState.instance.totalCollectedStars = 0;
        GameState.instance.totalStars = 0;

        //counterText = GetComponentInChildren<TextMeshProUGUI>();
        
        for(int i = 0; i < GameState.instance.collectedStarsInLevelsWorld1.Count; i++)
        {
            GameState.instance.totalCollectedStars += GameState.instance.collectedStarsInLevelsWorld1[i];
        }

        for(int i = 0; i < GameState.instance.collectedStarsInLevelsWorld2.Count; i++)
        {
            GameState.instance.totalCollectedStars += GameState.instance.collectedStarsInLevelsWorld2[i];
        }

        for(int i = 0; i < GameState.instance.totalStarsInLevelsWorld1.Count; i++)
        {
            GameState.instance.totalStars += GameState.instance.totalStarsInLevelsWorld1[i];
        }

        for(int i = 0; i < GameState.instance.totalStarsInLevelsWorld2.Count; i++)
        {
            GameState.instance.totalStars += GameState.instance.totalStarsInLevelsWorld2[i];
        }

        counterText.text = GameState.instance.totalCollectedStars + "/" + GameState.instance.totalStars;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
