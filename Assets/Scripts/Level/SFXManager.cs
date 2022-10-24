using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    //SFX names
    const string CLOUD_COLLISION = "CloudCollision";
    const string CLOUD_CONNECT = "CloudConnect";
    const string CLOUD_STORMY = "CloudStormy";
    const string CLOUD_SWIPE_LOOP = "CloudSwipe";
    const string CLOUD_SWIPE_RELEASE = "CloudSwipe_Release";
    const string CLOUD_SWIPE_TAP = "CloudSwipe_Tap";//+ SFX variation (1-3)
    const string UNDO = "Undo";

    private List<Vector2> tilesWithCloudToJoin = new List<Vector2>();

    private void Awake() {
        instance = this;
    }

    public static void PlayUndo() => AudioManager.instance.PlaySound(UNDO);

    

    public void PlayCloudCollision()
    {
        AudioManager.instance.PlaySound(CLOUD_COLLISION);
    }

    public void PlayCloudSwipeLoop()
    {
        AudioManager.instance.PlaySound(CLOUD_SWIPE_LOOP);
    }

    public void StopCloudSwipeLoop()
    {
        AudioManager.instance.Stop(CLOUD_SWIPE_LOOP);
    }

    public void PlayCloudSwipeTap()
    {
        int soundNumber = Random.Range(1,4);
        AudioManager.instance.PlaySound(CLOUD_SWIPE_TAP);
    }

    
    private void PlayCloudConnect() => PlayCloudConnect(TileType.WhiteCloud);
    private void PlayCloudConnect(int mechanicNumber){
        switch(mechanicNumber){
            case 1:
                PlayCloudConnect(TileType.WhiteCloud);
                break;
            
            case 2:
                break;

            case 3:
                PlayCloudConnect(TileType.CrystalCloudTop);
                break;

            case 4:
                PlayCloudConnect(TileType.CrystalCloudBot);
                break;

            case -1:
                PlayCloudConnect(TileType.ThunderCloud);
                break;

            default:
                PlayCloudConnect(TileType.WhiteCloud);
                break;
        }
    }
    private void PlayCloudConnect(TileType tileType) => AudioManager.instance.PlaySound(CLOUD_CONNECT + tileType.ToString());
    public void PlayCloudConnect(Vector2 coor){
        if(tilesWithCloudToJoin.Exists(x => x == coor)){
            PlayCloudConnect(MatrixManager.instance.GetMechanicsLayoutMatrix()[(int)coor.x, (int)coor.y]);
        }
    }

    public void SetCloudToJoin(int item){
        tilesWithCloudToJoin.Clear();

        int[,] itemLayoutMatrix = MatrixManager.instance.GetItemsLayoutMatrix();

        for (int i = 0; i < itemLayoutMatrix.GetLength(0); i++){
            for (int j = 0; j < itemLayoutMatrix.GetLength(1); j++){
                if(itemLayoutMatrix[i,j] == item) tilesWithCloudToJoin.Add(new Vector2(i,j));
            }
        }
    }
}
