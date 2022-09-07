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

  
    Transform[] cloudsToDeactivate;


    void Awake()
    {
        tilemapsLevelLayout = FindObjectOfType<TilemapsLevelLayout>();
        itemsLayoutMatrix = MatrixManager.instance.GetItemsLayoutMatrix();
        mechanicsLayoutMatrix = MatrixManager.instance.GetMechanicsLayoutMatrix();

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
        Vector3 itemPosition = MatrixManager.instance.FromMatrixIndexToWorld(i, j);

        Instantiate(whiteCloud, itemPosition, Quaternion.identity, cloudsParents[whiteCloudNumber].transform);
    }

    void InstantiateGreyCloud(int i, int j, int whiteCloudNumber)
    {
        Vector3 itemPosition = MatrixManager.instance.FromMatrixIndexToWorld(i, j);

        Instantiate(greyCloud, itemPosition, Quaternion.identity, cloudsParents[whiteCloudNumber].transform);
    }

    void InstantiateCrystalCloud(int i, int j, int whiteCloudNumber)
    {
        Vector3 itemPosition = MatrixManager.instance.FromMatrixIndexToWorld(i, j);

        Instantiate(crystalCloud, itemPosition, Quaternion.identity, cloudsParents[whiteCloudNumber].transform);
    }

    void InstantiateThunderCloud(int i, int j, int whiteCloudNumber)
    {
        Vector3 itemPosition = MatrixManager.instance.FromMatrixIndexToWorld(i, j);

        Instantiate(thunderCloud, itemPosition, Quaternion.identity, cloudsParents[whiteCloudNumber].transform);
    }

    void InstantiateFloor(int i, int j)
    {
        Vector3 itemPosition = MatrixManager.instance.FromMatrixIndexToWorld(i, j);

        Instantiate(floor, itemPosition, Quaternion.identity, walkableFloorParent.transform);

    }

    void InstantiateCrystalFloor(int i, int j)
    {
        Vector3 itemPosition = MatrixManager.instance.FromMatrixIndexToWorld(i, j);

        Instantiate(crystalFloor, itemPosition, Quaternion.identity, walkableFloorParent.transform);

    }

    void InstantiateSpikedFloor(int i, int j)
    {
        Vector3 itemPosition = MatrixManager.instance.FromMatrixIndexToWorld(i, j);

        Instantiate(spikedFloor, itemPosition, Quaternion.identity, spikedFloorParent.transform);
    }

    public void ReInstantiateItem (int item)
    {
        //DeactivateItemCloud(item);
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
        if(item != MatrixManager.instance.valueForFloor && item != MatrixManager.instance.valueForCrystalFloor)
        {
            cloudsToDeactivate = cloudsParents[item - 1].GetComponentsInChildren<Transform>();
            DeactivateClouds();
            
        }
    }

    public void DeactivateItemCloud(int item)
    {
        if(item != MatrixManager.instance.valueForFloor && item != MatrixManager.instance.valueForCrystalFloor)
        {
            cloudsToDeactivate = cloudsParents[item - 1].GetComponentsInChildren<Transform>();
            Invoke("DeactivateClouds",0);
        }
    }

    void DeactivateClouds()
    {    
        foreach (Transform child in cloudsToDeactivate)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void CreateNewCloud()
    {
        GameObject[] result = new GameObject[cloudsParents.Length + 1];

        for (int index = 0; index < cloudsParents.Length; index++)
        {
            result[index] = cloudsParents[index];
        }

        result[cloudsParents.Length] = Instantiate(cloudParent);

        cloudsParents = result;
    }

    void InstantiateTile(int i, int j)
    {
        if (mechanicsLayoutMatrix[i,j] == MatrixManager.instance.valueWhiteCloudMechanic) InstantiateWhiteCloud(i,j, itemsLayoutMatrix[i,j] - 1);

        if (mechanicsLayoutMatrix[i,j] == MatrixManager.instance.valueGreyCloudMechanic) InstantiateGreyCloud(i,j, itemsLayoutMatrix[i,j] - 1);

        if (mechanicsLayoutMatrix[i,j] == MatrixManager.instance.valueCrystalCloudMechanic || 
            mechanicsLayoutMatrix[i,j] == (MatrixManager.instance.valueCrystalCloudMechanic + 1)) InstantiateCrystalCloud(i,j, itemsLayoutMatrix[i,j] - 1);

        if (mechanicsLayoutMatrix[i,j] == MatrixManager.instance.valueThunderCloudMechanic) InstantiateThunderCloud(i,j, itemsLayoutMatrix[i,j] - 1);

        if (itemsLayoutMatrix[i,j] ==  MatrixManager.instance.valueForFloor)
        {
            if (mechanicsLayoutMatrix[i,j] ==  MatrixManager.instance.valueForFloor) InstantiateFloor(i,j);
            else if (mechanicsLayoutMatrix[i,j] == MatrixManager.instance.valueSpikedFloorMechanic) InstantiateFloor(i,j);
        }
        if (mechanicsLayoutMatrix[i,j] ==  MatrixManager.instance.valueCrystalFloorMechanic ||
            mechanicsLayoutMatrix[i,j] ==  (MatrixManager.instance.valueCrystalFloorMechanic + 1)) InstantiateCrystalFloor(i,j);

        if (mechanicsLayoutMatrix[i,j] == MatrixManager.instance.valueSpikedFloorMechanic) InstantiateSpikedFloor(i,j);
    }

    public void LoadLevelLayout()
    {
        itemsLayoutMatrix = MatrixManager.instance.GetItemsLayoutMatrix();
        mechanicsLayoutMatrix = MatrixManager.instance.GetMechanicsLayoutMatrix();
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

        numberOfClouds = GetNumberOfClouds(itemsLayoutMatrix);
        
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
    }


    private int GetNumberOfClouds(int[,] itemsLayoutMatrix)
    {
        int highestItemInMatrix = 0;

        for (int i = 0; i < itemsLayoutMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < itemsLayoutMatrix.GetLength(1); j++)
            {
                if(itemsLayoutMatrix[i,j] > 0 && itemsLayoutMatrix[i,j] < 999)
                {
                    if(itemsLayoutMatrix[i,j] > highestItemInMatrix) highestItemInMatrix = itemsLayoutMatrix[i,j];
                }
            }
        }

        return highestItemInMatrix;
    }
}
