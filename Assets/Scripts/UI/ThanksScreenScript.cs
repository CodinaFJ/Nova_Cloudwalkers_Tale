using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThanksScreenScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI collectedStarsText;
    LevelLoader levelLoader;
    // Start is called before the first frame update
    void Start()
    {     
        try{
            GameProgressManager.instance.CalculateCollectedStarsInGame();
            GameProgressManager.instance.CalculateTotalStarsInGame();
            collectedStarsText.text = "Star shards collected " + GameProgressManager.instance.GetCollectedStarsInGame() + "/" +  GameProgressManager.instance.GetTotalStarsInGame();
        }catch(Exception ex){
            Debug.LogWarning("Error in stars counters: " + ex.Message);
        }
        levelLoader = FindObjectOfType<LevelLoader>();
    }
    
    public void ToMap()
    {
        levelLoader.LoadLevel(LevelLoader.GetLevelContains("LevelSelectorMenu"));
        if(FindObjectOfType<MusicSelectionManager>() != null) FindObjectOfType<MusicSelectionManager>().FadeOutLevelMusic();
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
