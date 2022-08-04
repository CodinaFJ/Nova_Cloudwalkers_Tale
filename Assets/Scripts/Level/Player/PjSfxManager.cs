using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PjSfxManager : MonoBehaviour
{
 /*********************************************************************
    PjSfxManager.cs

    Description:
        Manage sfx of the object "Player" based on the states
        defined on PjInputManager.cs

    Check also:
        PjInputManager.cs
        PlayerBehavior.cs

**********************************************************************/
    PjInputManager pjInputManager;
    PlayerBehavior playerBehavior;

    int stepNumber = 1;

    //SFX names
    const string STEP_CLOUD = "StepCloud";// + number of SFX variation (1-5)
    const string STEP_CRYSTAL = "StepCrystal";// + number of SFX variation (1-4)
    const string STEP_STONE = "StepStone";// + number of SFX variation (1-4)

    void Start()
    {
        playerBehavior = FindObjectOfType<PlayerBehavior>();      
    }

    public void Step()
    {
        int mechanicUnderPj;
        mechanicUnderPj = playerBehavior.GetMechanicUnderPj();

        if(mechanicUnderPj == 1)
        {
            if(stepNumber > 5) stepNumber = 1;
            AudioManager.instance.PlaySound(STEP_CLOUD + stepNumber);
        }
        else if(mechanicUnderPj >= 3 && mechanicUnderPj < 7)
        {
            if(stepNumber > 4) stepNumber = 1;
            AudioManager.instance.PlaySound(STEP_CRYSTAL + stepNumber);
        }
        else if(mechanicUnderPj == 999)
        {
            if(stepNumber > 4) stepNumber = 1;
            AudioManager.instance.PlaySound(STEP_STONE + stepNumber);
        }
    }

    
}
