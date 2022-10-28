using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonForward : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData){
        SFXManager.PlaySelectUI_F();
    }
}
