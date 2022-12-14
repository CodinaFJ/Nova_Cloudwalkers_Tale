using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadButtons : MonoBehaviour
{
    public void OnSave()
    {
        GameProgressManager.instance.SaveGameState();
    }

    public void OnLoad()
    {
        GameProgressManager.instance.LoadGameState();
    }
}
