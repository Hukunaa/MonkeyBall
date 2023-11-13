using UnityEngine;
using UnityEngine.Events;

namespace SceneManagementSystem.Scripts.ScriptableObjects.EventChannels
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Events/Location Changed Event Channel")]
    public class LocationChangeEventChannel : ScriptableObject
    {
        public UnityAction<LocationSO> OnLoadingRequested;

        public void RaiseEvent(LocationSO locationToLoad)
        {
            OnLoadingRequested?.Invoke(locationToLoad);
        }
    }
}