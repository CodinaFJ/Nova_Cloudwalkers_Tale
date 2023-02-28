using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBackgroundSelector : MonoBehaviour
{
    [SerializeField] List<MainMenuBackground> backgroundsList;
    [SerializeField] int lastPlayedCinematic;

    void Start()
    {
        SetMainMenubackground();
    }

    private void SetMainMenubackground()
    {
        lastPlayedCinematic = GameProgressManager.instance.GetLastPlayedCinematic();

        foreach(var bg in backgroundsList)
        {
            if (bg.worldNumber == lastPlayedCinematic)
                bg.backgroundGO.SetActive(true);
            else
                bg.backgroundGO.SetActive(false);
        }
    }
}

[System.Serializable]
public struct MainMenuBackground{
    public GameObject  backgroundGO;
    public int         worldNumber;
}