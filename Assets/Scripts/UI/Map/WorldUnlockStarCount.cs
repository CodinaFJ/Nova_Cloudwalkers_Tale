using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Set correctly the states of world button and its lock status.
/// !Should be implemented in world button game object, not in lock.
/// </summary>
public class WorldUnlockStarCount : MonoBehaviour
{
    [SerializeField] int starsToOpen;
    [SerializeField] levelButton levelToUnlock;
    [SerializeField] int worldToUnlock;
    [SerializeField] TextMeshProUGUI countText;
    bool locked = true;

    void Start()
    {
        SetWorldInitialState();
        SetLockState();        
    }

    private void Update() {
        /*if (GameProgressManager.instance.CheckUnlockedWorld(worldToUnlock))
        {
            Debug.Log("World is unlocked");
            GetComponentInParent<Button>().interactable = true;
            //gameObject.SetActive(false);
            return ;
        }
        else
        {
            GetComponentInParent<Button>().interactable = false;
        }*/
    }

    private void SetWorldInitialState()
    {
        //TODO: Change to implementation in world button, not lock
        if (GameProgressManager.instance.CheckUnlockedWorld(worldToUnlock))
        {
            Debug.Log("World is unlocked");
            GetComponentInParent<Button>().interactable = true;
            gameObject.SetActive(false);
            return ;
        }
        else
        {
            GetComponentInParent<Button>().interactable = false;
        }
    }

    private void SetLockState()
    {
        //TODO: Change to implementation in world button, not lock
        string starCollectedNumberText;

        starCollectedNumberText = GameProgressManager.instance.GetCollectedStarsInGame().ToString();
        countText.text = starCollectedNumberText + "/" + starsToOpen;
        if (GameProgressManager.instance.GetCollectedStarsInGame() >= starsToOpen)
        {
            GetComponentInParent<Button>().interactable = true;
            GameProgressManager.instance.UnlockWorld(worldToUnlock);
            levelToUnlock.UnlockLevel();
        }
    }
    
}