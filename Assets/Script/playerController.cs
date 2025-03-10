using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class playerController : MonoBehaviour
{
    private CharacterController _characterController;
    private InputSystem_Actions _playerInputAction;
    private Vector2 _moveInput;

    [SerializeField] private float moveSpeed;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInputAction = new InputSystem_Actions();
    }
    #region enable/disable Inputs
    private void OnEnable()
    {
        _playerInputAction.Player.Move.Enable();
        _playerInputAction.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _playerInputAction.Player.Move.canceled += ctx => _moveInput = Vector2.zero;
    }
    private void OnDisable()
    {
        _playerInputAction.Player.Move.Disable();
    }
    #endregion

    private void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(_moveInput.x, 0, _moveInput.y);
        moveDir = transform.TransformDirection(moveDir);
        _characterController.Move(moveDir * moveSpeed * Time.deltaTime);
    }
}
