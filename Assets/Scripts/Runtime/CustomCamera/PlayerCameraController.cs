using CustomCamera;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    private PlayerCameraManager _playerCameraManager;

    private void Awake()
    {
        _playerCameraManager = FindObjectOfType<PlayerCameraManager>();
        SetTarget();
    }
    
    public void PlayOrbitCamera()
    {
        _playerCameraManager.SetOrbitCameraActive();
    }

    public void PlayRollingCamera()
    {
        _playerCameraManager.SetRollingCameraActive();
    }

    public void PlayGlidingCamera()
    {
        _playerCameraManager.SetGlidingCameraActive();
    }
        
    public void PlayWreckingCamera()
    {
        _playerCameraManager.SetWreckingCameraActive();
    }

    public void SetTarget()
    {
        _playerCameraManager.ChangeTarget(transform);
    }
}
