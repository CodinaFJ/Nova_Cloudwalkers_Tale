using UnityEngine;
using UnityEngine.UI;

public class ThanksForPlayingButtons : MonoBehaviour
{

    [SerializeField] Button mapButton;
    [SerializeField] Button menuButton;

    const string MAIN_MENU = "StartMenu";
    const string MAP_MENU = "LevelSelectorMenu";

    public void ToMap()
    {
        LevelLoader.instance.LoadLevel(MAP_MENU);
    }

    public void ToMainMenu()
    {
        LevelLoader.instance.LoadLevel(MAIN_MENU);
    }
}
