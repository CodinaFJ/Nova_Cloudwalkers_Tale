using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDisablemouse : MonoBehaviour
{
    [SerializeField] GameObject clickAnim;
    [SerializeField] GameObject pointerAnim;
    [SerializeField] GameObject mouse;

    public void DisableMouse()
    {
        clickAnim.SetActive(false);
        Destroy(clickAnim);
        pointerAnim.SetActive(false);
        Destroy(pointerAnim);
        mouse.SetActive(false);
        Destroy(mouse);
    }
}
