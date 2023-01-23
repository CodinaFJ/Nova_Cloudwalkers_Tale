using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapState
{
    protected MapContextController mapContextController;

    public MapState(MapContextController mapContextController)
    {
        this.mapContextController = mapContextController;
    }

    //STATE MACHINE ACTIONS

    public abstract void CloseWorldAction();
    public abstract void FinishUnlockWorldAction();
    public abstract void SelectWorldAction(int world);
    public abstract void UnlockWorldAction(int world);
}
