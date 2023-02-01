using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Context for the Machine State Pattern of the level selector menu.
/// </summary>
public class MapContextController : MonoBehaviour
{
    public static MapContextController Instance;

    private int                         openWorld;
    private MapState                    worldsMapState;
    private MapState                    unlockingWorldsMapState;
    private MapState                    levelsMapState;
    private MapState                    mapState;
    private WorldSelectorAnimatedItem[] animatedItemsArray;

    /**************************************************************************************************
    Initializers
    **************************************************************************************************/

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        InitializeStates();
        SetMapState(worldsMapState);
        animatedItemsArray = FindObjectsOfType<WorldSelectorAnimatedItem>();
    }

    /// <summary>
    /// Instantiate one object of each state.
    /// </summary>
    private void InitializeStates()
    {
        worldsMapState = new WorldsMapState(this);
        unlockingWorldsMapState = new UnlockingWorldMapstate(this);
        levelsMapState = new LevelsMapState(this);
    }

    /**************************************************************************************************
    Buttons Control Actions
    **************************************************************************************************/

    /// <summary> Called with UI world button. </summary>
    public void SelectWorld(GameObject selectWorldGO) => mapState.SelectWorldAction(selectWorldGO);

    /// <summary> Called with UI close world button. </summary>
    public void CloseWorld() => mapState.CloseWorldAction();

    /**************************************************************************************************
    Animation Control Methods
    **************************************************************************************************/

    /// <summary>
    /// Order to all animated items to play animations when world is selected.
    /// </summary>
    /// <param name="world"> Number of selected world </param>
    /// <param name="selectWorldGO"> Game Object of selected world </param>
    public void AnimationControlWorldSelected(int world, GameObject selectWorldGO)
    {
        foreach(var animatedItem in animatedItemsArray)
            animatedItem.AnimationControlWorldSelected(world, selectWorldGO);
    }

    /// <summary>
    /// Order to all animated items to play animations when world is closed.
    /// </summary>
    /// <param name="world"> Number of closed world </param>
    public void AnimationControlWorldClosed(int world)
    {
        foreach(var animatedItem in animatedItemsArray)
            animatedItem.AnimationControlWorldClosed(world);
    }

    /// <summary>
    /// Order to all animated items to play animations when world is unlocked.
    /// </summary>
    /// <param name="world"> Number of unlocked world </param>
    public void AnimationControlWorldUnlock(int world)
    {
        foreach(var animatedItem in animatedItemsArray)
            animatedItem.AnimationControlWorldUnlock(world);
    }

    /**************************************************************************************************
    Setters
    **************************************************************************************************/
    public void SetMapState (MapState mapState) => this.mapState = mapState;
    public void SetOpenWorld(int openWorld) => this.openWorld = openWorld;

    /**************************************************************************************************
    Getters
    **************************************************************************************************/
    public MapState GetWorldsMapState() => worldsMapState;
    public MapState GetUnlockingWorldsMapState() => unlockingWorldsMapState;
    public MapState GetLevelsMapState() => levelsMapState;
    public MapState GetMapState() => mapState;
    public int      GetOpenWorld() => openWorld;
}
