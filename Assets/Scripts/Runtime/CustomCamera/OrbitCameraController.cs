using Cinemachine;
using UnityEngine;

public class OrbitCameraController : MonoBehaviour
{
    [SerializeField] 
    private float _orbitalSpeed = 50;

    [SerializeField] 
    private CinemachineVirtualCamera _cmCamera;
    
    private CinemachineOrbitalTransposer _orbitalTransposer;

    private void Awake()
    {
        _orbitalTransposer = _cmCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Update()
    {
        _orbitalTransposer.m_XAxis.Value += _orbitalSpeed * Time.deltaTime;
    }
}
