using UnityEngine;
using UnityEngine.Events;

namespace GeneralScriptableObjects.EventChannels
{
    [CreateAssetMenu(fileName = "Void Event Channel", menuName = "ScriptableObjects/Events/VoidEventChannel", order = 0)]
    public class VoidEventChannel : ScriptableObject
    {
        public UnityAction onEventRaised;

        public void RaiseEvent()
        {
            onEventRaised?.Invoke();
        }
    }
}