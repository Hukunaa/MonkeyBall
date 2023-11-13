using Gameplay.Character;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI.GameplayUI
{
    [RequireComponent(typeof(Canvas))]
    public class PlayerNameUI : MonoBehaviour
    {
        [SerializeField] 
        private Player _player;

        [SerializeField] 
        private TMP_Text _playerName_tmp;

        [SerializeField] 
        private UnityEvent<Transform> _onTargetChanged;
        
        private Canvas _canvas;
       
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        public void SetPlayerInfo(Player _player)
        {
            this._player = _player;
            _player.onPlayerNameChanged += UpdatePlayerName;
            _onTargetChanged?.Invoke(_player.transform);
            UpdatePlayerName();
        }

        private void UpdatePlayerName()
        {
            _playerName_tmp.text = _player.PlayerName;
        }

        public void Show()
        {
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }
    }
}
