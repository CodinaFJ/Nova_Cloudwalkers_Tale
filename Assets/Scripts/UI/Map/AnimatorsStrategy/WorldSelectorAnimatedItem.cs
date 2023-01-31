using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Items with animation attached in Level Selector Menu.
/// </summary>
public class WorldSelectorAnimatedItem : MonoBehaviour
{

    [SerializeField] 
    private int worldNumber;//TODO: World number should be set parents of hierarchy
    [SerializeField]
    AnimatedItemType animatedItemType;

    private Vector2 initialPos;
    private Vector2 initialScale;

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
    public void PlaySelectWorldAnimation(int world, GameObject selectedWorldGO)
    {
        //TODO: Take animations to another method so fade in/out and position animations can be done with one method.
        if (worldNumber == world && animatedItemType == AnimatedItemType.WorldButton)
            LevelSelectorAnimations.instance.PlayWorldButtonSelectAnimation(this.gameObject);
        else if(animatedItemType == AnimatedItemType.UI)
            //TODO: Play selectWorld animation of UI item.
            return ;
        else
            LevelSelectorAnimations.instance.PlayWorldButtonHideAnimation(this.gameObject, selectedWorldGO);
            return ;
    }

    /// <summary>
    /// Play animation when world is closing.
    /// </summary>
    /// <param name="world"> World closing </param>
    public void PlayCloseWorldAnimation(int world)
    {
        //TODO: Play closeWorld animation of item.
    }

    /// <summary>
    /// Play animation when world is unlocking
    /// </summary>
    /// <param name="world"> World unlocking </param>
    public void PlayUnlockWorldAnimation(int world)
    {
        if (worldNumber == world)
            //TODO: Play unlockWorld animation of item.
            return ;
    }

    //TODO: Apply animation existence test with: https://answers.unity.com/questions/957649/check-animation-state-exists-before-playing-it.html


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
