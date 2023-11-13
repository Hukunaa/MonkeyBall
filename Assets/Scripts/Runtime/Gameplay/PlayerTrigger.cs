using GeneralScriptableObjects.EventChannels;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] 
    private string _playerTag;

    [SerializeField] 
    private VoidEventChannel _onPlayerEnteredTriggerVoidEventChannel;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            _onPlayerEnteredTriggerVoidEventChannel.RaiseEvent();
        }
    }
}
