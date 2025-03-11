using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(playerMovement))]
public class thirdPersonCamera : NetworkBehaviour
{
    [SerializeField] private Transform _playerRootCameraTransform;

    private void Start()
    {
        if (IsOwner)
        {
            cameraManager cameraManager = FindAnyObjectByType<cameraManager>();
            if (cameraManager != null)
            {
                cameraManager.AssignCamera(_playerRootCameraTransform);
            }
        }
    }
}
