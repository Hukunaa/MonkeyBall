using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "GameplaySettings", menuName = "ScriptableObjects/Settings/GameplaySettings", order = 0)]
    public class GameplaySettings : ScriptableObject
    {
        [SerializeField] 
        private int _playerAmount = 8;

        public int PlayerAmount => _playerAmount;
    }
}