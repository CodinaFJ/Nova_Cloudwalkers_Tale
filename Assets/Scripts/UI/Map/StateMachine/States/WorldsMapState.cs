using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// From MapState: State where worlds can be selected. Levels are hidden.
/// </summary>
public class WorldsMapState : MapState
{
	/// <summary>
	/// Initialize state using controller as context.
	/// </summary>
	/// <param name="mapContextController">Level selector (map) controller</param>
	/// <returns></returns>
    public WorldsMapState(MapContextController mapContextController) : base(mapContextController)
    {}

    /**************************************************************************************************
    STATE MACHINE ACTIONS
    **************************************************************************************************/
	/// <summary>
	/// Open selected world and updates openWorld in context.
	/// </summary>
	/// <param name="selectWorldGO"> Selected world GO </param>
	public override void SelectWorldAction(GameObject selectWorldGO)
	{
		int world;

		world = selectWorldGO.GetComponent<WorldSelectorAnimatedItem>().GetWorldNumber();
		//!THIS IS THE CORRECT LINE: mapContextController.AnimationControlWorldSelected(world, selectWorldGO);
		UnlockWorldAction(2);
       //! mapContextController.SetOpenWorld(world);
        //!mapContextController.SetMapState(mapContextController.GetLevelsMapState());
	}

	/// <summary>
	/// Starts unlock world sequence.
	/// </summary>
	/// <param name="world"> World to unlock </param>
	public override void UnlockWorldAction(int world)
	{
		mapContextController.SetMapState(mapContextController.GetUnlockingWorldsMapState());
        mapContextController.GetMapState().UnlockWorldAction(world);
	}
}
