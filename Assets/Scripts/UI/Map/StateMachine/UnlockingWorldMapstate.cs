using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockingWorldMapstate : MapState
{
    public UnlockingWorldMapstate(MapContextController mapContextController) : base(mapContextController)
    {
    }

    //STATE MACHINE ACTIONS

    public override void CloseWorldAction()
	{
		throw new System.NotImplementedException("Cannot perform " + System.Reflection.MethodBase.GetCurrentMethod().Name + "while in " + this.GetType().Name);
	}

	public override void FinishUnlockWorldAction()
	{
		mapContextController.SetMapState(mapContextController.GetWorldsMapState());
	}

	public override void SelectWorldAction(int world)
	{
		throw new System.NotImplementedException("Cannot perform " + System.Reflection.MethodBase.GetCurrentMethod().Name + "while in " + this.GetType().Name);
	}

	public override void UnlockWorldAction(int world)
	{
		Debug.Log("Unlocking world: " + world);
        Debug.Log("Waiting for unlocking world " + world + " to end");
        FinishUnlockWorldAction();
	}
}
