using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsMapState : MapState
{
    public LevelsMapState(MapContextController mapContextController) : base(mapContextController)
    {
    }

    //STATE MACHINE ACTIONS

	public override void CloseWorldAction()
	{
		CloseWorld(mapContextController.GetOpenWorld());
        mapContextController.SetMapState(mapContextController.GetWorldsMapState());
	}

	public override void FinishUnlockWorldAction()
	{
		throw new System.NotImplementedException("Cannot perform " + System.Reflection.MethodBase.GetCurrentMethod().Name + "while in " + this.GetType().Name);
	}

	public override void SelectWorldAction(int world)
	{
		throw new System.NotImplementedException("Cannot perform " + System.Reflection.MethodBase.GetCurrentMethod().Name + "while in " + this.GetType().Name);
	}

	public override void UnlockWorldAction(int world)
	{
		CloseWorld(mapContextController.GetOpenWorld());
        Debug.Log("Waiting for world to be closed");
        mapContextController.GetMapState().UnlockWorldAction(world);
	}

    //METHODS

    private void CloseWorld(int world)
    {
        Debug.Log("Closing World");
        mapContextController.SetOpenWorld(0);
    }
}
