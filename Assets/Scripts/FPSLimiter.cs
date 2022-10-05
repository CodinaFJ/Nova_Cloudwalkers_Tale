using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    public static FPSLimiter instance;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
