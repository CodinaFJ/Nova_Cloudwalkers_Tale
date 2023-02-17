using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLevelAnimator : MonoBehaviour
{
    public static ActiveLevelAnimator  instance;

    private Animator    animator;
    [SerializeField] private string      currentState;
    [SerializeField] private bool        wallLevel;

    private void Awake() 
    {
        instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        wallLevel = GameProgressManager.instance.GetActiveLevel().GetWallLevel();
        SetAnimation();
    }

    private void OnEnable()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        if (wallLevel)
            PlayAnimation("SelectorMapMural");
        else
            PlayAnimation("SelectorMap");
    }

    /// <summary>
    /// Plays requested animation if possible. Protected against no animator o no animation in animator.
    /// </summary>
    /// <param name="newState">Requested state name</param>
    private void    PlayAnimation(string newState)
    {
        if (!animator)
        {
            Debug.LogWarning("No animator for animating state: " + newState);
            return ;
        }
        if (animator.HasState(0, Animator.StringToHash(newState)))
        {
            animator.Play(newState);
            currentState = newState;
        }
        else
            Debug.LogWarning("No state: " + newState);
    }

    //public void SetWallLevel(bool value) => this.wallLevel = value;
}
