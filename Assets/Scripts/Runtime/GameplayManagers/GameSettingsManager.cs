using GeneralScriptableObjects;
using ScriptableObjects.Settings;
using UnityEngine;

namespace GameplayManagers
{
    public class GameSettingsManager : MonoBehaviour
    {
        [SerializeField] 
        private GameplaySettings _gameplaySettings;

        [SerializeField] 
        private IntVariable _playersAmount;

        private void Awake()
        {
            _playersAmount.SetValue(_gameplaySettings.PlayerAmount);
        }
    }
}