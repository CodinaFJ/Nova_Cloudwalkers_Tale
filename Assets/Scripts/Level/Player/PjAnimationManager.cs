using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PjAnimationManager : MonoBehaviour
{
    /*********************************************************************
    PjAnimationManager.cs

    Description:
        Change animation state of the object "Player" based on the states
        defined on PjInputManager.cs

    Check also:
        PlayerBehavior.cs

    **********************************************************************/

    PlayerBehavior playerBehavior;

    Animator myAnimator;
    Animator UIAnimator;
    private string currentState;

    //Animations names
    const string PLAYER_IDLE_UP = "Player_Idle_Up";
    const string PLAYER_IDLE_DOWN = "Player_Idle_Down";
    const string PLAYER_IDLE_RIGHT = "Player_Idle_R";
    const string PLAYER_RUNNING_RIGHT = "Player_Running_R";
    const string PLAYER_RUNNING_LEFT = "Player_Running_L";
    const string PLAYER_RUNNING_UP = "Player_Running_U";
    const string PLAYER_RUNNING_DOWN = "Player_Running_D";
    const string PLAYER_SITTING = "Player_Sitting";
    const string PLAYER_SLEEPING = "Player_Sleeping";

    

    void Start()
    {
        playerBehavior = FindObjectOfType<PlayerBehavior>();   
        myAnimator = GetComponent<Animator>();
        UIAnimator = GameObject.FindGameObjectWithTag("UI_anim").GetComponent<Animator>();  
    }

    void FixedUpdate()
    {
        if(playerBehavior.running)
        {
            if(playerBehavior.runningUp)
            {
                ChangeAnimationState(PLAYER_RUNNING_UP);
                transform.localScale = new Vector3(1f, 1f, 1f);
            } 
            else if(playerBehavior.runningDown)
            {
                ChangeAnimationState(PLAYER_RUNNING_DOWN);
                transform.localScale = new Vector3(1f, 1f, 1f);
            } 
            else if(playerBehavior.runningRight)
            {
                ChangeAnimationState(PLAYER_RUNNING_RIGHT);
                transform.localScale = new Vector3(1f, 1f, 1f);
            } 
            else if(playerBehavior.runningLeft)
            {
                ChangeAnimationState(PLAYER_RUNNING_LEFT);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            } 
        }
        else if(playerBehavior.sitting)
        {
            if(playerBehavior.sleeping)
            {
                ChangeAnimationState(PLAYER_SLEEPING);  
            }
            else
            {
                ChangeAnimationState(PLAYER_SITTING);
            }
        }
        else
        {
            if(playerBehavior.lastMovement == -2)
            {
                ChangeAnimationState(PLAYER_IDLE_UP);
                transform.localScale = new Vector3(1f, 1f, 1f);
            } 
            else if(playerBehavior.lastMovement == 2)
            {
                ChangeAnimationState(PLAYER_IDLE_DOWN);
                transform.localScale = new Vector3(1f, 1f, 1f);
            } 
            else if(playerBehavior.lastMovement == -1)
            {
                ChangeAnimationState(PLAYER_IDLE_RIGHT);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            } 
            else
            {
                ChangeAnimationState(PLAYER_IDLE_RIGHT);
                transform.localScale = new Vector3(1f, 1f, 1f);
            } 
        }
    }

    public void PjClickAnimation(Vector3 pos)
    {
        UIAnimator.transform.position = pos;
        UIAnimator.Play("UI_ClickFloor");
    }



    public void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if(currentState == newState) return;

        //play the animation
        myAnimator.Play(newState);
        //myAnimator.GetNextAnimatorStateInfo(0).

        //reassign the current state
        currentState = newState;
    }
}
