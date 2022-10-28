using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalStarsCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI counterText;

    private void Start(){
        counterText.text = GameProgressManager.instance.GetCollectedStarsInGame() + "/" + GameProgressManager.instance.GetTotalStarsInGame();
    }
    public void UpdateCounter(){
        counterText.text = GameProgressManager.instance.GetCollectedStarsInGame() + "/" + GameProgressManager.instance.GetTotalStarsInGame();
    }
}
