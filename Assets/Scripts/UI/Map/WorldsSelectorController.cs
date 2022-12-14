using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldsSelectorController : MonoBehaviour
{
    WorldsSelectorController instance;
    [SerializeField]
    GameObject worldsSelector;
    [SerializeField]
    List<GameObject> worldLevelsSelector;
    [SerializeField]
    GameObject worldsBg;

    GameObject activeWorld;
    Animator WorldsAnimator;
    private string currentState;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        WorldsAnimator = worldsSelector.GetComponent<Animator>();
        if (GameProgressManager.instance.WorldSelection == 0)
            StartOnClosedWorlds();
        else
            StartOnOpenedWorld(GameProgressManager.instance.WorldSelection);
    }

    public void OnOpenWorld(int nbr)
    {
        activeWorld = worldLevelsSelector[nbr - 1];
        activeWorld.SetActive(true);
        ChangeAnimationStateWorlds("UI_selectWorld_" + nbr);
        ChangeAnimationStateLevels("UI_showLevels", activeWorld.GetComponent<Animator>());
        UIController.instance.ToLevels();
        BackgroundAnimationController.instance.ZoomIn();
    }

    public void OnCloseWorld()
    {
        ChangeAnimationStateLevels("UI_exitLevels", activeWorld.GetComponent<Animator>());
        ChangeAnimationStateWorlds("UI_returnWorlds");
        UIController.instance.ToWorlds();
        BackgroundAnimationController.instance.ZoomOut();
    }

    public void StartOnOpenedWorld(int nbr){
        foreach (GameObject openedWorld in worldLevelsSelector)
        {
           ChangeAnimationStateLevels("UI_levelsInactive", openedWorld.GetComponent<Animator>());
        }
        activeWorld = worldLevelsSelector[nbr - 1];
        activeWorld.SetActive(true);
        ChangeAnimationStateLevels("UI_levelsActive", activeWorld.GetComponent<Animator>());
        ChangeAnimationStateWorlds("UI_selectWorld_inactive");
        UIController.instance.EnableLevelsUI();
        //BackgroundAnimationController.instance.ZoomIn();
    }

      public void StartOnClosedWorlds(){
        foreach (GameObject openedWorld in worldLevelsSelector)
        {
           ChangeAnimationStateLevels("UI_levelsInactive", openedWorld.GetComponent<Animator>());
        }
        ChangeAnimationStateWorlds("UI_selectWorld_active");
        UIController.instance.EnableWorldsUI();
        //BackgroundAnimationController.instance.ZoomOut();
    }

    public void ChangeAnimationStateWorlds(string newState)
    {
        //stop the same animation from interrupting itself
        if(currentState == newState) return;

        //play the animation
        WorldsAnimator.Play(newState);
        //myAnimator.GetNextAnimatorStateInfo(0).

        //reassign the current state
        currentState = newState;
    }

    public void ChangeAnimationStateLevels(string newState, Animator myAnimator)
    {
        myAnimator.Play(newState);
    }
}
