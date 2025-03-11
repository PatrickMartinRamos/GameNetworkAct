using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class playerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private playerInput _inputHandler;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float sprintSpeed = 10f;
    private Vector3 moveDirection;
    private float rotateSpeed = 5f;
    private Transform _cameraTransform;

    private Vector3 _velocity;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 2.5f;
    private bool canJump = true;  
    [SerializeField] private float jumpCooldown = 0.5f; 

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDistance = 0.2f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputHandler = GetComponent<playerInput>();  
        _cameraTransform = Camera.main.transform; 
    }

    private void Update()
    {   
        MovePlayer();
        ApplyGravity();
        Jump();    
        Debug.Log(_inputHandler.Jump);
    }

    private void MovePlayer()
    {   
        if (_cameraTransform == null) return;

        // Get the forward and right direction of the camera
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        moveDirection = (cameraForward * _inputHandler.runInput.y) + (cameraRight * _inputHandler.runInput.x);

        // Determine the speed based on input
        float currentSpeed = runSpeed;
        if (_inputHandler.isSprinting)
        {
            currentSpeed = sprintSpeed;
        }
        else if (_inputHandler.isWalking)
        {
            currentSpeed = walkSpeed;
        }

        _characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Smoothly rotate the player in the direction of movement
        if (_inputHandler.runInput.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundLayer);
    }

    void ApplyGravity()
    {
        if (IsGrounded() && _velocity.y < 0f)
        {
            _velocity.y = -2f; // Keeps player grounded
        }
        else
        {
            _velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        }

        _characterController.Move(_velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (IsGrounded() && _inputHandler.Jump) 
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
        
}
