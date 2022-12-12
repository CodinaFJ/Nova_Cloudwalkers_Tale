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
        ChangeAnimationStateWorlds("UI_selectWorld_" + nbr);
        worldLevelsSelector[nbr - 1].SetActive(true);
        ChangeAnimationStateLevels("UI_showLevels", worldLevelsSelector[nbr - 1].GetComponent<Animator>());
        BackgroundAnimationController.instance.ZoomIn();
    }

    public void BackToWorlds()
    {
        List<GameObject> levelsMapGO = worldLevelsSelector.FindAll(x => x.activeSelf);
        ChangeAnimationStateLevels("UI_exitLevels", levelsMapGO[0].GetComponent<Animator>());
        /*foreach (GameObject mapGO in levelsMapGO)
            mapGO.SetActive(false);*/
        ChangeAnimationStateWorlds("UI_returnWorlds");
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
