using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Context for the Machine State Pattern of the level selector menu.
/// </summary>
public class MapContextController : MonoBehaviour
{
    public static MapContextController Instance;

    [SerializeField] private int        openWorld;
    private MapState                    worldsMapState;
    private MapState                    unlockingWorldsMapState;
    private MapState                    levelsMapState;
    private MapState                    mapState;
    private WorldSelectorAnimatedItem[] animatedItemsArray;
    private List<WorldSelectorAnimatedItem> animatedItemsList;
    private int                         loadWithUnlockWorld = 0;
    private LockScript[]                locks;

    /**************************************************************************************************
    Initializers
    **************************************************************************************************/

    private void    Awake() {
        Instance = this;
    }

    private void    Start()
    {
        InitializeStates();
        animatedItemsArray = FindObjectsOfType<WorldSelectorAnimatedItem>();
        locks = FindObjectsOfType<LockScript>();
        animatedItemsList = ArrayAnimatedItemsToList(animatedItemsArray);
        StartState();
        LoadMapState();
    }

    private List<WorldSelectorAnimatedItem> ArrayAnimatedItemsToList(WorldSelectorAnimatedItem[] array)
    {
        List<WorldSelectorAnimatedItem> list = new List<WorldSelectorAnimatedItem>();
        foreach(var x in array)
            list.Add(x);
        return list;
    }

    /**************************************************************************************************
    Preparation for Machine State Pattern
    **************************************************************************************************/

    /// <summary>
    /// Instantiate one object of each state.
    /// </summary>
    private void    InitializeStates()
    {
        worldsMapState = new WorldsMapState(this);
        unlockingWorldsMapState = new UnlockingWorldMapstate(this);
        levelsMapState = new LevelsMapState(this);
    }

    private void    StartState()
    {
        int worldLoad = GameProgressManager.instance.WorldSelection;
        if (worldLoad == 0)
        {
            SetMapState(worldsMapState);
            openWorld = 0;
        }
        else
        {
            SetMapState(levelsMapState);
            openWorld = worldLoad;
            if (loadWithUnlockWorld != 0)
                StartCoroutine(StartAtUnlockWorld(loadWithUnlockWorld));
        }
    }

    private void    LoadMapState()
    {
        if (mapState == worldsMapState)
            AnimationControlStartWorldsClosed();
        else
            AnimationControlStartWorldOpen(openWorld);
    }

    IEnumerator     StartAtUnlockWorld(int world)
    {
        yield return new WaitForSeconds(4);
        Debug.Log("Coroutine to unlock");
        mapState.UnlockWorldAction(world);
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
        foreach(var animatedItem in animatedItemsList)
            animatedItem.AnimationControlWorldSelected(world, selectWorldGO);
    }

    /// <summary>
    /// Order to all animated items to play animations when world is closed.
    /// </summary>
    /// <param name="world"> Number of closed world </param>
    public void AnimationControlWorldClosed(int world)
    {
        foreach(var animatedItem in animatedItemsList)
            animatedItem.AnimationControlWorldClosed(world);
    }

    /// <summary>
    /// Order to all animated items to play animations when world is unlocked.
    /// </summary>
    /// <param name="world"> Number of unlocked world </param>
    public void AnimationControlWorldUnlock(int world)
    {
        foreach(var animatedItem in animatedItemsList)
            animatedItem.AnimationControlWorldUnlock(world);
    }

    public void AnimationControlStartWorldOpen(int world)
    {
        foreach(var animatedItem in animatedItemsList)
            animatedItem.AnimationControlStartWorldOpen(world);
    }

    public void AnimationControlStartWorldsClosed()
    {
        foreach(var animatedItem in animatedItemsList)
            animatedItem.AnimationControlStartWorldsClosed();
    }

    /**************************************************************************************************
    Locks
    **************************************************************************************************/

    public void UpdateLocksState()
    {
        foreach(var mapLock in locks)
        {
            mapLock.UpdateLockState();
        }
    }

    public void FinishWorldCloseAnimation(int world)
    {
        if (world != 0)
        {
            mapState.UnlockWorldAction(world);
        }
    }

    /**************************************************************************************************
    Setters
    **************************************************************************************************/
    public void SetMapState (MapState mapState) => this.mapState = mapState;
    public void SetOpenWorld(int openWorld) => this.openWorld = openWorld;
    public void SetLoadWithUnlockWorld(int unlockWorld) => this.loadWithUnlockWorld = unlockWorld;

    /**************************************************************************************************
    Getters
    **************************************************************************************************/
    public MapState GetWorldsMapState() => worldsMapState;
    public MapState GetUnlockingWorldsMapState() => unlockingWorldsMapState;
    public MapState GetLevelsMapState() => levelsMapState;
    public MapState GetMapState() => mapState;
    public int      GetOpenWorld() => openWorld;
    public int      GetLoadWithUnlockWorld() => loadWithUnlockWorld;

}
