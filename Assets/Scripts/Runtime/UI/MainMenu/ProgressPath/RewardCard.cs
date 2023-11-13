using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableObjects.DataContainers;
using ScriptableObjects.Settings;

namespace Runtime.UI.MainMenuUI.StarProgressPath
{
    public class RewardCard : MonoBehaviour
    {
        [SerializeField]
        private Image _rewardImage;
        [SerializeField]
        private Image _rewardBackgroundIcon;
        [SerializeField]
        private TMP_Text _rewardTitle;
        [SerializeField]
        private TMP_Text _rewardDesc;
        [SerializeField]
        private TMP_Text rewardXPNeededText;
        [SerializeField]
        private Button _rewardCollectButton;
        [SerializeField]
        private TMP_Text _rewardCollectButtonText;
        [SerializeField]
        private GameObject _rewardComplete;
        [SerializeField]
        private GameObject _rewardLock;
        [SerializeField]
        private GameObject _rewardScalableElement;
        [SerializeField]
        private RarityColors _rarityColors;

        private int _rewardXPNeeded;
        RewardItem _linkedItem;

        public void SetRewardCard(RewardItem _reward, int _xpRequirement)
        {
            _linkedItem = _reward;
            _rewardXPNeeded = _xpRequirement;

            if (_linkedItem == null)
                return;
            if(_linkedItem.RewardType == REWARD_TYPE.SKIN)
            {
                _rewardBackgroundIcon.color = _rarityColors.Colors[(int)_linkedItem.SkinReward.Rarity];
            }

            if (_rewardTitle != null)
                _rewardTitle.text = _linkedItem.Title;
            if (_rewardDesc != null)
                _rewardDesc.text = _linkedItem.Description;
            if (_rewardImage != null)
                _rewardImage.sprite = _linkedItem.RewardSprite;
            if (rewardXPNeededText != null)
                rewardXPNeededText.text = _rewardXPNeeded.ToString();
        }

        public void SetRewardLockState(bool _value, bool _doLockButton = true)
        {
            _rewardLock.SetActive(_value);
            if(_doLockButton)
            {
                _rewardCollectButton.interactable = !_value;
                _rewardCollectButtonText.text = _rewardCollectButton.interactable ? "Claim" : "Locked";
            }
        }

        public void SetRewardState(bool _value)
        {
            if (_rewardComplete != null)
            {
                _rewardComplete.SetActive(!_value);
                _rewardCollectButton.gameObject.SetActive(_value);
            }
        }
        public Button InteractionButton { get => _rewardCollectButton; }
        public RewardItem LinkedItem { get => _linkedItem; }
        public GameObject RewardScalableElement { get => _rewardScalableElement; }
    }
}
