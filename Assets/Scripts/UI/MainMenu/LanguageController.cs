using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageController : MonoBehaviour
{
    public const string DEFAULT_LANGUAGE = "English";
    public static LanguageController instance;

    private string activeLanguageName;
    public string ActiveLanguageName 
    {
        get => activeLanguageName; 
        set
        {
            activeLanguageName = value;
            SetLanguageIDFromActiveName();
        }
    }
    public int ActiveLanguageID {get; set;} = -1;

    private bool active = false;

    private void Awake() 
    {
		instance = this;
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
}
