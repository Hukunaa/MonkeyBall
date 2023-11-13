using Gameplay.Character;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.EventChannels
{
    [CreateAssetMenu(fileName = "PlayerInfoEventChannel", menuName = "ScriptableObjects/Events/PlayerInfoEventChannel", order = 0)]
    public class PlayerInfoEventChannel : ScriptableObject
    {
        public UnityAction<Player> onEventRaised;

        public void RaiseEvent(Player val)
        {
            onEventRaised?.Invoke(val);
        }
    }
}