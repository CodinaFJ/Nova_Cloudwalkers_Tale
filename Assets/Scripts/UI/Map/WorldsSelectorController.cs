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

    GameObject activeWorld;
    Animator WorldsAnimator;
    private string currentState;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        WorldsAnimator = worldsSelector.GetComponent<Animator>();  
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

    public void BackToWorlds()
    {
        ChangeAnimationStateLevels("UI_exitLevels", activeWorld.GetComponent<Animator>());
        ChangeAnimationStateWorlds("UI_returnWorlds");
        UIController.instance.ToWorlds();
        BackgroundAnimationController.instance.ZoomOut();
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
