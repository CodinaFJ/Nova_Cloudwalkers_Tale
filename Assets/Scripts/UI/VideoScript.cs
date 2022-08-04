using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoScript : MonoBehaviour
{
    [SerializeField] Animator textAnimator;
    [SerializeField] Button textButton;
    [SerializeField] float waitForFadeOut = 1f;
    public VideoPlayer vid;
 
    bool fadeInDone = false;
    bool fadingIn = false;
    bool fadeOutDone = true;
    bool fadingOut = false;

    const string FADE_IN_ANIMATION = "skipCinematic_FadeIn";
    const string FADE_OUT_ANIMATION = "skipCinematic_FadeOut";

    string currentState;

    float currentTime;
 
    void Start()
    {
        vid.url = System.IO.Path.Combine (Application.streamingAssetsPath,"cinematic1.mp4");

        vid.Play();     
        GetComponent<AudioSource>().Play();
        vid.loopPointReached += CheckOver;
        fadeInDone = false;
        fadingIn = false;
        fadeOutDone = true;
        fadingOut = false;
    }
    
    public void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        print("Video Is Over");
        FindObjectOfType<LevelLoader>().LoadLevel("LevelSelectorMenu");
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
