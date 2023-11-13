using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Character
{
    public class PlayerDistanceManager : MonoBehaviour
    {
        private Vector3 _startLocation;
        
        private int _distanceTraveled;
        private bool _updateDistance;

        public UnityAction<int> _onDistanceChanged;

        private void Update()
        {
            if (_updateDistance == false) return;
            _distanceTraveled = Mathf.RoundToInt(CalculateDistance());
            _onDistanceChanged?.Invoke(_distanceTraveled);
        }
        
        public void UpdateStartLocation(Vector3 _newStartLocation)
        {
            _startLocation = _newStartLocation;
        }

        public void StartUpdatingDistance()
        {
            _updateDistance = true;
        }

        public void StopUpdatingDistance()
        {
            _updateDistance = false;
        }

        public void ResetDistance()
        {
            _distanceTraveled = 0;
            _onDistanceChanged?.Invoke(_distanceTraveled);
        }

        public int CalculateDistance()
        {
            Vector2 startLocation = new Vector2(_startLocation.x, _startLocation.z);
            var position = transform.position;
            Vector2 currentLocation = new Vector2(position.x, position.z);
            return Mathf.RoundToInt(Vector2.Distance(startLocation, currentLocation));
        }
        
        public int DistanceTraveled => _distanceTraveled;
    }
}