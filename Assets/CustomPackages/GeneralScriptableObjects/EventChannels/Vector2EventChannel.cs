using UnityEngine;
using UnityEngine.Events;

namespace GeneralScriptableObjects.EventChannels
{
    [CreateAssetMenu(fileName = "Vector2 Event Channel", menuName = "ScriptableObjects/Events/Vector2EventChannel", order = 0)]
    public class Vector2EventChannel : ScriptableObject
    {
        public UnityAction<Vector2> onEventRaised;

        public void RaiseEvent(Vector2 val)
        {
            onEventRaised?.Invoke(val);
        }
    }
}