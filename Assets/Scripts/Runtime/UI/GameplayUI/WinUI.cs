using ScriptableObjects.DataContainer;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI.GameplayUI
{
    public class WinUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _winnerNameTMP;

        [SerializeField]
        private PlayersRankContainer _playersRankContainer;

        [SerializeField] 
        private UnityEvent _onShowWinUI;
        
        public void SetupWinScreen()
        {
            _winnerNameTMP.text = _playersRankContainer.Ranking[1].PlayerName;
            _onShowWinUI?.Invoke();
        }
    }
}
