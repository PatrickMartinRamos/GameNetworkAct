using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class playerInput : MonoBehaviour
{
    private InputSystem_Actions _playerInputActions;
    public Vector2 runInput { get; private set; }
    public bool isWalking { get; private set; }
    public bool isSprinting { get; private set; }
    public bool Jump { get; private set; }
    public bool Crouch {  get; private set; } // also used in crouch slide

    private void Awake()
    {
        _playerInputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Enable();

        // Subscribe to events properly
        _playerInputActions.Player.Walk.performed += OnWalkPerformed;
        _playerInputActions.Player.Walk.canceled += OnWalkCanceled;

        _playerInputActions.Player.Run.performed += OnRunPerformed;
        _playerInputActions.Player.Run.canceled += OnRunCanceled;

        _playerInputActions.Player.Sprint.performed += OnSprintPerformed;
        _playerInputActions.Player.Sprint.canceled += OnSprintCanceled;

        _playerInputActions.Player.Jump.started += onJumpPeformed;
        _playerInputActions.Player.Jump.canceled += onJumpCanceled;

        _playerInputActions.Player.Crouch.performed += onCrouchPerformed;
        _playerInputActions.Player.Crouch.canceled += onCrouchCanceled;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        _playerInputActions.Player.Walk.performed -= OnWalkPerformed;
        _playerInputActions.Player.Walk.canceled -= OnWalkCanceled;

        _playerInputActions.Player.Run.performed -= OnRunPerformed;
        _playerInputActions.Player.Run.canceled -= OnRunCanceled;

        _playerInputActions.Player.Sprint.performed -= OnSprintPerformed;
        _playerInputActions.Player.Sprint.canceled -= OnSprintCanceled;

        _playerInputActions.Player.Jump.started -= onJumpPeformed;
        _playerInputActions.Player.Jump.canceled -= onJumpCanceled;

        _playerInputActions.Player.Crouch.performed -= onCrouchPerformed;
        _playerInputActions.Player.Crouch.canceled -= onCrouchCanceled;

        _playerInputActions.Player.Disable();
    }

    // Event Handlers
    private void OnWalkPerformed(InputAction.CallbackContext ctx) => isWalking = true;
    private void OnWalkCanceled(InputAction.CallbackContext ctx) => isWalking = false;

    private void OnRunPerformed(InputAction.CallbackContext ctx) => runInput = ctx.ReadValue<Vector2>();
    private void OnRunCanceled(InputAction.CallbackContext ctx) => runInput = Vector2.zero;

    private void OnSprintPerformed(InputAction.CallbackContext ctx) => isSprinting = true;
    private void OnSprintCanceled(InputAction.CallbackContext ctx) => isSprinting = false;

    private void onCrouchPerformed(InputAction.CallbackContext ctx) => Crouch = true;
    private void onCrouchCanceled(InputAction.CallbackContext ctx) => Crouch = false;

    private void onJumpPeformed(InputAction.CallbackContext ctx)
    {
        if (!Jump)
        {
            Jump = true;
            StartCoroutine(ResetJump());
        }
    }
    private void onJumpCanceled(InputAction.CallbackContext ctx) => Jump = false;

    //temp fix until i figure out how to reset the jump input using the new input system ><
    private IEnumerator ResetJump()
    {
        yield return new WaitForEndOfFrame();
        Jump = false;
    }
}
