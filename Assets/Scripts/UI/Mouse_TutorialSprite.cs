using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_TutorialSprite : MonoBehaviour
{  
    Animator myAnimator;
    private string currentState;
    
    void Start() 
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<CloudInputManager>().doingMagic)
        {
            //ChangeAnimationState("Mouse_Tutorial_FadeOut");
            Destroy(gameObject);
        }
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
