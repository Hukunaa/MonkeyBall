using ScriptableObjects.DataContainer;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.EventChannels
{
    [CreateAssetMenu(fileName = "SkinDataEventChannel", menuName = "ScriptableObjects/Events/SkinDataEventChannel", order = 0)]
    public class SkinDataEventChannel : ScriptableObject
    {
        public UnityAction<SkinData> onEventRaised;

        public void RaiseEvent(SkinData val)
        {
            onEventRaised?.Invoke(val);
        }
    }
}