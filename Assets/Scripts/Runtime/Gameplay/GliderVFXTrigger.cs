using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GliderVFXTrigger : MonoBehaviour
{
    //Events here
    [SerializeField]
    private UnityEvent _wingsBurstFX;
    [SerializeField]
    private UnityEvent _wingsTrailOnFX;
    [SerializeField]
    private UnityEvent _wingsTrailOffFX;

    [SerializeField]
    private UnityEvent _onStartGliderVFXTriggerer;

    private void Start()
    {
        _onStartGliderVFXTriggerer?.Invoke();
    }
    public void TriggerEffects()
    {
        _wingsBurstFX?.Invoke();
        _wingsTrailOnFX?.Invoke();
        _wingsTrailOffFX?.Invoke();
    }

    public UnityEvent WingsBurstFX { get => _wingsBurstFX; }
    public UnityEvent WingsTrailOnFX { get => _wingsTrailOnFX; }
    public UnityEvent WingsTrailOffFX { get => _wingsTrailOffFX; }

}
