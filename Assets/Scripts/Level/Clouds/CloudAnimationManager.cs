using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAnimationManager : MonoBehaviour
{
    Animator myAnimator;
    private string currentState;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();   
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
}
