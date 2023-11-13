using UnityEngine;
using UnityEngine.Events;

namespace GeneralScriptableObjects.EventChannels
{
    [CreateAssetMenu(fileName = "Color Event Channel", menuName = "ScriptableObjects/Events/ColorEventChannel", order = 0)]
    public class ColorEventChannel : ScriptableObject
    {
        public UnityAction<Color> onEventRaised;

        public void RaiseEvent(Color col)
        {
            onEventRaised?.Invoke(col);
        }
    }
}