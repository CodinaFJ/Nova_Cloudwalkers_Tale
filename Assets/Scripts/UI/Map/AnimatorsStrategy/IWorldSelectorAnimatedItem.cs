using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSelectorAnimatedItem : MonoBehaviour
{

    private int worldNumber;//TODO: World number should be set parents of hierarchy

    public void Show()
    {
        Debug.Log("Show world from wherever it was");
    }
    public void Hide(int world)
    {
        Debug.Log("Hide world, this should change depending on the world given");
    }
    public void Unlock(int world)
    {
        if (worldNumber == world)
            Debug.Log("Do unlock world animation if there is any");
    }
    public void Open(int world)
    {
        if (worldNumber == world)
            Debug.Log("Do open animation if there is any");
        else
            Hide(world);
    }
    public void Close(int world)
    {
        if (worldNumber == world)
            Debug.Log("Do close animation if there is any");
    }

    //SETTERS
    public void SetWorldNumber(int worldNumber) => this.worldNumber = worldNumber;
}
