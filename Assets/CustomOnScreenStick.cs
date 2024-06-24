using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class CustomOnScreenStick : MonoBehaviour
{
    public RectTransform handle; // Assign this in the Inspector
    private Vector2 _startPosition;
    private int _activeTouchId = -1;

    void Start()
    {
        _startPosition = handle.anchoredPosition; // Save the initial position of the handle
    }

    public void OnPointerDown(InputAction.CallbackContext context)
    {
        if (_activeTouchId == -1)
        {
            _activeTouchId = context.control.device.deviceId; // Get the device ID for the current touch
            Vector2 position = context.ReadValue<Vector2>();
            handle.anchoredPosition = position; // Move the handle to the touch position
        }
    }

    public void OnPointerMove(InputAction.CallbackContext context)
    {
        if (_activeTouchId == context.control.device.deviceId)
        {
            Vector2 position = context.ReadValue<Vector2>();
            handle.anchoredPosition = position; // Move the handle as the touch moves
        }
    }

    public void OnPointerUp(InputAction.CallbackContext context)
    {
        if (_activeTouchId == context.control.device.deviceId)
        {
            _activeTouchId = -1; // Reset the active touch ID
            handle.anchoredPosition = _startPosition; // Return the handle to its initial position
        }
    }
}
