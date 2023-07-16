using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWorldButtonHover : UIButtonScaleOnEnter
{
    Button worldButton;

    private void Start() 
    {
        worldButton = this.gameObject.GetComponent<Button>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (worldButton.interactable)
            base.OnPointerEnter(eventData);
    }
}
