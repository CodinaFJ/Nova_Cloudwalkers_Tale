using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIButtonHover : MonoBehaviour, IPointerEnterHandler
{


    public void OnPointerEnter(PointerEventData eventData){
        SFXManager.PlayHoverUI();
    }

}
