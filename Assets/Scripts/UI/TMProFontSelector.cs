using UnityEngine;
using TMPro;

public class TMProFontSelector : MonoBehaviour
{
    TextMeshProUGUI textPro;
    void Start()
    {
        if (textPro == null)
            textPro = GetComponent<TextMeshProUGUI>();
        UpdateFont();
    }

    private void OnEnable() 
    {
        if (textPro == null)
            textPro = GetComponent<TextMeshProUGUI>();
        LanguageController.fontModified += UpdateFont;
        UpdateFont();
    }

    private void OnDisable() 
    {
        LanguageController.fontModified -= UpdateFont;
    }

    
    void UpdateFont()
    {
        if (LanguageController.instance == null)
            return;
        if (textPro.font != LanguageController.instance.activeFont)
            textPro.font = LanguageController.instance.activeFont;
    }
}
