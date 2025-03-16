using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.Netcode;

public class playerInput : NetworkBehaviour
{
    private InputSystem_Actions _playerInputActions;
    public Vector2 runInput { get; private set; }
    public bool isWalking { get; private set; }
    public bool isSprinting { get; private set; }
    public bool Jump { get; private set; }
    public bool Crouch { get; private set; } 
    public bool runningSlide { get; private set; }
    private bool canJump = true; 
    private float jumpCooldownTime = 1f; // Cooldown duration for jump

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

        _playerInputActions.Player.Slide.performed += onSlidePerformed;
        _playerInputActions.Player.Slide.canceled += onSlideCanceled;
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

        _playerInputActions.Player.Slide.performed -= onSlidePerformed;
        _playerInputActions.Player.Slide.canceled -= onSlideCanceled;

        _playerInputActions.Player.Disable();
    }

    // Event Handlers
    private void OnWalkPerformed(InputAction.CallbackContext ctx) => isWalking = true;
    private void OnWalkCanceled(InputAction.CallbackContext ctx) => isWalking = false;

    private void OnRunPerformed(InputAction.CallbackContext ctx) => runInput = ctx.ReadValue<Vector2>();
    private void OnRunCanceled(InputAction.CallbackContext ctx) => runInput = Vector2.zero;

    private void onSlidePerformed(InputAction.CallbackContext ctx)
    {
        if(!isSprinting) return;
        runningSlide = true;
        if(runningSlide)
        {
            StartCoroutine(DisableJumpForSlide());
            StartCoroutine(resetRunningSlide());
        }
    }
    private IEnumerator resetRunningSlide()
    {
        yield return new WaitForEndOfFrame();
        runningSlide = false;   
    }
    private void onSlideCanceled(InputAction.CallbackContext ctx)
    {
        runningSlide = false;
    }

    private IEnumerator DisableJumpForSlide()
    {
        canJump = false; 
        yield return new WaitForSeconds(1.5f); 
        canJump = true; 
    }

    private void OnSprintPerformed(InputAction.CallbackContext ctx) => isSprinting = true;
    private void OnSprintCanceled(InputAction.CallbackContext ctx) =>  isSprinting = false;
    private void onCrouchPerformed(InputAction.CallbackContext ctx)
    {
        if(isSprinting) return;
        Crouch = true;
    }
    private void onCrouchCanceled(InputAction.CallbackContext ctx) => Crouch = false;

    private void onJumpPeformed(InputAction.CallbackContext ctx)
    {
        if (canJump && !Jump && !Crouch && !runningSlide)
        {
            Jump = true;
            StartCoroutine(ResetJump());
            StartCoroutine(JumpCooldown());
        }
    }
    private void onJumpCanceled(InputAction.CallbackContext ctx) => Jump = false;
    private IEnumerator ResetJump()
    {
        yield return new WaitForEndOfFrame();
        Jump = false;
    }
    private IEnumerator JumpCooldown()
    {
        canJump = false; // Disable jumping
        yield return new WaitForSeconds(jumpCooldownTime); // Wait for cooldown duration
        canJump = true; // Re-enable jumping
    }

    private void Update()
    {
        if (IsOwner)
        {
            SendInputToServerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void SendInputToServerRpc()
    {
        // Send input data to the server
        SendInputServerRpc(runInput, isWalking, isSprinting, Jump, Crouch, runningSlide);
    }

    [Rpc(SendTo.Server)]
    private void SendInputServerRpc(Vector2 runInput, bool isWalking, bool isSprinting, bool jump, bool crouch, bool runningSlide)
    {
        this.runInput = runInput;
        this.isWalking = isWalking;
        this.isSprinting = isSprinting;
        this.Jump = jump;
        this.Crouch = crouch;
        this.runningSlide = runningSlide;
    }
}
