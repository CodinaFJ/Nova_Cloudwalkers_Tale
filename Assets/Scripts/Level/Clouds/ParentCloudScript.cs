using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ParentCloudScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] ParticleSystem particlesCloudOnClick;
    [SerializeField] float scaleTweenTime = 0.1f;

    //InstantiatedCloudBehavior[] cloudTiles;

    Vector3 posPrevious;
    Vector3 posDifference;

    bool pointerInCloud = false;
    bool cloudPointerClick = false;

    void Start() 
    {
        /*cloudTiles = GetComponentsInChildren<InstantiatedCloudBehavior>();
        posPrevious = transform.position;*/
    }

    private void Update() 
    {
        /*cloudTiles = GetComponentsInChildren<InstantiatedCloudBehavior>();
        posDifference = transform.position - posPrevious;

        foreach(InstantiatedCloudBehavior cloudTile in cloudTiles)
        {
            if(cloudTile.myMovementParticles != null) cloudTile.myMovementParticles.transform.position += posDifference;
        }
        posPrevious = transform.position;*/

    }

    

    public void PlayClickParticles(Vector3 ClickPos)
    {
        particlesCloudOnClick.transform.position = ClickPos;
        particlesCloudOnClick.Play();
        Invoke("StopParticles", 0.2f);
    }

    void StopParticles()
    {
        particlesCloudOnClick.Stop();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Do something.
        if(!cloudPointerClick)TweenCloudScaleIn();
        pointerInCloud = true;
        Debug.Log("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Do something.
        if(!cloudPointerClick)TweenCloudScaleOut();
        pointerInCloud = false;
        Debug.Log("OnPointerExit");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        cloudPointerClick = true;
        //TweenCloudScaleOut();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cloudPointerClick = false;
        TweenCloudScaleIn();
        if(!pointerInCloud)
        {
            Invoke("TweenCloudScaleOut", scaleTweenTime - 0.05f);
        }
    }


    void TweenCloudScaleIn()
    {
        foreach(TileBehavior cloudTile in gameObject.GetComponentsInChildren<TileBehavior>())
        {
            LeanTween.cancel(cloudTile.gameObject);
            LeanTween.scale(cloudTile.gameObject, new Vector3 (Mathf.Sign(cloudTile.transform.localScale.x) * 1.05f,Mathf.Sign(cloudTile.transform.localScale.y) * 1.05f, 1), scaleTweenTime).setEaseOutElastic();
        }
        
    }

    void TweenCloudScaleOut()
    {
        foreach(TileBehavior cloudTile in gameObject.GetComponentsInChildren<TileBehavior>())
        {
            LeanTween.cancel(cloudTile.gameObject);
            LeanTween.scale(cloudTile.gameObject, new Vector3 (Mathf.Sign(cloudTile.transform.localScale.x) * 1f,Mathf.Sign(cloudTile.transform.localScale.y) *1f, 1), scaleTweenTime).setEaseOutExpo(); 
        }
    }

}
