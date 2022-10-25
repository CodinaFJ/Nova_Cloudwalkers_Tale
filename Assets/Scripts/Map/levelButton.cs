using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class levelButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] Sprite lockedLevel;
    [SerializeField] Sprite unlockedLevel;
    [SerializeField] Sprite selectLevel;
    [SerializeField] Sprite semiCompletedLevel;
    [SerializeField] Sprite completedLevel;
    [SerializeField] Sprite activeLevel;
    [SerializeField] public int worldNumber;
    [SerializeField] public int levelNumber;

    Button myButton;
    Image myImage;
    Image transitionImage;
    public bool completed = false;

    private void Start() 
    {
        myButton = GetComponent<Button>();
        myImage = GetComponent<Image>();
        if(GetComponentInChildren<ParticleSystem>() != null)GetComponentInChildren<ParticleSystem>().Stop();

        if(worldNumber == 1)
        {
            if(GameState.instance.completedLevelsWorld1[levelNumber - 1])
            {
                Debug.Log("level completed");
                myImage.sprite = completedLevel;
                myButton.image.sprite = semiCompletedLevel;
                if(GameState.instance.collectedStarsInLevelsWorld1[levelNumber - 1] ==
                   GameState.instance.totalStarsInLevelsWorld1[levelNumber - 1])
                {
                    myButton.image.sprite = completedLevel;
                }
            }
        }
        else if(worldNumber == 2)
        {
            if(GameState.instance.completedLevelsWorld2[levelNumber - 1])
            {
                myImage.sprite = completedLevel;
                myButton.image.sprite = semiCompletedLevel;
                if(GameState.instance.collectedStarsInLevelsWorld2[levelNumber - 1] ==
                   GameState.instance.totalStarsInLevelsWorld2[levelNumber - 1])
                {
                    myButton.image.sprite = completedLevel;
                }
            }
        }
        if(GameState.instance.lastLevel[0] == worldNumber && GameState.instance.lastLevel[1] == levelNumber)
        {
            GameObject activeLevel = GameObject.FindGameObjectWithTag("ActiveLevel");
            activeLevel.transform.SetParent(transform, false);
        }
    }

    public void LoadLevel()
    {
        string levelNameID = levelNumber.ToString() + "-" + worldNumber.ToString();
        string sceneToLoad = null;

        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
        string[] scenes = new string[sceneCount];
        for( int i = 0; i < sceneCount; i++ )
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension( UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i ) );
            if(scenes[i].Contains(levelNameID)) sceneToLoad = scenes[i];
        }


        FindObjectOfType<LevelSelectorController>().LoadLevel(sceneToLoad);
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        SFXManager.PlayHoverLevel();
    }

}
