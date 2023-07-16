 using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    Button worldButton;

    private void Start() 
    {
        worldButton = this.gameObject.GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!worldButton.interactable)
            return ;
        SFXManager.PlayHoverWorld();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!worldButton.interactable)
            return ;
        SFXManager.PlaySelectWorld();
    }
}
