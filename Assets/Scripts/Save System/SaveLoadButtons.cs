using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadButtons : MonoBehaviour
{
    public void OnSave()
    {
        GameState.instance.SaveGameState();
    }

    public void OnLoad()
    {
        GameState.instance.LoadGameState();
    }
}
