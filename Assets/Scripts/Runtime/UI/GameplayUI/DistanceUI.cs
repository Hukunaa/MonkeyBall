using Gameplay.Character;
using TMPro;
using UnityEngine;

namespace UI.GameplayUI
{
    public class DistanceUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _distanceTMP;
        
        private PlayerDistanceManager _playerDistance;

        public void OnTargetSet(Transform _transform)
        {
            _playerDistance = _transform.GetComponent<PlayerDistanceManager>();
            _playerDistance._onDistanceChanged += UpdateDistanceUI;
            UpdateDistanceUI(_playerDistance.DistanceTraveled);
        }
        
        private void OnEnable()
        {
            if (_playerDistance == null) return;
            _playerDistance._onDistanceChanged += UpdateDistanceUI;
        }

        private void OnDisable()
        {
            if (_playerDistance == null) return;
            _playerDistance._onDistanceChanged -= UpdateDistanceUI;
        }

        private void UpdateDistanceUI(int _newDistance)
        {
            _distanceTMP.SetText($"{_newDistance}");
        }
    }
}