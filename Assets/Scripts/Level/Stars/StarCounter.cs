using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarCounter : MonoBehaviour
{
    TextMeshProUGUI counter;

    int totalStarsInLevel;
    int collectedStars = 0;

    void Start()
    {
        counter = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void InitializeCounter(int total)
    {
        totalStarsInLevel = total;
        counter.text = "0/" + total;
    }

    public void IncreaseCounter()
    {
        collectedStars++;
        counter.text = collectedStars + "/" + totalStarsInLevel;
    }


}
