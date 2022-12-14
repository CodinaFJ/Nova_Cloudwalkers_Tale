using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class levelButton : MonoBehaviour, IPointerEnterHandler
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
        
        
        if(GameProgressManager.instance.GetActiveLevel().GetLevelNumber() == levelNumber && GameProgressManager.instance.GetActiveWorld().GetLevelWorldNumber() == worldNumber){
            GameObject activeLevelAnim = GameObject.FindGameObjectWithTag("ActiveLevel");
            activeLevelAnim.transform.SetParent(transform, false);
        }
    }

    public int GetLevelNumber() => levelNumber;
    public int GetWorldNumber() => worldNumber;

    public void LoadLevel(){
        GameProgressManager.instance.SetActiveLevel(worldNumber, levelNumber);
        string levelNameID = levelNumber.ToString() + "-" + worldNumber.ToString();

        if(levelNumber == 1 && !GameProgressManager.instance.GetPlayedCinematic(worldNumber) && worldNumber != 1){
            levelNameID = "Cinematic" + worldNumber;
        }

        FindObjectOfType<LevelSelectorController>().LoadLevel(LevelLoader.GetLevelContains(levelNameID));
    }

    private void SelectButtonStatus(){
        if(level.GetLevelCompleted()){
            myImage.sprite = completedLevel;
            myButton.image.sprite = semiCompletedLevel;
            myButton.interactable = true;

            if(level.GetCollectedStars() >= level.GetNumberOfStars()){
                myButton.image.sprite = completedLevel;
            }
        }
        else if(level.GetLevelUnlocked()){
            myImage.sprite = unlockedLevel;
            myButton.image.sprite = unlockedLevel;
            myButton.interactable = true;
        }
        else if(!level.GetLevelUnlocked() && UnlockLevelQuery()){
            UnlockLevel();
        }
        
    }

    private bool UnlockLevelQuery(){
        if(unlockerLevels.Length == 0) return false;
        foreach(int i in unlockerLevels){
            if(GameProgressManager.instance.GetLevel(worldNumber, i).GetLevelCompleted()) return true;
        }
        return false;
    }

    IEnumerator DelayUnlockAnimationStart(string newState)
    {
        yield return new WaitForSeconds(1f);
        if(levelNumber == 1) SFXManager.PlayUnlockWorld();
        else SFXManager.PlayUnlockLevel();
        ChangeAnimationState(newState);
        myButton.interactable = true;
        level.SetLevelUnlocked(true);
        myButton.GetComponentInChildren<ParticleSystem>().Play();
    }

    public void ChangeAnimationState(string newState)
    {
        //play the animation
        myButton.GetComponent<Animator>().enabled = true;
        myButton.GetComponent<Animator>().Play(newState);
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        if(myButton.interactable) SFXManager.PlayHoverLevel();
    }

    public void UnlockLevel(){
        if(!GameProgressManager.instance.GetLevel(worldNumber, levelNumber).GetLevelUnlocked())
            StartCoroutine(DelayUnlockAnimationStart("UI_LevelUnlock"));
    }

}
