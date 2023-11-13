using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayUI
{
    public class SpeedUI : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rb;

        [SerializeField] 
        private TMP_Text _tmpt;

        [SerializeField]
        private Image _bar;

        [SerializeField]
        private int _maxSpeed = 150;

        private Transform _target;

        private int _currentSpeed;
        private float _speedFill;
    
        public Rigidbody Player
        {
            get => _rb;
            set => _rb = value;
        }
    
        private void Update()
        {
            if (_rb != null)
            _currentSpeed = Mathf.RoundToInt(_rb.velocity.magnitude * 3.6f);
            _tmpt.text = _currentSpeed.ToString("");

            if (_currentSpeed > _maxSpeed)
            {
                _currentSpeed = _maxSpeed;
            }

            _speedFill = (float)_currentSpeed / (float)_maxSpeed;

            _bar.fillAmount = _speedFill;
        }

        public Transform Target
        {
            get => _target;
            set
            {
                _target = value;
                _rb = value.GetComponent<Rigidbody>();
            }
        }
    }
}
