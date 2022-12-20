using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITweenAnimator : MonoBehaviour
{
    [SerializeField] float fadeOutTime = 0.5f;
    [SerializeField] float moveCenterTime = 0.5f;
    Button myButton;
    private float fadeOutEasingValue;
    // Start is called before the first frame update
    void Start()
    {
        myButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnWorldButton()
    {
        MoveCenter(gameObject);
    }

    private void FadeOut() => FadeOut(gameObject.GetComponent<Image>());
    private void FadeOut(Image image) => StartCoroutine(FadeOut(image, this.fadeOutTime));
    private IEnumerator FadeOut(Image image, float fadeOutTime)
    {
        LeanTween.value(1f, 0f, fadeOutTime).setEaseOutExpo().setOnUpdate(SetFadeOutEasingvalue);
        float time = 0;
        while(time < fadeOutTime){
            time += Time.deltaTime;
            Color color = image.color;
            color.a = fadeOutEasingValue;
            image.color = color;
            yield return null;
        }
        DisableButton();
    }

    private void MoveCenter(GameObject go)
    {
        LeanTween.moveLocal(go, Vector3.zero, moveCenterTime).setEaseOutSine();
        StartCoroutine(FadeOutWhenCentered(go));
    }

    private IEnumerator FadeOutWhenCentered(GameObject go){
        while (go.transform.localPosition != Vector3.zero){
            yield return null;
        }
        FadeOut(go.GetComponent<Image>());
    }

    private void DisableButton(){
        myButton.interactable = false;
    }

    private void SetFadeOutEasingvalue(float value) => fadeOutEasingValue = value;
}
