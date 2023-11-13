using UnityEngine;

namespace Runtime.UI.MainMenuUI.ProgressPath
{
    public abstract class RewardPopup : MonoBehaviour
    {
        [SerializeField] 
        private string _rewardHeaderText;

        public string RewardHeaderText
        {
            get => _rewardHeaderText;
            set => _rewardHeaderText = value;
        }

        public abstract void InitializePopup(/*RewardItem _rewardItem*/);

    }
}