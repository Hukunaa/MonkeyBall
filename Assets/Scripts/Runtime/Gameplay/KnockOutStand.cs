using System;
using GeneralScriptableObjects;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class KnockOutStand : MonoBehaviour
    {
        [SerializeField] 
        private Transform _standTransform;

        [SerializeField] 
        private TMP_Text _scoreText;
    
        [SerializeField] 
        private TMP_Text _timerText;

        [SerializeField] 
        private IntVariable _scoreIncrementationSpeed;

        [SerializeField]
        private GameObject _playerGlow;

        [SerializeField]
        private ParticleSystem _knockoutFX;
    
        private Character.Player _assignedPlayer;

        private bool _assigned;
        private bool _playerKnockedOut;
        private int _scoreIncrement;

        public void AssignPlayer(Character.Player _player)
        {
            _assignedPlayer = _player;
            _player.KnockOutStand = this;
            _assigned = true;
            if (_player.IsPlayer)
            {
                _playerGlow.SetActive(true);
            }
        }

        public void Initialize()
        {
            MovePlayerToStand();

            _scoreIncrement = 0;
            _scoreText.text = "0";
            _scoreText.enabled = true;
            _timerText.enabled = false;
        }

        public void HideUI()
        {
            _scoreText.enabled = false;
            _timerText.enabled = false;
        }

        public void MovePlayerToStand()
        {
            AssignedPlayer.PlayerKnockOut.MoveToStand(_standTransform.position);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public bool TryReachingScore()
        {
            if (_scoreIncrement == AssignedPlayer.Score.CurrentScore) return true;
            
            _scoreIncrement = Mathf.Clamp(_scoreIncrement + _scoreIncrementationSpeed.Value, 0, AssignedPlayer.Score.CurrentScore);
            _scoreText.text = _scoreIncrement.ToString();
            return false;
        }

        public void DisplayScore()
        {
            _scoreText.text = AssignedPlayer.Score.CurrentScore.ToString();
        }

        public void KnockOutPlayer(bool _showTime)
        {
            _assignedPlayer.PlayerKnockOut.KnockOutPlayer();
            if (_knockoutFX != null)
            {
               _knockoutFX.Play();
            }


            if (!_showTime) return;
            _timerText.text = TimeSpan.FromSeconds(AssignedPlayer.Timer.TimerValue).ToString(@"mm\:ss\:ff");
            _timerText.enabled = true;
        }

        public void QualifyPlayer()
        {

        }

        public bool Assigned => _assigned;

        public Character.Player AssignedPlayer => _assignedPlayer;
    }
}
