using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayUI
{
    public class LeaderboardEntry : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _rankTmp;
        
        [SerializeField] 
        private TMP_Text _playerNameTmp;
        
        [SerializeField] 
        private TMP_Text _playerScoreTmp;

        [SerializeField] 
        private Image _entryBackground;

        [SerializeField] private Color _defaultBackgroundColor = Color.white;
        [SerializeField] private Color _knockOutBackgroundColor = Color.red;
        
        [SerializeField] private Color _playerEntryDefaultColor = Color.yellow;
        [SerializeField] private Color _playerEntryKnockoutColor = Color.gray;

        [SerializeField] private float _playerEntryScaleFactor = 1.1f;
        [SerializeField] private float _playerEntryScaleDuration = 1;

        
        public string PlayerName { get; set; }
        
        public void InitializeEntry(int _rank, string _playerName, int _playerScore, bool _inKnockOutPosition, bool _isPlayer)
        {
            _entryBackground.color = _inKnockOutPosition ? _knockOutBackgroundColor : _defaultBackgroundColor;

            if (_isPlayer)
            {
                transform.DOScale(Vector3.one * _playerEntryScaleFactor, _playerEntryScaleDuration).SetLoops(-1, LoopType.Yoyo);
                _entryBackground.color = _inKnockOutPosition ? _playerEntryKnockoutColor : _playerEntryDefaultColor;
            }

            PlayerName = _playerName;
            _rankTmp.text = _rank.ToString();
            _playerNameTmp.text = _playerName;
            _playerScoreTmp.text = _playerScore.ToString();

        }
    }
}