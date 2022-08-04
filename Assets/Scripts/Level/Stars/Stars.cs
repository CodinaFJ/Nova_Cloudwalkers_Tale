using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
StarCounter starCounter;
StarBehavior[] starBehaviorArray;

[HideInInspector]
public int collectedStars = 0;

    void Start()
    {
        //starCounter = FindObjectOfType<StarCounter>();
        starBehaviorArray = GetComponentsInChildren<StarBehavior>();

        //starCounter.InitializeCounter(starBehaviorArray.Length);
    }
}
