

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapsLevelLayout : MonoBehaviour
{
    int[,] levelLayoutMatrix;
    Vector3 coordinatesOriginMatrix = new Vector3 (0f,0f,0f);

    public Tilemap borderTilemap;
    [SerializeField] Grid cloudsGrid;

    public Tilemap floorTilemap;
    public Tilemap spikedFloorTilemap;
    [HideInInspector]
    public Tilemap[] cloudsTilemaps;

    public Tilemap blockTilemap;

    void Awake()
    {

    }

    void Update()
    {
        
    }

    public void FillCloudTilemaps()
    {
        cloudsTilemaps = cloudsGrid.GetComponentsInChildren<Tilemap>();
    }

    public int GetNumberOfClouds()
    {
        return cloudsTilemaps.Length;
    }

    public float GetGridCellSize()
    {
        return cloudsGrid.cellSize.x;
    }

}
