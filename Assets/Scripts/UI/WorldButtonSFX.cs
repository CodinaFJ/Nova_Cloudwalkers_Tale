 using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    Button worldButton;
    [HideInInspector]
    bool sfxEnabled = true;

    private void Start() 
    {
        worldButton = this.gameObject.GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!worldButton.interactable)
            return ;
        if (sfxEnabled) SFXManager.PlayHoverWorld();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!worldButton.interactable)
            return ;
        if (sfxEnabled) SFXManager.PlaySelectWorld();
    }

    public void SetSfxEnabled(bool value) => sfxEnabled = value;
}
