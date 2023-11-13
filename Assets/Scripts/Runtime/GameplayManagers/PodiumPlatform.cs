using Gameplay.Character;
using Gameplay.Player;
using UI.GameplayUI;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayManagers
{
    public class PodiumPlatform : MonoBehaviour
    {
        [SerializeField] 
        private Animator _animator;
        
        [SerializeField] 
        private PlayerSkinHandler _playerSkinHandler;

        [SerializeField] 
        private PlayerNameUI _playerNameUI;
        
        [SerializeField] 
        private UnityEvent _onPlatformInitialized;

        private static readonly int IsWinner = Animator.StringToHash("IsWinner");

        public void SetupWinPlatform(Player _player)
        {
            _playerSkinHandler.UpdateHeadSkin(_player.PlayerSkinHandler.CurrentHeadMesh); 
            _playerSkinHandler.UpdateBodySkin(_player.PlayerSkinHandler.CurrentBodyMesh);
            _playerSkinHandler.UpdateSkinColor(_player.PlayerSkinHandler.CurrentSkinColorMeshes);
            _playerSkinHandler.UpdateFaceSkin(_player.PlayerSkinHandler.CurrentFaceMesh);

            _playerNameUI.SetPlayerInfo(_player);
            _playerNameUI.gameObject.SetActive(_player.IsPlayer == false);
            
            _onPlatformInitialized?.Invoke();
        }

        public void PlayWinAnimation()
        {
            _animator.SetBool(IsWinner, true);
        }
    }
}
