using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // There can be only one
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }

    // The Action Scheme
    private RunnerInputAction actionScheme;

    // Configuration
    [SerializeField] private float sqrSwipeDeadzone = 50.0f;

    #region private
    private bool tap;
    private Vector2 touchPosition;
    private Vector2 startDrag;
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;
    #endregion

    #region public proprities
    public bool Tap {  get { return tap; } }
    public Vector2 TouchPositon {  get { return touchPosition; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    #endregion

    
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SetupControl();
    }
    private void LateUpdate()
    {

        ResetInputs();
    }

    private void ResetInputs()
    {
        tap = swipeLeft = swipeRight = swipeDown = swipeUp = false;
    }

    private void SetupControl()
    {
        actionScheme = new RunnerInputAction();

        // Register different actions
        actionScheme.Gameplay.Tap.performed += ctx => OnTap(ctx);
        actionScheme.Gameplay.TouchPosition.performed += ctx => OnPosition(ctx);
        actionScheme.Gameplay.StartDrag.performed += ctx => OnStartDrag(ctx);
        actionScheme.Gameplay.EndDrag.performed += ctx => OnEndDrag(ctx);
    }

    private void OnEndDrag(InputAction.CallbackContext ctx)
    {
        Vector2 delta = touchPosition - startDrag;
        float sqrDistance = delta.sqrMagnitude;

        // Actual Swipe
        if (sqrDistance > sqrSwipeDeadzone)
        {
            float x = Math.Abs(delta.x);
            float y = Math.Abs(delta.y);

            if (x > y)// left or tight
            {
                if (delta.x > 0)
                    swipeRight = true;
                else
                    swipeLeft = true;
            }
            else // up or down
            {
                if (delta.y > 0)
                    swipeUp = true;
                else
                    swipeDown = true;
            }
        }

        startDrag = Vector2.zero;
    }
    private void OnStartDrag(InputAction.CallbackContext ctx)
    {
        startDrag = touchPosition;
    }
    private void OnPosition(InputAction.CallbackContext ctx)
    {
        touchPosition = ctx.ReadValue<Vector2>();
    }
    private void OnTap(InputAction.CallbackContext ctx)
    {
        tap = true;
    }

    public void OnEnable()
    {
        actionScheme.Enable();
    }

    public void OnDisable()
    {
        actionScheme.Disable();
    }
}
