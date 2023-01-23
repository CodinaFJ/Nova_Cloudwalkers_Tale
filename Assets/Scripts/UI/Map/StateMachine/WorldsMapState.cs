using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldsMapState : MapState
{
    public WorldsMapState(MapContextController mapContextController) : base(mapContextController)
    {
    }

    //STATE MACHINE ACTIONS

	public override void CloseWorldAction()
	{
		throw new System.NotImplementedException("Cannot perform " + System.Reflection.MethodBase.GetCurrentMethod().Name + "while in " + this.GetType().Name);
	}

	public override void FinishUnlockWorldAction()
	{
		throw new System.NotImplementedException("Cannot perform " + System.Reflection.MethodBase.GetCurrentMethod().Name + "while in " + this.GetType().Name);
	}

	public override void SelectWorldAction(int world)
	{
		Debug.Log("Perform open world & perform hiding worlds");
        mapContextController.SetOpenWorld(world);
        mapContextController.SetMapState(mapContextController.GetLevelsMapState());
	}

	public override void UnlockWorldAction(int world)
	{
		mapContextController.SetMapState(mapContextController.GetUnlockingWorldsMapState());
        mapContextController.GetMapState().UnlockWorldAction(world);
	}
}
