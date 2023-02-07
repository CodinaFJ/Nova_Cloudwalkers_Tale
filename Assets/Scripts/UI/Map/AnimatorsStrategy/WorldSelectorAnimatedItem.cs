using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Items with animation attached in Level Selector Menu.
/// </summary>
public class WorldSelectorAnimatedItem : MonoBehaviour
{

    [SerializeField] int                worldNumber;//TODO: World number should be set by parents in hierarchy
    [SerializeField] AnimatedItemType   animatedItemType;

    private Vector2     initialPos;
    private Vector2     initialScale;
    private Animator    animator;
    private string      currentState;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() 
    {
        initialPos = transform.position;
        initialScale = transform.localScale;
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
        if (worldNumber == world && animatedItemType == AnimatedItemType.WorldButton)
            PlayOpenWorldAnimation();
        else if(worldNumber == world && animatedItemType == AnimatedItemType.Levels)
            PlayWorldOpenAnimation();
        else if(animatedItemType == AnimatedItemType.UIWorlds
                || animatedItemType == AnimatedItemType.UILevels
                || animatedItemType == AnimatedItemType.Lock
                || animatedItemType == AnimatedItemType.Background)
            PlayWorldOpenAnimation();
        else if (animatedItemType == AnimatedItemType.WorldButton)
            PlayHideWorldAnimation(selectedWorldGO);
    }

    /// <summary>
    /// Play animation when world is closing.
    /// </summary>
    /// <param name="world"> World closing </param>
    public void AnimationControlWorldClosed(int world)
    {
        if (animatedItemType == AnimatedItemType.WorldsContainer)
            LevelSelectorAnimations.instance.PlayWorldsContainerScaleDownAnimation(this.gameObject);
        else
            PlayCloseWorldAnimation();
    }

    /// <summary>
    /// Play animation when world is unlocking
    /// </summary>
    /// <param name="world"> World unlocking </param>
    public void AnimationControlWorldUnlock(int world)
    {
        if (worldNumber == world)
        {
            if (animatedItemType == AnimatedItemType.WorldButton)
                LevelSelectorAnimations.instance.PlayWorldButtonUnlockAnimation(this.gameObject);
            PlayUnlockWorldAnimation();
        }
    }

    public void AnimationControlStartWorldOpen(int world)
    {
        if (animatedItemType == AnimatedItemType.WorldButton
            || animatedItemType == AnimatedItemType.Lock)
            PlayStartOut();
        else if (worldNumber == world
                || animatedItemType == AnimatedItemType.UILevels
                || animatedItemType == AnimatedItemType.Background)
            PlayStartIn();
        else
            PlayStartOut();
    }

    public void AnimationControlStartWorldsClosed()
    {
        if (animatedItemType == AnimatedItemType.WorldButton
            || animatedItemType == AnimatedItemType.Lock)
            PlayStartIn();
        else if (worldNumber != 0
                || animatedItemType == AnimatedItemType.UILevels
                || animatedItemType == AnimatedItemType.Background)
            PlayStartOut();
        else
            PlayStartIn();
    }

    /**************************************************************************************************
    Animations
    **************************************************************************************************/
    /************************************************
    Open World
    ************************************************/

    private void    PlayOpenWorldAnimation()
    {
        LevelSelectorAnimations.instance.PlayWorldButtonSelectAnimation(this.gameObject);
        PlayAnimation("WorldOpenFadeOut");
    }

    private void    PlayHideWorldAnimation(GameObject selectedWorldGO)
    {
        LevelSelectorAnimations.instance.PlayWorldButtonHideAnimation(this.gameObject, selectedWorldGO);
        PlayAnimation("WorldHideFadeOut");
    }

    private void    PlayWorldOpenAnimation() => PlayAnimation("WorldOpen");

    /************************************************
    Close World
    ************************************************/

    private void    PlayCloseWorldAnimation()
    {
        PlayAnimation("WorldClose");
    }

    /************************************************
    Unlock World
    ************************************************/

    private void    PlayUnlockWorldAnimation()
    {
        Button worldButton;
        if (worldButton = this.gameObject.GetComponent<Button>())
            worldButton.interactable = true;
        PlayAnimation("WorldUnlock");
    }

    /************************************************
    Start In Screen
    ************************************************/

    private void    PlayStartIn()
    {
        PlayAnimation("StartIn");
    }

    /************************************************
    Start Off Screen
    ************************************************/

    private void    PlayStartOut()
    {
        PlayAnimation("StartOut");
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
    public Vector2          GetInitialPos()         => this.initialPos;
    public Vector2          GetInitialScale()       => this.initialScale;
    public int              GetWorldNumber()        => this.worldNumber;
    public AnimatedItemType GetAnimatedItemType()   => this.animatedItemType;
}
