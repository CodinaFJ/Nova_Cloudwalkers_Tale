using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalStarsCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI counterText;

    // Start is called before the first frame update
    void Start()
    {
        //counterText = GetComponentInChildren<TextMeshProUGUI>();
        counterText.text = GameProgressManager.instance.GetCollectedStarsInGame() + "/" + GameProgressManager.instance.GetTotalStarsInGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
