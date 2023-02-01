using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Items with animation attached in Level Selector Menu.
/// </summary>
public class WorldSelectorAnimatedItem : MonoBehaviour
{

    [SerializeField] int                worldNumber;//TODO: World number should be set parents of hierarchy
    [SerializeField] AnimatedItemType   animatedItemType;

    private Vector2     initialPos;
    private Vector2     initialScale;
    private Animator    animator;
    private string      currentState;

    private void Start() 
    {
        initialPos = transform.position;
        initialScale = transform.localScale;
        animator = GetComponent<Animator>();
    }
    /**************************************************************************************************
    Animation control methods
    **************************************************************************************************/
    /// <summary>
    /// Play animations when world is selected.
    /// </summary>
    /// <param name="world"> World selected </param>
    public void AnimationControlWorldSelected(int world, GameObject selectedWorldGO)
    {
        //TODO: Take animations to another method so fade in/out and position animations can be done with one method.
        if (worldNumber == world && animatedItemType == AnimatedItemType.WorldButton)
            PlayOpenWorldAnimation();
        else if(worldNumber == world && animatedItemType == AnimatedItemType.Levels)
            PlayShowLevelsAnimation();
        else if(animatedItemType == AnimatedItemType.UI)
            PlayOpenWorldUIAnimation();
        else
            PlayHideWorldAnimation(selectedWorldGO);
    }

    /// <summary>
    /// Play animation when world is closing.
    /// </summary>
    /// <param name="world"> World closing </param>
    public void AnimationControlWorldClosed(int world)
    {
        //TODO: Play closeWorld animation of item.
    }

    /// <summary>
    /// Play animation when world is unlocking
    /// </summary>
    /// <param name="world"> World unlocking </param>
    public void AnimationControlWorldUnlock(int world)
    {
        if (worldNumber == world)
            //TODO: Play unlockWorld animation of item.
            return ;
    }

    /**************************************************************************************************
    Animations
    **************************************************************************************************/

    /// <summary>
    /// Play open world animations
    /// </summary>
    private void    PlayOpenWorldAnimation()
    {
        LevelSelectorAnimations.instance.PlayWorldButtonSelectAnimation(this.gameObject);
        PlayAnimation("WorldOpenFadeOut");
    }

    /// <summary>
    /// Play hide world animations
    /// </summary>
    private void    PlayHideWorldAnimation(GameObject selectedWorldGO)
    {
        LevelSelectorAnimations.instance.PlayWorldButtonHideAnimation(this.gameObject, selectedWorldGO);
        PlayAnimation("WorldHideFadeOut");
    }

    private void    PlayShowLevelsAnimation()
    {
        PlayAnimation("LevelsShow");
    }

    private void    PlayOpenWorldUIAnimation()
    {
        PlayAnimation("UIOpenWorld");
    }


    /**************************************************************************************************
    Animator control
    **************************************************************************************************/

    /// <summary>
    /// Plays requested animation if possible. Protected against no animator o no animation in animator.
    /// </summary>
    /// <param name="newState">Requested state name</param>
    private void    PlayAnimation(string newState)
    {
        if (!animator || newState == currentState)
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


    /**************************************************************************************************
    Setters
    **************************************************************************************************/
    public void     SetWorldNumber(int worldNumber) => this.worldNumber = worldNumber;

    /**************************************************************************************************
    Getters
    **************************************************************************************************/
    public Vector2  GetInitialPos()     => this.initialPos;
    public Vector2  GetInitialScale()   => this.initialScale;
    public int      GetWorldNumber()    => this.worldNumber;
}
