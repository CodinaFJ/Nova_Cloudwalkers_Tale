using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.EnhancedTouch;

public class VideoScript : MonoBehaviour
{
    [SerializeField] Animator textAnimator;
    [SerializeField] Button textButton;
    [SerializeField] float waitForFadeOut = 1f;
    [SerializeField] VideoClip videoClip;
    private int worldUnlocked;
    public VideoPlayer vid;
    double videoLength;
    double timeElapsed = 0.0;
 
    bool fadeInDone = false;
    bool fadingIn = false;
    bool fadeOutDone = true;
    bool fadingOut = false;

    const string FADE_IN_ANIMATION = "skipCinematic_FadeIn";
    const string FADE_OUT_ANIMATION = "skipCinematic_FadeOut";

    string currentState;

    float currentTime;

    private void OnEnable() {
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable() {
        TouchSimulation.Disable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
        EnhancedTouchSupport.Disable();
    }
 
    void Start()
    {
        foreach ( Sound sound in AudioManager.instance.musics)
        {
            if(AudioManager.instance.IsPlaying(sound.name)) StartCoroutine(AudioManager.instance.FadeOutMusic(sound.name));
        }

        //vid.url = System.IO.Path.Combine (Application.streamingAssetsPath,"cinematic1.mp4");
        //vid.clip = videoClip;
        vid.Play();     
        //GetComponent<AudioSource>().Play();
        vid.loopPointReached += CheckOver;
        fadeInDone = false;
        fadingIn = false;
        fadeOutDone = true;
        fadingOut = false;

        videoLength = vid.clip.length;

        InitializeWorldUnlocked();
        MouseMatrixScript.ReleasePointer();
    }

    private void Update() 
    {
        if(timeElapsed > 50) CheckOver(vid);
        timeElapsed += Time.deltaTime;
    }
    
    private void InitializeWorldUnlocked(){
        string levelNumberString = Regex.Match(SceneManager.GetActiveScene().name, @"\d+").Value;
        worldUnlocked = int.Parse(levelNumberString);
        Debug.Log("World Unlocked with this cinematic: " + worldUnlocked);
    }

    public void CheckOver(VideoPlayer vp)
    {
        print("Video Is Over");
        MouseMatrixScript.BlockPointer();

        GameProgressManager.instance.SetPlayedCinematic(worldUnlocked);
        if(worldUnlocked == 1)
        {
            string levelToLoad = LevelLoader.GetLevelContains("LevelSelectorMenu");
            if (levelToLoad == null)
                Debug.LogWarning("Level to Load not found");
            else
                LevelLoader.instance.LoadLevel(LevelLoader.GetLevelContains("LevelSelectorMenu"));
        } 
        else LevelLoader.instance.LoadLevel(LevelLoader.GetLevelContains("1-" + worldUnlocked));
    }

    private void FingerDown(Finger finger)
    {
        FadeInText();
    }

    public void OnAnyKey()
    {
        FadeInText();
    }

    public void OnAnyMouse()
    {
        FadeInText();
    }

    public void OnMouseDrag() 
    {
        FadeInText();
    }

    void FadeInText()
    {
        if(!fadeInDone && !fadingIn)
        {
            ChangeAnimationState(FADE_IN_ANIMATION);
            Invoke("EnableButton", 0.5f);
            fadingIn = true;

            Debug.Log(textAnimator.GetCurrentAnimatorStateInfo(0));
        }

        currentTime = 0f;
    }

    void EnableButton()
    {
        textButton.interactable = true;
        fadeInDone = true;
        fadingIn = false;
        fadeOutDone = false;
        StartCoroutine(WaitForFadeOut());
    }

    void FadeOutText()
    {
        if(!fadeOutDone && !fadingOut)
        {
            ChangeAnimationState(FADE_OUT_ANIMATION);
            Invoke("DisableButton", 0.5f);
            fadingOut = true;
        }
    }

    void DisableButton()
    {
        textButton.interactable = false;
        fadeInDone = false;
        fadeOutDone = true;
        fadingOut = false;
    }

    IEnumerator WaitForFadeOut()
    {
        currentTime = 0f;

        while (currentTime < waitForFadeOut)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        
        FadeOutText();
    }


    public void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if(currentState == newState) return;

        //play the animation
        textAnimator.Play(newState);

        //reassign the current state
        currentState = newState;
    }
}
