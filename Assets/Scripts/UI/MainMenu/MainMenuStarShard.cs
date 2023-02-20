using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStarShard : MonoBehaviour
{
    [SerializeField] GameObject[] childs;
    [SerializeField] int starsRequirement;

    void Start()
    {
        SetStatus();
    }

    private void SetStatus()
    {
        bool status = GameProgressManager.instance.GetCollectedStarsInGame() >= starsRequirement;

        foreach (GameObject child in childs)
            child.SetActive(status);
    }
}
