using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] Animator creditsAnimator;
    
    const string THANKS_FOR_PLAYING = "ThanksForPlaying_Scene";

    void Update()
    {
        if (creditsAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !creditsAnimator.IsInTransition(0))
        {
            LevelLoader.instance.LoadLevel(THANKS_FOR_PLAYING);
        }
    }
}
