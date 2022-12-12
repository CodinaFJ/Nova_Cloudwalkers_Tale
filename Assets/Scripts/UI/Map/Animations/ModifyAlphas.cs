using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModifyAlphas : MonoBehaviour
{
    Image[] images;
    TextMeshProUGUI[] texts;
    [SerializeField]
    float alphas = 1;

    void Start()
    {
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Image image in images)
        {
            Color color = image.color;
            color.a = alphas; 
            image.color = color;
        }
        foreach(TextMeshProUGUI text in texts)
        {
            Color color = text.color;
            color.a = alphas;
            text.color = color;
        }
    }
}
