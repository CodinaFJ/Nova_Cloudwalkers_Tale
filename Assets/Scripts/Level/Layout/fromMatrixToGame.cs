using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class fromMatrixToGame : MonoBehaviour
{
    [SerializeField] GameObject whiteCloud;
    [SerializeField] GameObject greyCloud;
    [SerializeField] GameObject crystalCloud;
    [SerializeField] GameObject thunderCloud;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject spikedFloor;
    [SerializeField] GameObject crystalFloor;
    [SerializeField] Grid whiteCloudsGrid;
    [SerializeField] GameObject cloudParent;
    [SerializeField] Tilemap floorTilemap;
    [SerializeField] Tilemap spikedFloorTilemap;
    TilemapRenderer floorTilemapRenderer;
    TilemapRenderer spikedFloorTilemapRenderer;
    Tilemap[] whiteCloudsTilemaps;
    TilemapRenderer[] whiteCloudsTilemapRederers;

    int numberOfClouds;

    TilemapsLevelLayout tilemapsLevelLayout;
    MatrixManager matrixManager;
    int[,] itemsLayoutMatrix;
    int[,] mechanicsLayoutMatrix;

    GameObject[] cloudsParents;
    GameObject walkableFloorParent;
    GameObject spikedFloorParent;

  
    InstantiatedCloudBehavior[] cloudsToDeactivate;


    void Awake()
    {
        matrixManager = FindObjectOfType<MatrixManager>();
        tilemapsLevelLayout = FindObjectOfType<TilemapsLevelLayout>();
        itemsLayoutMatrix = matrixManager.GetItemsLayoutMatrix();
        mechanicsLayoutMatrix = matrixManager.GetMechanicsLayoutMatrix();

        whiteCloudsTilemaps = whiteCloudsGrid.GetComponentsInChildren<Tilemap>();
        whiteCloudsTilemapRederers = whiteCloudsGrid.GetComponentsInChildren<TilemapRenderer>();

        floorTilemapRenderer = floorTilemap.GetComponent<TilemapRenderer>();
        spikedFloorTilemapRenderer = spikedFloorTilemap.GetComponent<TilemapRenderer>();

        numberOfClouds = tilemapsLevelLayout.GetNumberOfClouds();

        cloudsParents = new GameObject[numberOfClouds];
        walkableFloorParent = new GameObject("Floor");
        spikedFloorParent = new GameObject("Spiked Floor");

        for (int i = 0; i < numberOfClouds; i++)
        {
            cloudsParents[i] = Instantiate(cloudParent);
        }

        InstantiateLevel();
    }

    void InstantiateLevel()
    {
        for (int i = 0; i < itemsLayoutMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < itemsLayoutMatrix.GetLength(1); j++)
            {
                InstantiateTile(i, j);
            }
        }

        for(int i = 0; i < whiteCloudsTilemaps.Length; i++)
        {
            whiteCloudsTilemaps[i].enabled = false;
            whiteCloudsTilemapRederers[i].enabled = false;
        }

        floorTilemap.enabled = false;
        floorTilemapRenderer.enabled = false;

        spikedFloorTilemap.enabled = false;
        spikedFloorTilemapRenderer.enabled = false;
    
    }

    void InstantiateWhiteCloud(int i, int j, int whiteCloudNumber)
    {
        Vector3 itemPosition = matrixManager.FromMatrixIndexToWorld(i, j);

        Instantiate(whiteCloud, itemPosition, Quaternion.identity, cloudsParents[whiteCloudNumber].transform);
    }

    void InstantiateGreyCloud(int i, int j, int whiteCloudNumber)
    {
        Vector3 itemPosition = matrixManager.FromMatrixIndexToWorld(i, j);

        Instantiate(greyCloud, itemPosition, Quaternion.identity, cloudsParents[whiteCloudNumber].transform);
    }

    void InstantiateCrystalCloud(int i, int j, int whiteCloudNumber)
    {
        Vector3 itemPosition = matrixManager.FromMatrixIndexToWorld(i, j);

        Instantiate(crystalCloud, itemPosition, Quaternion.identity, cloudsParents[whiteCloudNumber].transform);
    }

    void InstantiateThunderCloud(int i, int j, int whiteCloudNumber)
    {
        Vector3 itemPosition = matrixManager.FromMatrixIndexToWorld(i, j);

        Instantiate(thunderCloud, itemPosition, Quaternion.identity, cloudsParents[whiteCloudNumber].transform);
    }

    void InstantiateFloor(int i, int j)
    {
        Vector3 itemPosition = matrixManager.FromMatrixIndexToWorld(i, j);

        Instantiate(floor, itemPosition, Quaternion.identity, walkableFloorParent.transform);

    }

    void InstantiateCrystalFloor(int i, int j)
    {
        Vector3 itemPosition = matrixManager.FromMatrixIndexToWorld(i, j);

        Instantiate(crystalFloor, itemPosition, Quaternion.identity, walkableFloorParent.transform);

    }

    void InstantiateSpikedFloor(int i, int j)
    {
        Vector3 itemPosition = matrixManager.FromMatrixIndexToWorld(i, j);

        Instantiate(spikedFloor, itemPosition, Quaternion.identity, spikedFloorParent.transform);
    }

    public void ReInstantiateItem (int item)
    {
        DeactivateItemCloud(item);
        for (int i = 0; i < itemsLayoutMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < itemsLayoutMatrix.GetLength(1); j++)
            {
                if(itemsLayoutMatrix[i,j] == item)
                {
                    InstantiateTile(i, j);
                }
                
            }
        }
    }

    public GameObject[] GetCloudsParents()
    {
        return cloudsParents;
    }

    public void DeactivateItem(int item)
    {
        if(item != matrixManager.valueForFloor && item != matrixManager.valueForCrystalFloor)
        {
            cloudsToDeactivate = cloudsParents[item - 1].GetComponentsInChildren<InstantiatedCloudBehavior>();
            DeactivateClouds();
            
        }
    }

    public void DeactivateItemCloud(int item)
    {
        if(item != matrixManager.valueForFloor && item != matrixManager.valueForCrystalFloor)
        {
            cloudsToDeactivate = cloudsParents[item - 1].GetComponentsInChildren<InstantiatedCloudBehavior>();
            Invoke("DeactivateClouds",0);
        }
    }

    void DeactivateClouds()
    {    
        foreach (InstantiatedCloudBehavior child in cloudsToDeactivate)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void CreateNewCloud()
    {
        GameObject[] result = new GameObject[cloudsParents.GetLength(0) + 1];

        for (int index = 0; index < cloudsParents.GetLength(0); index++)
        {
            result[index] = cloudsParents[index];
        }

        result[cloudsParents.GetLength(0)] = Instantiate(cloudParent);

        cloudsParents = result;
    }

    void InstantiateTile(int i, int j)
    {
        if (mechanicsLayoutMatrix[i,j] == matrixManager.valueWhiteCloudMechanic) InstantiateWhiteCloud(i,j, itemsLayoutMatrix[i,j] - 1);
        if (mechanicsLayoutMatrix[i,j] == matrixManager.valueGreyCloudMechanic) InstantiateGreyCloud(i,j, itemsLayoutMatrix[i,j] - 1);
        if (mechanicsLayoutMatrix[i,j] == matrixManager.valueCrystalCloudMechanic) InstantiateCrystalCloud(i,j, itemsLayoutMatrix[i,j] - 1);
        if (mechanicsLayoutMatrix[i,j] == matrixManager.valueThunderCloudMechanic) InstantiateThunderCloud(i,j, itemsLayoutMatrix[i,j] - 1);
        if (itemsLayoutMatrix[i,j] ==  matrixManager.valueForFloor)
        {
            if (mechanicsLayoutMatrix[i,j] ==  matrixManager.valueForFloor) InstantiateFloor(i,j);
            else if (mechanicsLayoutMatrix[i,j] == matrixManager.valueSpikedFloorMechanic) InstantiateFloor(i,j);
        }
        if (itemsLayoutMatrix[i,j] ==  matrixManager.valueForCrystalFloor) InstantiateCrystalFloor(i,j);
        if (mechanicsLayoutMatrix[i,j] == matrixManager.valueSpikedFloorMechanic) InstantiateSpikedFloor(i,j);
    }

    public void LoadLevelLayout()
    {
        itemsLayoutMatrix = matrixManager.GetItemsLayoutMatrix();
        mechanicsLayoutMatrix = matrixManager.GetMechanicsLayoutMatrix();
        foreach(GameObject movParticles in GameObject.FindGameObjectsWithTag("Part_mov"))
        {
            movParticles.SetActive(false);
            Destroy(movParticles);
        }

        foreach (GameObject child in cloudsParents)
        {
            child.SetActive(false);
            Destroy(child);
        }
        spikedFloorParent.SetActive(false);
        Destroy(spikedFloorParent);
        walkableFloorParent.SetActive(false);
        Destroy(walkableFloorParent);
        
        cloudsParents = new GameObject[numberOfClouds];
        walkableFloorParent = new GameObject("Floor");
        spikedFloorParent = new GameObject("Spiked Floor");

        for (int i = 0; i < numberOfClouds; i++)
        {
            cloudsParents[i] = Instantiate(cloudParent);
        }

        for (int i = 0; i < itemsLayoutMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < itemsLayoutMatrix.GetLength(1); j++)
            {
                InstantiateTile(i, j);
            }
        }

        /*for (int i = 0; i < numberOfClouds; i++)
        {
            foreach(Transform cloudTile in cloudsParents[i].transform)
            {
                if(cloudTile.GetComponent<InstantiatedCloudBehavior>() != null)
                cloudTile.GetComponent<InstantiatedCloudBehavior>().SelectCorrectSprite();
            }
        }*/
    }
}
