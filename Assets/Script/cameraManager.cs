using Unity.Cinemachine;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    private CinemachineCamera _freeLookCamera;
    private static cameraManager _instance;

    private void Awake()
    {
        // Ensure there's only one instance of CameraManager
        if (_instance == null)
        {
            _instance = this;
            _freeLookCamera = FindAnyObjectByType<CinemachineCamera>();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            
        }
    }

    public void AssignCamera(Transform playerCameraRootTransform)
    {
        if (_freeLookCamera != null)
        {
            _freeLookCamera.Follow = playerCameraRootTransform;
        }
    }
}
