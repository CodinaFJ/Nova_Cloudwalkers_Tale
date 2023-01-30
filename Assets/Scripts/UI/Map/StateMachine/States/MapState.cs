using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Abstract class for states of the State Machine Pattern applied for the Level Selection menu.
/// </summary>
public class MapState
{
    protected MapContextController mapContextController;

    /// <summary>
    /// Initialize state using controller as context.
    /// </summary>
    /// <param name="mapContextController">Level selector (map) controller</param>
    public MapState(MapContextController mapContextController)
    {
        this.mapContextController = mapContextController;
    }

    /**************************************************************************************************
    STATE MACHINE ACTIONS
    **************************************************************************************************/

    /// <summary>
    /// State Machine Action: Close level selector within one world to navigate to worlds selector.
    /// </summary>
    public virtual void CloseWorldAction()
    {
        throw new System.NotImplementedException("Cannot perform " + System.Reflection.MethodBase.GetCurrentMethod().Name + "while in " + this.GetType().Name);
    }

    /// <summary>
    /// State Machine Action: Finish animation of world unlocking.
    /// </summary>
    public virtual void FinishUnlockWorldAction()
    {
        throw new System.NotImplementedException("Cannot perform " + System.Reflection.MethodBase.GetCurrentMethod().Name + "while in " + this.GetType().Name);
    }

    /// <summary>
    /// State Machine Action: Select one of the worlds to enter to its levels.
    /// </summary>
    /// <param name="world"> World selected </param>
    public virtual void SelectWorldAction(int world)
    {
        throw new System.NotImplementedException("Cannot perform " + System.Reflection.MethodBase.GetCurrentMethod().Name + "while in " + this.GetType().Name);
    }

    /// <summary>
    /// State Machine Action: Start unlocking process of one world
    /// </summary>
    /// <param name="world"> World to unlock </param>
    public virtual void UnlockWorldAction(int world)
    {
        throw new System.NotImplementedException("Cannot perform " + System.Reflection.MethodBase.GetCurrentMethod().Name + "while in " + this.GetType().Name);
    }
}
