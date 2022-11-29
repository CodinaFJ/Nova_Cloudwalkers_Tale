using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchControlScript : MonoBehaviour
{
    public static TouchControlScript instance;

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
    }

    private void OnEnable() {
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += FingerUp;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += FingerMove;
    }

    private void OnDisable() {
        TouchSimulation.Disable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= FingerUp;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= FingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void Start(){ 
        playerBehavior = PlayerBehavior.instance;
        pjInputManager = PjInputManager.instance;

        fingerScreenPosition = new Vector3(0,0,0);
    }

    private void FingerDown(Finger finger)
    {
        Debug.Log("FingerDown");
        fingerScreenPosition = finger.screenPosition;
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(finger.screenPosition);
        touchPosition.z = 0f;
        pjInputManager.OnClickMouseWorldPos = touchPosition; 
        CloudInputManager.instance.SelectCloud(touchPosition);
        
        playerBehavior.clickIsForCloud = false;        
    }

    private void FingerUp(Finger finger)
    {
        Debug.Log("FingerUp");
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(finger.screenPosition);
        touchPosition.z = 0f;

        CloudInputManager.instance.OnReleaseLeftClick();

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
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(fingerScreenPosition);
        touchPosition.z = 0f;
        return touchPosition;
    }

}
