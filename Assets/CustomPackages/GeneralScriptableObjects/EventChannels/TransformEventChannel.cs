using UnityEngine;
using UnityEngine.Events;

namespace GeneralScriptableObjects.EventChannels
{
    [CreateAssetMenu(fileName = "Transform event channel", menuName = "ScriptableObjects/Events/TransformEventChannel", order = 0)]
    public class TransformEventChannel : ScriptableObject
    {
        public UnityAction<Transform> onEventRaised;

        public void RaiseEvent(Transform val)
        {
            onEventRaised?.Invoke(val);
        }
    }
}