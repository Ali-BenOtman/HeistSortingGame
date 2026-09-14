// SwipeDetector.cs
// Detects swipe direction on mobile and mouse (for testing in editor)

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class SwipeDetector : MonoBehaviour
{
    [Header("Swipe Settings")]
    public float minSwipeDistance = 50f;

    private Vector2 touchStartPos;
    private bool isSwiping = false;

    public UnityEvent<SwipeDirection> onSwipe;

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
        HandleMouseInput();
        HandleTouchInput();
    }

    void HandleMouseInput()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            touchStartPos = mouse.position.ReadValue();
            isSwiping = true;
        }

        if (mouse.leftButton.wasReleasedThisFrame && isSwiping)
        {
            Vector2 endPos = mouse.position.ReadValue();
            ProcessSwipe(touchStartPos, endPos);
            isSwiping = false;
        }
    }

    void HandleTouchInput()
    {
        if (Touch.activeTouches.Count > 0)
        {
            Touch touch = Touch.activeTouches[0];

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                touchStartPos = touch.screenPosition;
                isSwiping = true;
            }

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended && isSwiping)
            {
                ProcessSwipe(touchStartPos, touch.screenPosition);
                isSwiping = false;
            }
        }
    }

    void ProcessSwipe(Vector2 start, Vector2 end)
    {
        Vector2 delta = end - start;

        if (delta.magnitude < minSwipeDistance)
            return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            onSwipe.Invoke(delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left);
        else
            onSwipe.Invoke(delta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down);
    }
}