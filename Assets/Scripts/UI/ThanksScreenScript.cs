using System.Collections;
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
        GameState.instance.totalCollectedStars = 0;
        GameState.instance.totalStars = 0;

        
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

        if(GameState.instance != null)
        {
            collectedStarsText.text = "Star shards collected " + GameState.instance.totalCollectedStars + "/" + GameState.instance.totalStars;
        }
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToMap()
    {
        levelLoader.LoadLevel("LevelSelectorMenu");
        if(FindObjectOfType<MusicSelectionManager>() != null) FindObjectOfType<MusicSelectionManager>().FadeOutLevelMusic();
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
