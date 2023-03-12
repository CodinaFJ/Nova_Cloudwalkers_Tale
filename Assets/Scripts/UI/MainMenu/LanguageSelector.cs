using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{
    private bool active = false;

    [SerializeField] Button resolutionRightButton;
    [SerializeField] Button resolutionLeftButton;

    private void Start()
    {
        InitializeLanguage();
        InitializeLanguageButtons();
    }

    private void OnEnable()
    {
        InitializeLanguage();
        InitializeLanguageButtons();
    }

    private void InitializeLanguageButtons()
    {
        if(LanguageController.ActiveLanguageID <= 0) resolutionLeftButton.interactable = false;
        else resolutionLeftButton.interactable = true;

        if(LanguageController.ActiveLanguageID >= LocalizationSettings.AvailableLocales.Locales.Count - 1) resolutionRightButton.interactable = false; 
        else resolutionRightButton.interactable = true;
    }

    public void NextLanguage()
    {
        LanguageController.ActiveLanguageID++;
        SFXManager.PlaySelectUI_F();
        ChangeLanguage(LanguageController.ActiveLanguageID);

        if(LanguageController.ActiveLanguageID == 0) resolutionLeftButton.interactable = false;
        else if(!resolutionLeftButton.interactable) resolutionLeftButton.interactable = true;

        
        if(LanguageController.ActiveLanguageID == LocalizationSettings.AvailableLocales.Locales.Count - 1) resolutionRightButton.interactable = false; 
        else if (!resolutionRightButton.interactable) resolutionRightButton.interactable = true;
        
    }

    public void PreviousLanguage()
    {
        LanguageController.ActiveLanguageID--;
        SFXManager.PlaySelectUI_B();
        ChangeLanguage(LanguageController.ActiveLanguageID);

        if(LanguageController.ActiveLanguageID <= 0) resolutionLeftButton.interactable = false;
        else if(!resolutionLeftButton.interactable) resolutionLeftButton.interactable = true;
        
        if(LanguageController.ActiveLanguageID >= LocalizationSettings.AvailableLocales.Locales.Count - 1) resolutionRightButton.interactable = false; 
        else if (!resolutionRightButton.interactable) resolutionRightButton.interactable = true;
    }

    public void ChangeLanguage(int languageID)
    {
        Debug.Log("ChangeLanguage 1");
        if (active)
            return;
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[languageID])
            return;
        Debug.Log("ChangeLanguage 2");
        StartCoroutine(SetLanguage(languageID));
    }

    IEnumerator SetLanguage(int languageID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageID];
        LanguageController.ActiveLanguageName = LocalizationSettings.SelectedLocale.LocaleName;
        active = false;
    }

    private void InitializeLanguage()
    {
        if (LanguageController.ActiveLanguageID == -1)
        {
            for (LanguageController.ActiveLanguageID = 0; LanguageController.ActiveLanguageID < LocalizationSettings.AvailableLocales.Locales.Count; LanguageController.ActiveLanguageID++)
            {
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[LanguageController.ActiveLanguageID])
                    break ;
            }
        }
        else
            ChangeLanguage(LanguageController.ActiveLanguageID);
    }
}

public static class LanguageController
{
    public const string DEFAULT_LANGUAGE = "English";

    public static string ActiveLanguageName {get; set;} = DEFAULT_LANGUAGE;
    public static int ActiveLanguageID {get; set;} = -1;
}
