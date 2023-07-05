using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{
    private bool active = false;

    [SerializeField] Button languageRightButton;
    [SerializeField] Button languageLeftButton;

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

    private void InitializeLanguage()
    {
        if (LanguageController.instance.ActiveLanguageID == -1)
        {
            for (LanguageController.instance.ActiveLanguageID = 0; LanguageController.instance.ActiveLanguageID < LocalizationSettings.AvailableLocales.Locales.Count; LanguageController.instance.ActiveLanguageID++)
            {
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[LanguageController.instance.ActiveLanguageID])
                    break ;
            }
        }
        else
            ChangeLanguage(LanguageController.instance.ActiveLanguageID);
    }

    private void InitializeLanguageButtons()
    {
        if(LanguageController.instance.ActiveLanguageID <= 0) languageLeftButton.interactable = false;
        else languageLeftButton.interactable = true;

        if(LanguageController.instance.ActiveLanguageID >= LocalizationSettings.AvailableLocales.Locales.Count - 1) languageRightButton.interactable = false; 
        else languageRightButton.interactable = true;
    }

    public void NextLanguage()
    {
        LanguageController.instance.ActiveLanguageID++;
        SFXManager.PlaySelectUI_F();
        ChangeLanguage(LanguageController.instance.ActiveLanguageID);

        if(LanguageController.instance.ActiveLanguageID == 0) languageLeftButton.interactable = false;
        else if(!languageLeftButton.interactable) languageLeftButton.interactable = true;

        
        if(LanguageController.instance.ActiveLanguageID == LocalizationSettings.AvailableLocales.Locales.Count - 1) languageRightButton.interactable = false; 
        else if (!languageRightButton.interactable) languageRightButton.interactable = true;
        
    }

    public void PreviousLanguage()
    {
        LanguageController.instance.ActiveLanguageID--;
        SFXManager.PlaySelectUI_B();
        ChangeLanguage(LanguageController.instance.ActiveLanguageID);

        if(LanguageController.instance.ActiveLanguageID <= 0) languageLeftButton.interactable = false;
        else if(!languageLeftButton.interactable) languageLeftButton.interactable = true;
        
        if(LanguageController.instance.ActiveLanguageID >= LocalizationSettings.AvailableLocales.Locales.Count - 1) languageRightButton.interactable = false; 
        else if (!languageRightButton.interactable) languageRightButton.interactable = true;
    }

    public void ChangeLanguage(int languageID)
    {
        if (active)
            return;
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[languageID])
            return;
        StartCoroutine(SetLanguage(languageID));
    }

    IEnumerator SetLanguage(int languageID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageID];
        LanguageController.instance.ActiveLanguageName = LocalizationSettings.SelectedLocale.LocaleName;
        SaveSystem.SaveConfigData();
        active = false;
    }
}