using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    //SFX names
    const string CLOUD_COLLISION = "CloudCollision";
    const string CLOUD_CONNECT = "CloudConnect";
    const string CLOUD_STORMY = "CloudStormy";
    const string CLOUD_SWIPE_LOOP = "CloudSwipe_Loop";
    const string CLOUD_SWIPE_RELEASE = "CloudSwipe_Release";
    const string CLOUD_SWIPE_TAP = "CloudSwipe_Tap";//+ SFX variation (1-3)
    const string UNDO = "Undo";

    public static SFXManager instance;

    private void Awake() {
        instance = this;
    }

    public static void PlayUndo() => AudioManager.instance.PlaySound(UNDO);
    public void PlayCloudConnect() => AudioManager.instance.PlaySound(CLOUD_CONNECT);

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

}
