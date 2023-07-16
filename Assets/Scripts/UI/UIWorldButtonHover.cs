using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWorldButtonHover : UIButtonScaleOnEnter
{
    Button worldButton;
    Image worldImage;
    WorldSelectorAnimatedItem worldSelectorAnimatedItem;

    private void Start() 
    {
        worldButton = this.gameObject.GetComponent<Button>();
        worldImage = this.gameObject.GetComponent<Image>();
        worldSelectorAnimatedItem = this.gameObject.GetComponent<WorldSelectorAnimatedItem>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (worldButton.interactable && worldImage.color.a == 1)
        {
            worldSelectorAnimatedItem.PlayHoverAnimation(true);
            base.OnPointerEnter(eventData);
        }
    }

    public override void OnPointerExit(PointerEventData eventData){
        if (worldButton.interactable && worldImage.color.a == 1)
        {
            worldSelectorAnimatedItem.PlayHoverAnimation(false);
            base.OnPointerExit(eventData);
        }
    }
}
