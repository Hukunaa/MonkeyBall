using System;
using GeneralScriptableObjects;
using ScriptableObjects.DataContainer;
using TMPro;
using UnityEngine;

namespace UI.GameplayUI
{
    public class RemainingPlayersUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _remainingPlayerTMP;

        [SerializeField] 
        private RemainingPlayersContainer _remainingPlayersContainer;

        [SerializeField] 
        private bool _showPlayerTurnCompleteCount;

        [SerializeField] 
        private EPlayerTurnCompleteFormat _turnCompleteCountFormat;
        
        [SerializeField]
        private IntVariable _playerTurnCompleteCount;

        private void Awake()
        {
            _remainingPlayersContainer.OnRemainingPlayersChanged += UpdateRemainingPlayerText;
            if (_showPlayerTurnCompleteCount)
            {
                _playerTurnCompleteCount.onValueChanged += UpdateRemainingPlayerText;
            }
        }

        private void OnDestroy()
        {
            _remainingPlayersContainer.OnRemainingPlayersChanged -= UpdateRemainingPlayerText;
            if (_showPlayerTurnCompleteCount)
            {
                _playerTurnCompleteCount.onValueChanged -= UpdateRemainingPlayerText;
            }
        }

        private void UpdateRemainingPlayerText()
        {
            _remainingPlayerTMP.text = _showPlayerTurnCompleteCount?
                $"{ProcessPlayerTurnCompleteCountValue().ToString()}/{_remainingPlayersContainer.RemainingPlayersCount.ToString()}" :
                _remainingPlayersContainer.RemainingPlayersCount.ToString();
        }

        private int ProcessPlayerTurnCompleteCountValue()
        {
            int playerTurnCompleteText = 0;
            switch (_turnCompleteCountFormat)
            {
                case EPlayerTurnCompleteFormat.Increment:
                    playerTurnCompleteText = _playerTurnCompleteCount.Value;
                    break;
                case EPlayerTurnCompleteFormat.Decrement:
                    playerTurnCompleteText = _remainingPlayersContainer.RemainingPlayersCount -
                                             _playerTurnCompleteCount.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return playerTurnCompleteText;
        }

        private enum EPlayerTurnCompleteFormat
        {
            Increment,
            Decrement
        }
    }
}
