using GeneralScriptableObjects;
using ScriptableObjects.DataContainer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayUI
{
    public class PlayerTurnCompleteUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _playersRoundCompleteCountText;

        [SerializeField] 
        private RemainingPlayersContainer _remainingPlayersContainer;

        [SerializeField] 
        private IntVariable _playerTurnCompleteCount;

        [SerializeField]
        private Image _button01;

        [SerializeField]
        private Image _button02;

        [SerializeField]
        private Image _button03;

        [SerializeField]
        private Color _buttonSelected;

        [SerializeField]
        private Color _buttonNormal;

        private void Awake()
        {
            _playerTurnCompleteCount.onValueChanged += UpdateUI;
        }

        private void OnDestroy()
        {
            _playerTurnCompleteCount.onValueChanged -= UpdateUI;
        }

        private void UpdateUI()
        {
            _playersRoundCompleteCountText.text = $"{_playerTurnCompleteCount.Value}/{_remainingPlayersContainer.RemainingPlayersCount}";
        }

        public void UpdateButtons(int button)
        {
            switch(button)
            {
                case 1:
                    _button01.color = _buttonSelected;
                    _button02.color = _buttonNormal;
                    _button03.color = _buttonNormal;
                    break;

                case 2:
                    _button01.color = _buttonNormal;
                    _button02.color = _buttonSelected;
                    _button03.color = _buttonNormal;
                    break;

                case 3:
                    _button01.color = _buttonNormal;
                    _button02.color = _buttonNormal;
                    _button03.color = _buttonSelected;
                    break;

                default:
                    break;
            }
        }
    }
}