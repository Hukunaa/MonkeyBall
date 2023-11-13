using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTutorialEvents : MonoBehaviour
{
    [SerializeField] 
    private VoidEventChannel _onRunButtonTappedEventChannel;
    
    [FormerlySerializedAs("_joystickUsedEventChannel")] [SerializeField] 
    private VoidEventChannel _holdJoystickEventChannel;

    [SerializeField] 
    private VoidEventChannel _releaseJoystickEventChannel;
    
    [SerializeField] 
    private VoidEventChannel _platformLeftEventChannel;
    
    [SerializeField] 
    private VoidEventChannel _transformButtonClickedEventChannel;

    public void OnRunButtonTapped()
    {
        _onRunButtonTappedEventChannel.RaiseEvent();
    }

    public void OnHoldJoystick()
    {
        _holdJoystickEventChannel.RaiseEvent();
    }

    public void OnReleaseJoystick()
    {
        _releaseJoystickEventChannel.RaiseEvent();
    }

    public void OnPlatformLeft()
    {
        _platformLeftEventChannel.RaiseEvent();
    }

    public void OnTransformButtonClicked()
    {
        _transformButtonClickedEventChannel.RaiseEvent();
    }
}
