using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HotkeysScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
[SerializeField] GameObject hotkey;
// When highlighted with mouse.
public void OnPointerEnter(PointerEventData eventData)
{
    // Do something.
    if(hotkey != null) hotkey.SetActive(true);
}

public void OnPointerExit(PointerEventData eventData)
{
    // Do something.
    if(hotkey != null) hotkey.SetActive(false);
}

}
