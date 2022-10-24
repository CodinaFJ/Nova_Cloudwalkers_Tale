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


    

    public void PlayCloudSwipeTap()
    {
        int soundNumber = Random.Range(1,4);
        AudioManager.instance.PlaySound(CLOUD_SWIPE_TAP);
    }

    public void StopCloudSwipeLoop(){
        AudioManager.instance.Stop(CLOUD_SWIPE_LOOP + TileType.WhiteCloud);
        AudioManager.instance.Stop(CLOUD_SWIPE_LOOP + TileType.CrystalCloudTop);
        AudioManager.instance.Stop(CLOUD_SWIPE_LOOP + TileType.ThunderCloud);
    }

    public void PlayCloudSwipeLoop() => PlayCloudSwipeLoop(TileType.WhiteCloud);
    public void PlayCloudSwipeLoop(int mechanicNumber) => PlayCloudSwipeLoop(TileBehavior.MechanicNumberToTyleTipe(mechanicNumber));
    public void PlayCloudSwipeLoop(TileType tileType) => AudioManager.instance.PlaySound(CLOUD_SWIPE_LOOP + tileType.ToString());
    
    private void PlayCloudConnect() => PlayCloudConnect(TileType.WhiteCloud);
    public void PlayCloudConnect(Vector2 coor){
        if(tilesWithCloudToJoin.Exists(x => x == coor)){
            PlayCloudConnect(MatrixManager.instance.GetMechanicsLayoutMatrix()[(int)coor.x, (int)coor.y]);
        }
    }
    private void PlayCloudConnect(int mechanicNumber) => PlayCloudConnect(TileBehavior.MechanicNumberToTyleTipe(mechanicNumber));
    private void PlayCloudConnect(TileType tileType) => AudioManager.instance.PlaySound(CLOUD_CONNECT + tileType.ToString());

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
