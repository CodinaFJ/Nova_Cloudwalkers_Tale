using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchControlScript : MonoBehaviour
{
    public static TouchControlScript instance;

    private PlayerControls playerControls;
    private PlayerBehavior playerBehavior;
    private PjInputManager pjInputManager;

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
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Start(){ 
        playerControls.TouchControls.TouchPress.started += ctx => StartTouch(ctx);
        playerControls.TouchControls.TouchPress.canceled += ctx => EndTouch(ctx);
        playerBehavior = PlayerBehavior.instance;
        pjInputManager = PjInputManager.instance;
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch started " + playerControls.TouchControls.TouchPosition.ReadValue<Vector2>());
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(playerControls.TouchControls.TouchPosition.ReadValue<Vector2>());
        touchPosition.z = 0f;
        playerBehavior.clickIsForCloud = false;
        pjInputManager.onClickMouseWorldPos = touchPosition; 

        Debug.Log("On Toch Position: " + touchPosition);
        CloudInputManager.instance.SelectCloud(touchPosition);
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(playerControls.TouchControls.TouchPosition.ReadValue<Vector2>());
        touchPosition.z = 0f;

        //if(Vector3.Magnitude((Vector3)playerControls.TouchControls.TouchPosition.ReadValue<Vector2>() - onClickMouseWorldPos) > releaseMouseTolerance && !wallLevel) return;

        Debug.Log("Touch ended ");
        if(!playerBehavior.clickIsForCloud)
        pjInputManager.FindPath(touchPosition);
    }

    public Vector3 GetTouchPosition()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(playerControls.TouchControls.TouchPosition.ReadValue<Vector2>());
        touchPosition.z = 0f;
        return touchPosition;
    }

}
