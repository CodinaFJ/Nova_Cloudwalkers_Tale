using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModifyAlphas : MonoBehaviour
{
    Image[] images;
    Button[] buttons;
    TextMeshProUGUI[] texts;
    [SerializeField]
    private float alphas = 1;
    public float Alphas { 
        get => alphas; 
        set
        {
            if (value == 0 || value == 1)
                alphas = value;
            else
                Debug.LogWarning(("ModifyAlphas: Wrong alphas"));
        }
    }
    [SerializeField]
    int worldNumber;

    void Start()
    {
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<TextMeshProUGUI>();
        buttons = GetComponentsInChildren<Button>();
        if (worldNumber == GameProgressManager.instance.WorldSelection)
            alphas = 1;
        else
            alphas = 0;
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
        foreach(Button button in buttons)
        {
            if (alphas == 0)
                button.gameObject.SetActive(false);
            else
                button.gameObject.SetActive(true);
        }
    }
}
