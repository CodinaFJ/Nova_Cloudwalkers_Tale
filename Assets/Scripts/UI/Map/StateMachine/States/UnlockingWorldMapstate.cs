using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// From MapState: State where one world is being unlocked.
/// </summary>
public class UnlockingWorldMapstate : MapState
{
	/// <summary>
	/// Initialize state using controller as context.
	/// </summary>
	/// <param name="mapContextController">Level selector (map) controller</param>
	/// <returns></returns>
    public UnlockingWorldMapstate(MapContextController mapContextController) : base(mapContextController)
    {}

    /**************************************************************************************************
    STATE MACHINE ACTIONS
    **************************************************************************************************/

	/// <summary>
	/// Sets state back to GetWorldsMapState.
	/// </summary>
	public override void FinishUnlockWorldAction()
	{
		mapContextController.SetMapState(mapContextController.GetWorldsMapState());
	}

	/// <summary>
	/// Starts Unlock World Animation.
	/// On finish calls FinishUnlockWorldAction.
	/// </summary>
	/// <param name="world"> World to unlock </param>
	public override void UnlockWorldAction(int world)
	{
		mapContextController.PlayUnlockWorldAnimations(world);
        FinishUnlockWorldAction();
	}
}
