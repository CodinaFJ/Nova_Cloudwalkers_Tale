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
    [SerializeField] private GameObject starsCounterGO;

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
        SetActiveLevelAnimation();
        SetCounterState();
    }

    private void OnEnable()
    {
        try{
            level = GameProgressManager.instance.GetLevel(worldNumber, levelNumber);
            SelectButtonStatus();
        }catch{
            Debug.LogWarning("Error importing level on button for level: " + worldNumber + ", " +levelNumber);
        }
    }

    /**************************************************************************************************
    Button mechanics
    **************************************************************************************************/

    public void LoadLevel(){
        GameProgressManager.instance.SetActiveLevel(worldNumber, levelNumber);
        GameProgressManager.instance.SaveGameState();
        string levelNameID = levelNumber.ToString() + "-" + worldNumber.ToString();

        if(levelNumber == 1 && !GameProgressManager.instance.GetPlayedCinematic(worldNumber) && worldNumber != 1){
            levelNameID = "Cinematic" + worldNumber;
        }

        FindObjectOfType<LevelSelectorController>().LoadLevel(LevelLoader.GetLevelContains(levelNameID));
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        if(myButton.interactable) SFXManager.PlayHoverLevel();
    }

    /**************************************************************************************************
    Initial status
    **************************************************************************************************/

    private void SetCounterState()
    {
        bool status = GameProgressManager.instance.GetLevel(worldNumber, levelNumber).GetLevelUnlocked();

        if (!starsCounterGO)
            return ;
        starsCounterGO.SetActive(status);
    }

    private void SetCounterState(bool status)
    {
        if (!starsCounterGO)
            return ;
        starsCounterGO.SetActive(status);
    }

    private void SetActiveLevelAnimation()
    {
        int activeLevel = GameProgressManager.instance.GetActiveLevel().GetLevelNumber();
        int activeWorld = GameProgressManager.instance.GetActiveWorld().GetLevelWorldNumber();
        bool wallLevel = GameProgressManager.instance.GetActiveLevel().GetWallLevel();
        GameObject activeLevelAnim = GameObject.FindGameObjectWithTag("ActiveLevel");

        if(activeLevel == levelNumber && activeWorld == worldNumber)
            activeLevelAnim.transform.SetParent(transform, false);
    }

    private void SelectButtonStatus(){
        if(level.GetLevelCompleted())
        {
            myImage.sprite = completedLevel;
            myButton.image.sprite = semiCompletedLevel;
            myButton.interactable = true;
            SetCounterState(true);

            if(level.GetCollectedStars() >= level.GetNumberOfStars()){
                myButton.image.sprite = completedLevel;
            }
        }
        else if(level.GetLevelUnlocked())
        {
            myImage.sprite = unlockedLevel;
            myButton.image.sprite = unlockedLevel;
            myButton.interactable = true;
            SetCounterState(true);
        }
        else if(!level.GetLevelUnlocked() && UnlockLevelQuery())
        {
            UnlockLevel();
        }
    }

    private bool UnlockLevelQuery(){
        if(unlockerLevels.Length == 0)
            return false;
        foreach(int i in unlockerLevels)
        {
            if(GameProgressManager.instance.GetLevel(worldNumber, i).GetLevelCompleted())
                return true;
        }
        return false;
    }

    IEnumerator DelayUnlockAnimationStart(string newState)
    {
        yield return new WaitWhile(GetTransitionEnded);
        if(levelNumber == 1) SFXManager.PlayUnlockWorld();
        else SFXManager.PlayUnlockLevel();
        ChangeAnimationState(newState);
        level.SetLevelUnlocked(true);
        myButton.interactable = true;
        myButton.GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.3f);
        if (starsCounterGO)
            starsCounterGO.SetActive(true);
    }

    private bool GetTransitionEnded()
    {
        return !(LevelLoader.instance.GetTransitionEnded());
    }

    public void UnlockLevel()
    {
        if(!GameProgressManager.instance.GetLevel(worldNumber, levelNumber).GetLevelUnlocked())
            StartCoroutine(DelayUnlockAnimationStart("UI_LevelUnlock"));
    }

    public void ChangeAnimationState(string newState)
    {
        myButton.GetComponent<Animator>().enabled = true;
        myButton.GetComponent<Animator>().Play(newState);
    }

    /**************************************************************************************************
    Getters
    **************************************************************************************************/

    public int GetLevelNumber() => levelNumber;
    public int GetWorldNumber() => worldNumber;
}
