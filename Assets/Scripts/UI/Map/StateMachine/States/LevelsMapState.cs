using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// From MapState: State where levels from one specific world can be selected.
/// </summary>
public class LevelsMapState : MapState
{
	/// <summary>
	/// Initialize state using controller as context.
	/// </summary>
	/// <param name="mapContextController">Level selector (map) controller</param>
	/// <returns></returns>
    public LevelsMapState(MapContextController mapContextController) : base(mapContextController)
    {}

    /**************************************************************************************************
    STATE MACHINE ACTIONS
    **************************************************************************************************/

	/// <summary>
	/// Close world when click on return UI Button
	/// </summary>
	public override void CloseWorldAction()
	{
		CloseWorld(mapContextController.GetOpenWorld());
        mapContextController.SetMapState(mapContextController.GetWorldsMapState());
	}

	/// <summary>
	/// Close world when need to unlock world.
	/// Start UnlockWorldAction after closing world.
	/// </summary>
	/// <param name="world"> World to unlock. </param>
	public override void UnlockWorldAction(int world)
	{
		CloseWorld(mapContextController.GetOpenWorld());
        Debug.Log("Waiting for world to be closed");
        mapContextController.GetMapState().UnlockWorldAction(world);
	}

    /**************************************************************************************************
    METHODS
    **************************************************************************************************/

	/// <summary>
	/// Start close world animation and set state on context.
	/// ? Parameter may not be needed.
	/// </summary>
	/// <param name="world"> World to close. </param>
    private void CloseWorld(int world)
    {
        mapContextController.PlayCloseWorldAnimations(world);
        mapContextController.SetOpenWorld(0);
    }
}
