using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Set correctly the states of world button and its lock status.
/// !Should be implemented in world button game object, not in lock.
/// </summary>
public class LockScript : MonoBehaviour
{
    [SerializeField] int starsToOpen;
    //[SerializeField] levelButton levelToUnlock;
    [SerializeField] int worldToUnlock;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] GameObject lockGO;
    [SerializeField] GameObject counterGO;
    bool locked = true;

    void Start()
    {
        StartCoroutine(SetLockState());
    }

    /*private void Update() {
        if (GameProgressManager.instance.CheckUnlockedWorld(worldToUnlock))
        {
            Debug.Log("World is unlocked");
            GetComponentInParent<Button>().interactable = true;
            //gameObject.SetActive(false);
            return ;
        }
        else
        {
            GetComponentInParent<Button>().interactable = false;
        }
    }*/

    /*private void SetWorldInitialState()
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
    }*/

    IEnumerator SetLockState()
    {
        //TODO: Change to implementation in world button, not lock
        string starCollectedNumberText;

        starCollectedNumberText = GameProgressManager.instance.GetCollectedStarsInGame().ToString();
        countText.text = starCollectedNumberText + "/" + starsToOpen;
        yield return new WaitForSeconds(0.5f);
        if (GameProgressManager.instance.GetCollectedStarsInGame() >= starsToOpen && !GameProgressManager.instance.GetUnlockedWorld(worldToUnlock))
        {
            MapContextController.Instance.GetMapState().UnlockWorldAction(worldToUnlock);
            //levelToUnlock.UnlockLevel();
        }
        UpdateLockState();
    }

    public void UpdateLockState()
    {
        if (GameProgressManager.instance.GetUnlockedWorld(worldToUnlock))
        {
            lockGO.SetActive(false);
            counterGO.SetActive(false);
        }
        else
        {
            lockGO.SetActive(true);
            counterGO.SetActive(true);
        }
    }
}