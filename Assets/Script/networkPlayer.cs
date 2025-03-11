using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;


[RequireComponent(typeof(playerMovement))]
[RequireComponent(typeof(playerInput))]
[RequireComponent(typeof(clientNetworkAnimator))]
[RequireComponent(typeof(clientNetworkTransform))]
[RequireComponent(typeof(playerAnimation))]
public class networkPlayer : NetworkBehaviour
{
    private playerMovement _playerMovement;
    private playerInput _inputHandler;
    private playerAnimation _playerAnimation;
    private void Awake()
    {
        _playerMovement = GetComponent<playerMovement>();
        _inputHandler = GetComponent<playerInput>();
        _playerAnimation = GetComponent<playerAnimation>();

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //transform.position = Vector3.zero;
        if (!IsOwner)
        {
            // Disable input for non-owners
            _playerMovement.enabled = false;
            _inputHandler.enabled = false;
            _playerAnimation.enabled = false;
        }
    }
}
