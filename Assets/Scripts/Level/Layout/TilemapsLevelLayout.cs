using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapsLevelLayout : MonoBehaviour
{
    public static TilemapsLevelLayout instance;
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
        if(instance == null) instance = this;
    }

    public void FillCloudTilemaps() => cloudsTilemaps = cloudsGrid.GetComponentsInChildren<Tilemap>();
    public int GetNumberOfClouds() => cloudsTilemaps.Length;
    public float GetGridCellSize() => cloudsGrid.cellSize.x;

}
