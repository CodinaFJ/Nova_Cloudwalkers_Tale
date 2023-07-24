using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using System;

public class LanguageController : MonoBehaviour
{
    [SerializeField] List<SpecialFontLanguage> specialFontsLanguages;
    [SerializeField] public TMPro.TMP_FontAsset defaultFont;
    public const string DEFAULT_LANGUAGE = "English";
    public static LanguageController instance;

    public TMPro.TMP_FontAsset activeFont;
    public static Action fontModified;

    private string activeLanguageName;
    public string ActiveLanguageName 
    {
        get => activeLanguageName; 
        set
        {
            activeLanguageName = value;
            SetFont();
            SetLanguageIDFromActiveName();
        }
    }
    public int ActiveLanguageID {get; set;} = -1;

    private bool active = false;

    private void Awake() 
    {
		if(instance == null)
           instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        activeLanguageName = DEFAULT_LANGUAGE;
    }

    private void SetLanguageIDFromActiveName()
    {
        for (ActiveLanguageID = 0; ActiveLanguageID < LocalizationSettings.AvailableLocales.Locales.Count; ActiveLanguageID++)
        {
            if (ActiveLanguageName == LocalizationSettings.AvailableLocales.Locales[ActiveLanguageID].ToString())
            {
                StartCoroutine(SetLanguage(ActiveLanguageID));
                return ;
            }
        }
        ActiveLanguageID = -1;
    }

    IEnumerator SetLanguage(int languageID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageID];
        active = false;
    }

    public void SetFont()
    {
        if (!specialFontsLanguages.Exists(x => x.language == activeLanguageName) && activeFont == defaultFont)
        {
            if (activeFont == defaultFont) 
                return;
            else
                activeFont = defaultFont;
        }
        else
            activeFont = specialFontsLanguages.Find(x => x.language == activeLanguageName).font;
        if (activeFont == null)
            activeFont = defaultFont;
        fontModified?.Invoke();
    }
}

[System.Serializable]
public struct SpecialFontLanguage{
    public TMPro.TMP_FontAsset font;
    public string language;
}
