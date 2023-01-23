using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITweenAnimator : MonoBehaviour
{
    [SerializeField] float fadeOutTime = 0.5f;
    [SerializeField] float moveCenterTime = 0.5f;
    //[SerializeField] float scaleUpTime = 0.5f;
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
        StartCoroutine(AnimationSelectedWorld(gameObject));
    }

    private void FadeOut() => FadeOut(gameObject.GetComponent<Image>());
    private void FadeOut(Image image) => StartCoroutine(FadeOut(image, this.fadeOutTime));
    private IEnumerator FadeOut(Image image, float fadeOutTime)
    {
        LeanTween.value(1f, 0f, fadeOutTime).setOnUpdate(SetFadeOutEasingvalue);
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

    private IEnumerator AnimationSelectedWorld(GameObject go){
		float eTime = 0;
		while (eTime < 0.1f/0.6f)
		{
			eTime += Time.deltaTime;
			yield return null;
		}
		FadeOut(go.GetComponent<Image>());
		LeanTween.scale(go, new Vector3(1.05f, 1.05f, 1.05f), 0.4f/0.6f);
		LeanTween.moveLocal(go, Vector3.zero, 0.4f/0.6f);
		eTime = 0;
		while (eTime < 0.4f/0.6f)
		{
			eTime += Time.deltaTime;
			yield return null;
		}
		LeanTween.scale(go, new Vector3(1.5f, 1.5f, 1.5f), 0.2f/0.6f);
    }

    private void ScaleTween(GameObject go, float scaleTime, float scaleEnd){
        LeanTween.scale(go, new Vector3(scaleEnd, scaleEnd, scaleEnd), scaleTime).setEaseInOutSine();
    }

    private void MoveCenter(GameObject go)
    {
        LeanTween.moveLocal(go, Vector3.zero, moveCenterTime).setEaseOutSine();
        StartCoroutine(FadeOutBigWhenCentered(go));
    }

    private IEnumerator FadeOutBigWhenCentered(GameObject go){
        while (go.transform.localPosition != Vector3.zero){
            yield return null;
        }
        FadeOut(go.GetComponent<Image>());
        ScaleTween(go, 0.4f, 1.05f);
    }

    private void DisableButton(){
        myButton.interactable = false;
    }

    private void SetFadeOutEasingvalue(float value) => fadeOutEasingValue = value;
}
