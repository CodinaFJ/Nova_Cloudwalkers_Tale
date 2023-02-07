using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controller of UI in levels and in level selector.
/// </summary>
public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] GameObject OptionCanvas;
    [SerializeField] GameObject levelsUIGO;
    [SerializeField] GameObject worldsUIGO;

    LevelLoader levelLoader;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        OptionCanvas.SetActive(false);
        MouseMatrixScript.ReleasePointer();//! This is also in level selector controller. Redundant?
    }

    /**************************************************************************************************
    LEVEL SELECTOR UI METHODS
    **************************************************************************************************/

    public void ToOptionsMap()
    {
        OptionCanvas.SetActive(true);
        OptionCanvas.GetComponent<OptionsMenuController>().ToPauseMap();
        SFXManager.PlayOpenMenu();
    }
    
    /**************************************************************************************************
    IN-LEVEL UI METHODS
    **************************************************************************************************/

    public void undoButton()
    {
        LevelStateManager.instance.OnUndo();
    }

    public void ToOptionsLevel()
    {
        SFXManager.PlayOpenMenu();
        if(GameManager.instance != null)
            GameManager.instance.PauseGame();
        OptionCanvas.SetActive(true);
        OptionCanvas.GetComponent<PlayerInput>().enabled = true;
        OptionCanvas.GetComponent<PlayerInput>().ActivateInput();
        OptionCanvas.GetComponent<OptionsMenuController>().ToPauseLevel(); 
    }

    public void restartButton()
    {
        GameManager.instance.OnRestart();
    }

    /**************************************************************************************************
    ANIMATION STATE METHODS //!All these are now controlled by new WorldSelectorAnimatedItem
    **************************************************************************************************/

    public void ToWorlds(){
        worldsUIGO.GetComponent<Animator>().Play("UI_fadeIn");
        levelsUIGO.GetComponent<Animator>().Play("UI_fadeOut");
    }

    public void ToLevels(){
        worldsUIGO.GetComponent<Animator>().Play("UI_fadeOut");
        levelsUIGO.GetComponent<Animator>().Play("UI_fadeIn");
    }

    public void EnableLevelsUI(){
        worldsUIGO.GetComponent<Animator>().Play("UI_inactive");
        levelsUIGO.GetComponent<Animator>().Play("UI_active");
    }

    public void EnableWorldsUI(){
        worldsUIGO.GetComponent<Animator>().Play("UI_active");
        levelsUIGO.GetComponent<Animator>().Play("UI_inactive");
    }

    public void DisableUI(){
        worldsUIGO.GetComponent<Animator>().Play("UI_inactive");
    }

    public void EnableUI(){
        worldsUIGO.GetComponent<Animator>().Play("UI_active");
    }

}