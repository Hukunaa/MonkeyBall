using System.Collections.Generic;
using Gameplay.Character;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.GameplayUI
{
    public class AttemptFeedbackUI : MonoBehaviour
    {
        [SerializeField] 
        private Player _player;

        [SerializeField] 
        private TMP_Text _roundFeedbackTmp;

        [SerializeField]
        private TMPAnimator _roundFeedbackAnimator;

        [SerializeField]
        private int _duration;

        [SerializeField]
        private List<string> outStrings = new List<string>();

        [SerializeField]
        private List<string> onTargetStrings = new List<string>();
        
        [SerializeField] 
        private string _timeOutText = "Time Out";
        
        public void SetFeedback()
        {
            if (_player.PlayerAttemptManager.ReachedTarget)
            {
                if (onTargetStrings.Count > 0)
                {
                    int randomIndex = Random.Range(0, onTargetStrings.Count);
                    string randomString = onTargetStrings[randomIndex];
                    StartCoroutine(_roundFeedbackAnimator.ShowTextForDuration(randomString, _duration));
                }
            }

            else if (_player.PlayerAttemptManager.PlayerTurnInProgress)
            {
                _roundFeedbackTmp.text = _timeOutText;
            }

            else
            {
                if (outStrings.Count > 0)
                {
                    int randomIndex = Random.Range(0, outStrings.Count);
                    string randomString = outStrings[randomIndex];
                    StartCoroutine(_roundFeedbackAnimator.ShowTextForDuration(randomString, _duration));
                }
            }
        }
        
        public void SetTarget(Transform _newTarget)
        {
            _player = _newTarget.GetComponent<Player>();
        }

        public Player Player => _player;
    }
}
