using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Legacy. Should be replaced by MapContextController.
/// </summary>
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
        //! Wouldn't it be better to leave all logic to the method itself?
        if (GameProgressManager.instance.WorldSelection == 0)
            StartOnClosedWorlds();
        else
            StartOnOpenedWorld(GameProgressManager.instance.WorldSelection);
    }

    

    /**************************************************************************************************
    INITIAL STATE METHODS //!This should now be done with the machine state pattern
    **************************************************************************************************/

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

    /**************************************************************************************************
    ANIMATION STATE METHODS //!All these are now controlled by new WorldSelectorAnimatedItem
    **************************************************************************************************/

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
        yield return null;
        while (true){
            if (worldsSelectorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !worldsSelectorAnimator.IsInTransition(0)) break;
            yield return null;
        }
        worldsButtons[nbr - 1].GetComponent<Button>().interactable = true;
        worldsSelectorAnimator.enabled = false;
        yield return new WaitForSeconds(0.5f);
        worldsSelectorAnimator.enabled = true;
        //worldsSelectorAnimator.controller
    }

    private IEnumerator DisableAfterPlay(Animator animator)
    {
        while (true){
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !worldsSelectorAnimator.IsInTransition(0)) break;
            yield return null;
        }
        worldsSelectorAnimator.enabled = false;
    }
}
