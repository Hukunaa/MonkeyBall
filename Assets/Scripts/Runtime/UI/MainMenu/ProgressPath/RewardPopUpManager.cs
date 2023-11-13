using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenuUI.ProgressPath
{
    public class RewardPopUpManager : MonoBehaviour
    {
        [SerializeField] 
        private RectTransform _rectTransform;

        [SerializeField] 
        private Image _popUpFrontImage;

        [SerializeField] 
        private Color _popUpDefaultColor = Color.white;
        
        [SerializeField]
        private TMP_Text _rewardHeaderText;
        
        private RewardPopup _activePopup;

        /*public void ShowRewardItemPopup(RewardItem _rewardItem)
        {
            switch (_rewardItem.RewardType)
            {
                case REWARD_TYPE.COINS:
                    _coinRewardPopup.InitializePopup(_rewardItem);
                    _rewardHeaderText.text = _coinRewardPopup.RewardHeaderText;
                    SetPopupFrontImageColor(_popUpDefaultColor);
                    SetPopupActive(_coinRewardPopup);
                    break;
                case REWARD_TYPE.RANK:
                    _rankRewardPopup.InitializePopup(_rewardItem);
                    _rewardHeaderText.text = _rankRewardPopup.RewardHeaderText;
                    SetPopupFrontImageColor(_popUpDefaultColor);
                    SetPopupActive(_rankRewardPopup);
                    break;
                case REWARD_TYPE.CHEF:
                    _chefRewardPopup.InitializePopup(_rewardItem);
                    _rewardHeaderText.text = _chefRewardPopup.RewardHeaderText;
                    SetPopupFrontImageColor(_chefRewardPopup.RarityColor);
                    SetPopupActive(_chefRewardPopup);
                    break;
                case REWARD_TYPE.RECIPE:
                    _recipeRewardPopup.InitializePopup(_rewardItem);
                    _rewardHeaderText.text = _recipeRewardPopup.RewardHeaderText;
                    SetPopupFrontImageColor(_recipeRewardPopup.RarityColor);
                    SetPopupActive(_recipeRewardPopup);
                    break;
                case REWARD_TYPE.CHEST:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        
            OpenPopup();
        }*/

        private void SetPopupFrontImageColor(Color _color)
        {
            _popUpFrontImage.color = _color;
        }
    
        private void SetPopupActive(RewardPopup _rewardPopup)
        {
            _activePopup = _rewardPopup;
            _rewardPopup.gameObject.SetActive(true);
        }

        private void ResetActivePopup()
        {
            _activePopup.gameObject.SetActive(false);
            _activePopup = null;
        }
    
        private void OpenPopup()
        {
            _rectTransform.localScale = Vector3.one * 0.8f;
            gameObject.SetActive(true);
            _rectTransform.DOKill();
            _rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutExpo);
        }
    
        public void ClosePopup()
        {
            _rectTransform.DOKill();
            _rectTransform.DOScale(Vector3.one * 0.8f, 0.2f).SetEase(Ease.InExpo).OnComplete(() =>
            {
                gameObject.SetActive(false);
                ResetActivePopup();
            });
        }
    }
}
