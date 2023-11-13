using System.Collections.Generic;
using Gameplay.Character;
using UnityEngine;

namespace GameplayManagers
{
    public class PodiumConfiguration : MonoBehaviour
    {
        [SerializeField][Tooltip("Order matter. Start with the first position podium, then second etc...")]
        private List<PodiumPlatform> _platforms;

        public void ActivateConfiguration(Player[] _players)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _platforms[i].SetupWinPlatform(_players[i]);
            }

            gameObject.SetActive(true);
            
            _platforms[0].PlayWinAnimation();
        }

        public List<PodiumPlatform> Platforms => _platforms;

        public int PodiumSize => _platforms.Count;
    }
}