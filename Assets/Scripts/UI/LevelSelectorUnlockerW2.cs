using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorUnlockerW2 : MonoBehaviour
{
    Button[] buttons;
    GameState gameState;

    Animator buttonAnimator;
    private string currentState;

    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        GameState gameState = FindObjectOfType<GameState>();

        EnableUnlockedLevels();
        
    }


    void Update()
    {
        EnableUnlockedLevels();
        
    }

    void EnableUnlockedLevels()
    {
        gameState = GameState.instance;
    
        for (int i = 0; i < buttons.Length; i++)
        {
            if(i == 0)
            {
                if(!gameState.completedLevelsWorld2[i] && 
                    gameState.totalCollectedStars >= 14) 
                {
                    StartCoroutine(DelayAnimationStart("UI_LevelUnlock", i));
                }
                
                else buttons[i].interactable = gameState.totalCollectedStars >= 14;
            }
            else if(i == 3)
            {
                if(!gameState.preCompletedLevelsWorld2[i - 2] && 
                    gameState.completedLevelsWorld2[i - 2]) 
                {
                    StartCoroutine(DelayAnimationStart("UI_LevelUnlock", i));
                }
                
                else buttons[i].interactable = gameState.completedLevelsWorld2[i - 2];
            }
            else if(i == 5)
            {
                if(!gameState.preCompletedLevelsWorld2[i - 2] && 
                    gameState.completedLevelsWorld2[i - 2]) 
                {
                    StartCoroutine(DelayAnimationStart("UI_LevelUnlock", i));
                }
                
                else buttons[i].interactable = gameState.completedLevelsWorld2[i - 2];
            }
            else
            {
                if(!gameState.preCompletedLevelsWorld2[i - 1] && 
                    gameState.completedLevelsWorld2[i - 1]) 
                {
                    StartCoroutine(DelayAnimationStart("UI_LevelUnlock", i));
                }
                
                else buttons[i].interactable = gameState.completedLevelsWorld2[i - 1];
            }
        }
    }

    IEnumerator DelayAnimationStart(string newState, int buttonNumber)
    {
        yield return new WaitForSeconds(1.5f);
        ChangeAnimationState(newState, buttonNumber);
        buttons[buttonNumber].interactable = true;
        buttons[buttonNumber].GetComponentInChildren<ParticleSystem>().Play();
    }

    public void ChangeAnimationState(string newState, int buttonNumber)
    {
        Debug.Log("Doing unlock animation");

        //play the animation
        buttons[buttonNumber].GetComponent<Animator>().enabled = true;
        buttons[buttonNumber].GetComponent<Animator>().Play(newState);
    }
}
