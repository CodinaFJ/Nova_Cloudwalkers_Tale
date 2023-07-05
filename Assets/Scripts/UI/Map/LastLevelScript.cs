using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastLevelScript : MonoBehaviour
{
    [SerializeField] Sprite unlockedSprite;
    [SerializeField] Sprite lockedSprite;
    [SerializeField] Button levelButton;
    Image haloImage;

    private void Start() {
        haloImage = GetComponent<Image>();
    }

    void Update()
    {
        if (levelButton.interactable)
        {
            haloImage.sprite = unlockedSprite;
        }
        else
            haloImage.sprite = lockedSprite;
    }
}
