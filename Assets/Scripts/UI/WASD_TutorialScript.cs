using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASD_TutorialScript : MonoBehaviour
{   
    [SerializeField] GameObject clickAnim;
    [SerializeField] GameObject pointerAnim;

    PlayerBehavior playerBehavior;
    Animator myAnimator;
    private string currentState;

    void Start() 
    {
        myAnimator = GetComponent<Animator>();
        playerBehavior = FindObjectOfType<PlayerBehavior>();
    }

    public void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if(currentState == newState) return;

        //play the animation
        myAnimator.Play(newState);

        //reassign the current state
        currentState = newState;
    }


    public void FadeOutMouse()
    {
        //ChangeAnimationState("WASD_TutorialFadeOut");
        Destroy(clickAnim);
        Destroy(pointerAnim);
        Destroy(gameObject);
    }
}
