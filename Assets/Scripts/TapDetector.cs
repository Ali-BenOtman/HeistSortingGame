// TapDetector.cs
// Detects taps on the current bill or obstacle

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TapDetector : MonoBehaviour
{
    [Header("Settings")]
    public float maxTapDuration = 0.2f;
    public float maxTapMovement = 20f;

    private float touchStartTime;
    private Vector2 touchStartPos;

    public UnityEvent onTap;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        HandleMouseTap();
        HandleTouchTap();
    }

    void HandleMouseTap()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            touchStartTime = Time.time;
            touchStartPos = mouse.position.ReadValue();
        }

        if (mouse.leftButton.wasReleasedThisFrame)
        {
            float duration = Time.time - touchStartTime;
            float movement = Vector2.Distance(mouse.position.ReadValue(), touchStartPos);

            if (duration <= maxTapDuration && movement <= maxTapMovement)
                onTap.Invoke();
        }
    }

    void HandleTouchTap()
    {
        if (Touch.activeTouches.Count > 0)
        {
            Touch touch = Touch.activeTouches[0];

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                touchStartTime = Time.time;
                touchStartPos = touch.screenPosition;
            }

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                float duration = Time.time - touchStartTime;
                float movement = Vector2.Distance(touch.screenPosition, touchStartPos);

                if (duration <= maxTapDuration && movement <= maxTapMovement)
                    onTap.Invoke();
            }
        }
    }
}