using UnityEngine;
using UnityEngine.Events;

namespace GeneralScriptableObjects.EventChannels
{
    public enum ANIMATION_STATE
    {
        ROLLING,
        FLYING,
        IDLE,
        FALLING,
        NONE
    }

    [CreateAssetMenu(fileName = "Animation State Event Channel", menuName = "ScriptableObjects/Events/AnimationStateEventChannel", order = 0)]
    public class AnimationStateEventChannel : ScriptableObject
    {
        public UnityAction<ANIMATION_STATE> onEventRaised;

        public void RaiseEvent(ANIMATION_STATE val)
        {
            onEventRaised?.Invoke(val);
        }
    }
}