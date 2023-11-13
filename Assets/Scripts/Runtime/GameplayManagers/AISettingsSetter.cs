using Gameplay.Character.AI;
using GeneralScriptableObjects.EventChannels;
using ScriptableObjects.Settings;
using UnityEngine;
using Random = UnityEngine.Random;


namespace GameplayManagers
{
    public class AISettingsSetter : MonoBehaviour
    {
        [SerializeField] private AITargetPracticeSettings[] _settings;

        [SerializeField] private VoidEventChannel _onPlayersSpawnedEventChannel;

        private void Awake()
        {
            _onPlayersSpawnedEventChannel.onEventRaised += SetAISettings;
        }
    
        private void SetAISettings()
        {
            _onPlayersSpawnedEventChannel.onEventRaised -= SetAISettings;

            if (_settings == null || _settings.Length == 0) return;
        
            var aiControllers = FindObjectsOfType<AIController>();

            if (aiControllers.Length == 0)
            {
                Debug.LogWarning("There is no AIController in the scene. Cannot assign AI Settings.");
            }

            foreach (var aiController in aiControllers)
            {
                var settingIndex = Random.Range(0, _settings.Length);
                aiController.Settings = _settings[settingIndex];
            }
        }
    }
}
