using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Items with animation attached in Level Selector Menu.
/// </summary>
public class WorldSelectorAnimatedItem : MonoBehaviour
{

    [SerializeField] private int worldNumber;//TODO: World number should be set parents of hierarchy

    [SerializeField]
    AnimatedItemType animatedItemType;

    /**************************************************************************************************
    Animation control methods
    **************************************************************************************************/
    /// <summary>
    /// Play animations when world is selected.
    /// </summary>
    /// <param name="world"> World selected </param>
    public void PlaySelectWorldAnimation(int world)
    {
        if (worldNumber == world && animatedItemType == AnimatedItemType.WorldButton)
            LevelSelectorAnimations.instance.PlayWorldButtonSelectAnimation(this.gameObject);
        else if(animatedItemType == AnimatedItemType.UI)
            //TODO: Play selectWorld animation of UI item.
            return ;
        else
            //TODO: Play hide animation (Implemented with leanTween?)
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
    public void SetWorldNumber(int worldNumber) => this.worldNumber = worldNumber;
}
