using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Context for the Machine State Pattern of the level selector menu.
/// </summary>
public class MapContextController : MonoBehaviour
{
    public static MapContextController Instance;

    private MapState worldsMapState;
    private MapState unlockingWorldsMapState;
    private MapState levelsMapState;
    private MapState mapState;

    private WorldSelectorAnimatedItem[] animatedItemsArray;

    private int openWorld;

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

    public void SelectWorld(GameObject selectWorldGO) => mapState.SelectWorldAction(selectWorldGO);
    public void CloseWorld(int world) => mapState.CloseWorldAction();

    /**************************************************************************************************
    Animation Control Methods
    **************************************************************************************************/

    public void PlaySelectWorldAnimations(int world, GameObject selectWorldGO)
    {
        foreach(var animatedItem in animatedItemsArray)
            animatedItem.PlaySelectWorldAnimation(world, selectWorldGO);
    }

    public void PlayCloseWorldAnimations(int world)
    {
        foreach(var animatedItem in animatedItemsArray)
            animatedItem.PlayCloseWorldAnimation(world);
    }

    public void PlayUnlockWorldAnimations(int world)
    {
        foreach(var animatedItem in animatedItemsArray)
            animatedItem.PlayUnlockWorldAnimation(world);
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
    public int GetOpenWorld() => openWorld;
}
