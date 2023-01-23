using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapContextController : MonoBehaviour
{
    public static MapContextController Instance;

    private MapState worldsMapState;
    private MapState unlockingWorldsMapState;
    private MapState levelsMapState;
    private MapState mapState;

    private int openWorld;

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        InitializeStates();
    }

    private void InitializeStates()
    {
        worldsMapState = new WorldsMapState(this);
        unlockingWorldsMapState = new UnlockingWorldMapstate(this);
        levelsMapState = new LevelsMapState(this);
    }

    //SETTERS
    public void SetMapState (MapState mapState) => this.mapState = mapState;
    public void SetOpenWorld(int openWorld) => this.openWorld = openWorld;

    //GETTERS
    public MapState GetWorldsMapState() => worldsMapState;
    public MapState GetUnlockingWorldsMapState() => unlockingWorldsMapState;
    public MapState GetLevelsMapState() => levelsMapState;
    public MapState GetMapState() => mapState;
    public int GetOpenWorld() => openWorld;
}
