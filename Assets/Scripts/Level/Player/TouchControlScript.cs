using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchControlScript : MonoBehaviour
{
    public static TouchControlScript instance;

    private PlayerControls playerControls;
    private PlayerBehavior playerBehavior;
    private PjInputManager pjInputManager;

    private Vector3 fingerScreenPosition;

    private void Awake() {
        if(instance == null)
           instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        playerControls =  new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += FingerUp;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += FingerMove;
    }

    private void OnDisable() {
        playerControls.Disable();
        TouchSimulation.Disable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= FingerUp;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= FingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void Start(){ 
        /*playerControls.TouchControls.TouchPress.started += ctx => StartTouch(ctx);
        playerControls.TouchControls.TouchPress.canceled += ctx => EndTouch(ctx);*/
        playerBehavior = PlayerBehavior.instance;
        pjInputManager = PjInputManager.instance;

        fingerScreenPosition = new Vector3(0,0,0);
    }

    /*private void StartTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch started " + playerControls.TouchControls.TouchPosition.ReadValue<Vector2>());
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(playerControls.TouchControls.TouchPosition.ReadValue<Vector2>());
        touchPosition.z = 0f;
        playerBehavior.clickIsForCloud = false;
        pjInputManager.onClickMouseWorldPos = touchPosition; 

        //Debug.Log("On Toch Position: " + touchPosition);
        CloudInputManager.instance.SelectCloud(touchPosition);
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(playerControls.TouchControls.TouchPosition.ReadValue<Vector2>());
        touchPosition.z = 0f;

        //if(Vector3.Magnitude((Vector3)playerControls.TouchControls.TouchPosition.ReadValue<Vector2>() - onClickMouseWorldPos) > releaseMouseTolerance && !wallLevel) return;

        if(!playerBehavior.clickIsForCloud)
        pjInputManager.FindPath(touchPosition);
    }*/

    private void FingerDown(Finger finger)
    {
        Debug.Log("Enhanced touch control - Touch started " + finger.screenPosition);
        fingerScreenPosition = finger.screenPosition;
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(finger.screenPosition);
        touchPosition.z = 0f;
        playerBehavior.clickIsForCloud = false;
        pjInputManager.onClickMouseWorldPos = touchPosition; 

        //Debug.Log("On Toch Position: " + touchPosition);
        CloudInputManager.instance.SelectCloud(touchPosition);
    }

    private void FingerUp(Finger finger)
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(finger.screenPosition);
        touchPosition.z = 0f;

        //if(Vector3.Magnitude((Vector3)playerControls.TouchControls.TouchPosition.ReadValue<Vector2>() - onClickMouseWorldPos) > releaseMouseTolerance && !wallLevel) return;

        if(!playerBehavior.clickIsForCloud)
        pjInputManager.FindPath(touchPosition);
    }

    private void FingerMove(Finger finger)
    {
        fingerScreenPosition = finger.screenPosition;
    }

    public Vector3 GetTouchPosition()
    {
        /*Finger finger;
        if(UnityEngine.InputSystem.EnhancedTouch.Touch.fingers[0] != null) finger = UnityEngine.InputSystem.EnhancedTouch.Touch.fingers[0];
        else return new Vector3(0,0,0);  */

        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(fingerScreenPosition);
        //Vector3 touchPosition = Camera.main.ScreenToWorldPoint(playerControls.TouchControls.TouchPosition.ReadValue<Vector2>());
        touchPosition.z = 0f;
        return touchPosition;
    }

}
