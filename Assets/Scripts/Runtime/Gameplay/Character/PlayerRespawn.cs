using GameplayManagers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Gameplay.Character
{
    public class PlayerRespawn : MonoBehaviour
    {
        [FormerlySerializedAs("_respawnTransform")] [SerializeField] 
        private SpawnPoint _spawnPoint;

        [SerializeField] 
        private UnityEvent _onRespawn;

        [SerializeField] 
        private UnityEvent<Vector3> _onRespawnLocationAssigned;
        
        public void AssignSpawnPoint(SpawnPoint _spawnPoint)
        {
            this._spawnPoint = _spawnPoint;
            _onRespawnLocationAssigned?.Invoke(_spawnPoint.transform.position);
        }
    
        public void Respawn()
        {
            var spawnPointTransform = _spawnPoint.transform;

            transform.SetPositionAndRotation(spawnPointTransform.position, spawnPointTransform.rotation);
            _onRespawn?.Invoke();
        }
        
        public SpawnPoint SpawnPoint => _spawnPoint;
    }
}
