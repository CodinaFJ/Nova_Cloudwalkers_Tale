using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorUnlockerW1 : MonoBehaviour
{
    Button[] buttons;
    GameState gameState;

    Animator buttonAnimator;
    private string currentState;

    void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        EnableUnlockedLevels();
        
    }

    void EnableUnlockedLevels()
    {
        gameState = GameState.instance;
    
        for (int i = 0; i < buttons.Length; i++)
        {
            if(i == 0)
            {
                buttons[i].interactable = true;
            }
            else if(i == 4)
            {
                if(!gameState.preCompletedLevelsWorld1[i - 2] && 
                    gameState.completedLevelsWorld1[i - 2]) 
                {
                    StartCoroutine(DelayAnimationStart("UI_LevelUnlock", i));
                }
                    
                else buttons[i].interactable = gameState.completedLevelsWorld1[i - 2];
            }
            else if(i == 5 || i == 6)
            {
                if(!(gameState.preCompletedLevelsWorld1[3] || gameState.preCompletedLevelsWorld1[4]) && 
                    (gameState.completedLevelsWorld1[3] || gameState.completedLevelsWorld1[4])) 
                {
                    StartCoroutine(DelayAnimationStart("UI_LevelUnlock", i));
                }
                
                else buttons[i].interactable = (gameState.completedLevelsWorld1[3] || gameState.completedLevelsWorld1[4]);
            }
            else if(i == 7)
            {
                if(!(gameState.preCompletedLevelsWorld1[i - 1] || gameState.preCompletedLevelsWorld1[i - 2]) && 
                    (gameState.completedLevelsWorld1[i - 1] || gameState.completedLevelsWorld1[i - 2])) 
                {
                    StartCoroutine(DelayAnimationStart("UI_LevelUnlock", i));
                }
                
                else buttons[i].interactable = (gameState.completedLevelsWorld1[i - 1] || gameState.completedLevelsWorld1[i - 2]);
            }
            else
            {
                if(!gameState.preCompletedLevelsWorld1[i - 1] && 
                    gameState.completedLevelsWorld1[i - 1])
                {
                    StartCoroutine(DelayAnimationStart("UI_LevelUnlock", i));
                }
                
                else buttons[i].interactable = gameState.completedLevelsWorld1[i - 1];
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
        Debug.Log("Doing unlock animation for level: " + buttonNumber);

        //play the animation
        buttons[buttonNumber].GetComponent<Animator>().enabled = true;
        buttons[buttonNumber].GetComponent<Animator>().Play(newState);
    }
}
