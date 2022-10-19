using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelButton : MonoBehaviour
{
    [Header ("Sprites")]
    [SerializeField] Sprite lockedLevel;
    [SerializeField] Sprite unlockedLevel;
    [SerializeField] Sprite selectLevel;
    [SerializeField] Sprite semiCompletedLevel;
    [SerializeField] Sprite completedLevel;
    [SerializeField] Sprite activeLevel;

    [Header ("Level Info")]
    [SerializeField] private int worldNumber;
    [SerializeField] private int levelNumber;
    [SerializeField] private int[] unlockerLevels;

    Button myButton;
    Image myImage;
    Image transitionImage;
    public bool completed = false;
    Level level;

    private void Start() 
    {
        myButton = GetComponent<Button>();
        myImage = GetComponent<Image>();
        if(GetComponentInChildren<ParticleSystem>() != null)GetComponentInChildren<ParticleSystem>().Stop();

        try{
            level = GameProgressManager.instance.GetLevel(worldNumber, levelNumber);
            SelectButtonStatus();
        }catch{
            Debug.LogWarning("Error importing level on button for level: " + worldNumber + ", " +levelNumber);
        }
        
        
        if(GameProgressManager.instance.GetActiveLevel() == level){
            GameObject activeLevelAnim = GameObject.FindGameObjectWithTag("ActiveLevel");
            activeLevelAnim.transform.SetParent(transform, false);
        }
    }

    public int GetLevelNumber() => levelNumber;
    public int GetWorldNumber() => worldNumber;

    public void LoadLevel(){
        GameProgressManager.instance.SetActiveLevel(worldNumber, levelNumber);
        string levelNameID = levelNumber.ToString() + "-" + worldNumber.ToString();
        string sceneToLoad = null;

        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
        string[] scenes = new string[sceneCount];
        for( int i = 0; i < sceneCount; i++ ){
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i ));
            if(scenes[i].Contains(levelNameID)) sceneToLoad = scenes[i];
        }

        FindObjectOfType<LevelSelectorController>().LoadLevel(sceneToLoad);
    }

    private void SelectButtonStatus(){
        if(!UnlockLevelQueryPre() && UnlockLevelQuery()){
            StartCoroutine(DelayUnlockAnimationStart("UI_LevelUnlock"));
        }
        if(level.GetLevelCompleted()){
            myImage.sprite = completedLevel;
            myButton.image.sprite = semiCompletedLevel;

            if(level.GetCollectedStars() == level.GetNumberOfStars()){
                myButton.image.sprite = completedLevel;
            }
        }
    }

    private bool UnlockLevelQuery(){
        if(unlockerLevels.Length == 0) return true;
        foreach(int i in unlockerLevels){
            if(GameProgressManager.instance.GetLevel(worldNumber, i).GetLevelCompleted()) return true;
        }
        return false;
    }

    private bool UnlockLevelQueryPre(){
        foreach(int i in unlockerLevels){
            if(GameProgressManager.instance.GetLevelPrevious(worldNumber, i).GetLevelCompleted()) return true;
        }
        return false;
    }

    IEnumerator DelayUnlockAnimationStart(string newState)
    {
        yield return new WaitForSeconds(1.5f);
        ChangeAnimationState(newState);
        myButton.interactable = true;
        myButton.GetComponentInChildren<ParticleSystem>().Play();
    }

    public void ChangeAnimationState(string newState)
    {
        //play the animation
        myButton.GetComponent<Animator>().enabled = true;
        myButton.GetComponent<Animator>().Play(newState);
    }
    

}
