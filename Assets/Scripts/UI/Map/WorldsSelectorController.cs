using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldsSelectorController : MonoBehaviour
{
    public static WorldsSelectorController instance;
    [SerializeField]
    Animator worldsSelectorAnimator;
    [SerializeField]
    List<GameObject> worldsButtons;
    [SerializeField]
    List<GameObject> worldLevelsSelector;
    [SerializeField]
    GameObject worldsBg;

    GameObject activeWorld;
    private string currentState;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
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

    public void UnlockWorld(int nbr)
    {
        //ChangeAnimationStateLevels("UI_exitLevels", activeWorld.GetComponent<Animator>());
        StartCoroutine(UnlockWorldAfterAnimation("UI_returnUnlock_" + nbr, nbr));
        UIController.instance.ToWorlds();
    }

    public void ChangeAnimationStateWorlds(string newState)
    {
        //stop the same animation from interrupting itself
        if(currentState == newState) return;

        Debug.Log(("Animation: " + newState));
        //play the animation
        worldsSelectorAnimator.Play(newState);
        //myAnimator.GetNextAnimatorStateInfo(0).

        //reassign the current state
        currentState = newState;
    }

    public void ChangeAnimationStateLevels(string newState, Animator myAnimator)
    {
        myAnimator.Play(newState);
    }

    private IEnumerator UnlockWorldAfterAnimation(string newState, int nbr)
    {
        Debug.Log("PlayAnimationNext: " + newState);
        while (true){
            if (worldsSelectorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !worldsSelectorAnimator.IsInTransition(0)) break;
            yield return null;
        }
        foreach (GameObject openedWorld in worldLevelsSelector)
        {
           ChangeAnimationStateLevels("UI_levelsInactive", openedWorld.GetComponent<Animator>());
        }
        ChangeAnimationStateWorlds(newState);
        while (true){
            if (worldsSelectorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !worldsSelectorAnimator.IsInTransition(0)) break;
            yield return null;
        }
        worldsButtons[nbr - 1].GetComponent<Button>().interactable = true;
    }
}
