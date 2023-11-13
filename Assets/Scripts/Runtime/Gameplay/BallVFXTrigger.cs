using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallVFXTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _ballBurstFX;

    [SerializeField]
    private UnityEvent _ballTrailOnFX;

    [SerializeField]
    private UnityEvent _ballTrailOffFX;

    [SerializeField]
    private UnityEvent _onStartBallVFXTriggerer;

    private void Start()
    {
        _onStartBallVFXTriggerer?.Invoke();
    }
    public void TriggerEffects()
    {
        _ballBurstFX?.Invoke();
        _ballTrailOnFX?.Invoke();
        _ballTrailOffFX?.Invoke();
    }

    public UnityEvent BallBurstFX { get => _ballBurstFX; }
    public UnityEvent BallTrailOnFX { get => _ballTrailOnFX; }
    public UnityEvent BallTrailOffFX { get => _ballTrailOffFX; }

}
