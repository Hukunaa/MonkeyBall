using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Gameplay.Character;
using ScriptableObjects.DataContainer;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayManagers
{
    public class PodiumManager : MonoBehaviour
    {
        [SerializeField] 
        private PodiumConfiguration[] _platformConfigurations;

        [SerializeField] 
        private PlayersRankContainer _playersRankContainer;

        [SerializeField][Range(2,4)] 
        private int _podiumSize = 4;

        [SerializeField] 
        private CinemachineTargetGroup _winCameraTargetGroup;
        
        [SerializeField] 
        private UnityEvent _onPodiumReady;
        
        public void SetUpPodium()
        {
            var config = FindConfiguration();
            
            foreach (var configPlatform in config.Platforms)
            {
                _winCameraTargetGroup.AddMember(configPlatform.transform, 1, 0);
            }
            
            List<Player> players = new List<Player>();
            for (int i = 1; i <= _podiumSize; i++)
            {
                players.Add(_playersRankContainer.Ranking[i]); 
            }
            
            config.ActivateConfiguration(players.ToArray());
            
            _onPodiumReady?.Invoke();
        }

        private PodiumConfiguration FindConfiguration()
        {
            var configuration = _platformConfigurations.FirstOrDefault(x => x.PodiumSize == _podiumSize);
            if (configuration == null)
            {
                Debug.LogError("Cannot find a configuration matching the WinPlatformManager Podium Size.");
            }

            return configuration;
        }
    }
}
