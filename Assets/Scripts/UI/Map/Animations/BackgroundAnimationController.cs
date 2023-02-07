using UnityEngine;

/// <summary>
/// This class is legacy. Replaced by WorldSelectorAnimatedItem
/// </summary>
public class BackgroundAnimationController : MonoBehaviour
{
    public static BackgroundAnimationController instance;
    Animator myAnimator;
    private string currentState;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        myAnimator = GetComponent<Animator>();
    }
   
   public void ZoomIn() => ChangeAnimationStateWorlds("Background_ZoomIn");
   public void ZoomOut() => ChangeAnimationStateWorlds("Background_ZoomOut");

    public void ChangeAnimationStateWorlds(string newState)
    {
        //stop the same animation from interrupting itself
        if(currentState == newState) return;

        //play the animation
        myAnimator.Play(newState);

        //reassign the current state
        currentState = newState;
    }
}
