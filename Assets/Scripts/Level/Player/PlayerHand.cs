using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    /*********************************************************************
    PlayerHand.cs

    Description:
        Controls the animations of the hand of the player. 

    Check also:

        PlayerBehavior.cs

    **********************************************************************/

    public static PlayerHand instance;
    Animator myAnimator;

    //Name of the current animation
    private string currentState;

    //Name of the animation that should be played
    private string handAnimation;

    //Check if the hand is being put out or not
    private bool putHandOut;
    public bool PutHandOut { get => putHandOut; set => putHandOut = value;}

    //Animation names
    const string HAND_DOWN_OUT = "HandDown_Out";
    const string HAND_DOWN_IN = "HandDown_In";
    const string HAND_UP_OUT = "HandUp_Out";
    const string HAND_UP_IN = "HandUp_In";
    const string HAND_RIGHT_OUT = "HandRight_Out";
    const string HAND_RIGHT_IN = "HandRight_In";

    private void Awake() 
    {
        instance = this;
    }

    void Start()
    {
        myAnimator = GetComponent<Animator>();

        //Dummy first animation so hand starts IN
        ChangeAnimationState(HAND_UP_IN);
    }

    void FixedUpdate()
    {
        //Each frame checks if the current animation is the correct one
        ChangeAnimationHandOut();
    }

    //Animation selection based on movement and position on PJ
    public void ChangeAnimationHandOut()
    {
        if(putHandOut)
        {
            if(PlayerBehavior.instance.runningUp || PlayerBehavior.instance.lastMovement == 2) handAnimation = HAND_UP_OUT;
            else if(PlayerBehavior.instance.runningDown || PlayerBehavior.instance.lastMovement == -2) handAnimation = HAND_DOWN_OUT;
            else if(PlayerBehavior.instance.runningRight || PlayerBehavior.instance.lastMovement == 1) handAnimation = HAND_RIGHT_OUT;
            else if(PlayerBehavior.instance.runningLeft || PlayerBehavior.instance.lastMovement == -1) handAnimation = HAND_RIGHT_OUT;
        }
        else
        {
            if(PlayerBehavior.instance.runningUp || PlayerBehavior.instance.lastMovement == 2) handAnimation = HAND_UP_IN;
            else if(PlayerBehavior.instance.runningDown || PlayerBehavior.instance.lastMovement == -2) handAnimation = HAND_DOWN_IN;
            else if(PlayerBehavior.instance.runningRight || PlayerBehavior.instance.lastMovement == 1) handAnimation = HAND_RIGHT_IN;
            else if(PlayerBehavior.instance.runningLeft || PlayerBehavior.instance.lastMovement == -1) handAnimation = HAND_RIGHT_IN;
        }

        ChangeAnimationStateMatchFrame(handAnimation);
    }

    //Changes animation based on the frame the last one is. This way the hand is not put out each time PJ changes orientation
    public void ChangeAnimationStateMatchFrame(string newState)
    {
        //Was the hand out and now should go in?
        bool changeDirection = false;
        if(currentState.Contains("In")  && newState.Contains("Out") || 
           currentState.Contains("Out") && newState.Contains("In"))
        {
            changeDirection = true;
        }

        //Check which fraction of the animation is complete
        float fractionOfAnimation;
        if(myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            fractionOfAnimation = 1.0f;
        }
        else
        {
            fractionOfAnimation = myAnimator.GetNextAnimatorStateInfo(0).normalizedTime;
        }

        //If the animation to play is the inverse of the one playing, let the current one finish
        if(changeDirection && fractionOfAnimation < 1) return;

        //Stop the same animation from interrupting itself
        if(currentState == newState) return;

        //The frames correspondace is reversed for OUT-IN animations
        if(changeDirection) fractionOfAnimation = 1 - fractionOfAnimation;

        //play the animation on the frame the other one was
        myAnimator.Play(newState, 0, fractionOfAnimation);

        //Reassign the current state
        currentState = newState;
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
