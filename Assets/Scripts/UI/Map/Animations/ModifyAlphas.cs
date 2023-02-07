using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModifyAlphas : MonoBehaviour
{
    [SerializeField] private float alphas = 1;
    [SerializeField] bool onlyImages = false;

    private Image[] images;
    private Button[] buttons;
    private TextMeshProUGUI[] texts;
    private Color color;

    /*Properties*/

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

    void Start()
    {
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<TextMeshProUGUI>();
        buttons = GetComponentsInChildren<Button>();
    }

    void Update()
    {
        //TODO: The right way of doing this would probably be to have each object identify its alpha from a parent.
        foreach(Image image in images)
        {
            color = image.color;
            color.a = alphas; 
            image.color = color;
        }
        foreach(TextMeshProUGUI text in texts)
        {
            color = text.color;
            color.a = alphas;
            text.color = color;
        }
        //! Probably I should not do this
        foreach(Button button in buttons)
        {
            if (alphas == 0)
                button.gameObject.SetActive(false);
            else
                button.gameObject.SetActive(true);
        }
    }
}
