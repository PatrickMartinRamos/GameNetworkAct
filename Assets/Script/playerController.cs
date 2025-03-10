using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class playerController : NetworkBehaviour
{
    //character controller
    private CharacterController _characterController;
    private InputSystem_Actions _playerInputAction;
    private Vector2 _moveInput;

    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInputAction = new InputSystem_Actions();


        _playerInputAction.Disable();
        this.enabled = false;
    }
    #region enable/disable Inputs   
    void enableInput()
    {
        if (IsOwner)
        {
            _playerInputAction.Player.Enable();

            _playerInputAction.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _playerInputAction.Player.Move.canceled += ctx => _moveInput = Vector2.zero;
        }
    }
    private void OnDisable()
    {
        _playerInputAction.Player.Move.Disable();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(IsOwner)
        {
            enableInput();
            this.enabled = true;
        }
        if(IsServer)
        {
            enableInput();
            this.enabled = true;
        }
    }

    #endregion

    private void Update()
    {
       // updateToRpc();
    }

    #region move 
    void Move()
    {
        Vector3 moveDir = new Vector3(_moveInput.x, 0, _moveInput.y);
        moveDir = transform.TransformDirection(moveDir);
        _characterController.Move(moveDir * moveSpeed * Time.deltaTime);
    }
    void rotateToCameraDir()
    {
        Vector3 cameraTransformForward = Camera.main.transform.forward;

        cameraTransformForward.y = 0;

        transform.rotation = Quaternion.LookRotation(cameraTransformForward);
    }
    #endregion

    [Rpc(SendTo.Server)]
    void updateToRpc()
    {
        Move();
        rotateToCameraDir();
    }
    private void LateUpdate()
    {
        if(!IsOwner)
        {
            return;
        }
        updateToRpc();
    }
}
