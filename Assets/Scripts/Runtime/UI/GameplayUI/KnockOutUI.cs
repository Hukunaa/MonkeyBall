using ScriptableObjects.DataContainer;
using TMPro;
using UnityEngine;

namespace UI.GameplayUI
{
    public class KnockOutUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _remainingKnockOutCounter_tmp;

        [SerializeField]
        private RemainingPlayersContainer _remainingPlayersContainer;
    
        private void Awake()
        {
            _remainingPlayersContainer.OnRemainingPlayersChanged += UpdateRemainingKnockOutCounter;
        }

        public void UpdateRemainingKnockOutCounter()
        {
            int remainingKnockOut = _remainingPlayersContainer.RemainingPlayersCount;
            _remainingKnockOutCounter_tmp.text = remainingKnockOut.ToString();
        }
    }
}
