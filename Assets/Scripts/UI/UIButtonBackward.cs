using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonBackward : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData){
        SFXManager.PlaySelectUI_B();
    }

}
