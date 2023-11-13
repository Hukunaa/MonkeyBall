using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class WindController : MonoBehaviour
    {
        [SerializeField] 
        private float _lowWindForce;
        [SerializeField]
        private float _midWindForce;
        [SerializeField]
        private float _maxWindForce;
        [SerializeField]
        private Material _lowWindMat;
        [SerializeField]
        private Material _midWindMat;
        [SerializeField]
        private Material _highWindMat;
        [SerializeField]
        private MeshRenderer _arrowMesh1;
        [SerializeField]
        private MeshRenderer _arrowMesh2;

        private Vector3 _windDirection;
        private float _windForce;

        public Action _onWindChanged;

        public void RandomizeWind()
        {
            var randomPoint = Random.insideUnitCircle.normalized;
            _windDirection = new Vector3(randomPoint.x, 0,  randomPoint.y);
            _windForce = Random.Range(0, _maxWindForce);

            if(_windForce < _lowWindForce)
            {
                _arrowMesh1.material = _lowWindMat;
                _arrowMesh2.material = _lowWindMat;
            }
            else if (_windForce < _midWindForce)
            {
                _arrowMesh1.material = _midWindMat;
                _arrowMesh2.material = _midWindMat;
            }
            else
            {
                _arrowMesh1.material = _highWindMat;
                _arrowMesh2.material = _highWindMat;
            }
            _onWindChanged?.Invoke();
        }

        public Vector3 WindDirection => _windDirection;

        public float WindForce => _windForce;

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position, transform.position + _windDirection);
        }
    }
}