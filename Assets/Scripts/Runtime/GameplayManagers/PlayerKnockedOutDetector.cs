using Gameplay.Character;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayManagers
{
    public class PlayerKnockedOutDetector : MonoBehaviour
    {
        [SerializeField] 
        private UnityEvent _onPlayerEliminated;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            var player = other.GetComponent<Player>();

            if (player.IsPlayer)
            {
                Debug.Log("Player was eliminated");
                _onPlayerEliminated?.Invoke();
            }
            player.KnockOut();
        }
    }
}
