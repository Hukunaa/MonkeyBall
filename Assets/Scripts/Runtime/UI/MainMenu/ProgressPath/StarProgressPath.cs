using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomUtilities;
using DG.Tweening;
using Runtime.ScriptableObjects.DataContainers;
using Runtime.UI.MainMenuUI.StarProgressPath;
using SceneManagementSystem.Scripts;
using ScriptableObjects;
using ScriptableObjects.DataContainers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Utilities;

namespace Runtime.UI.MainMenuUI.ProgressPath
{
    public class StarProgressPath : MonoBehaviour
    {
        [SerializeField]
        private AssetReference _cardRewardDefault;
        [SerializeField]
        private AssetReference _cardRewardPrefab;
        [SerializeField]
        private AssetReference _cardRewardInvertedPrefab;
        [SerializeField]
        private GameObject _starProgressContent;
        [SerializeField]
        private ScrollRect _starProgressScrollRect;
        [SerializeField]
        private RectTransform _progressBar;
        [SerializeField]
        private GameObject _chefLoadingLogo;

        [SerializeField] 
        private RewardPopUpManager _rewardPopUpManager;
        [SerializeField]
        private BattlePassRewardManager _battlePass;
        [SerializeField]
        private SeasonPassHeader _seasonPassHeader;

        private List<RewardCard> _rewardCards;
        private int _side = 0;
        private PlayerDataContainer _playerContainer;

        private bool _isOpen;
        private RewardCard _maxXPCard;
        private RewardCard _maxDifferenceXPSelectedCard;

        private void Start()
        {
            _rewardCards = new List<RewardCard>();
        }
        async Task LoadAssets()
        {
            _rewardCards = new List<RewardCard>();

            if (_playerContainer == null)
                _playerContainer = GameManager.Instance.PlayerDataContainer;

            for (int i = 0; i < _battlePass.Rewards.Count; ++i)
            {
                RewardItem item = _battlePass.Rewards[i];
                AssetReference prefab = _side % 2 == 0 ? _cardRewardPrefab : _cardRewardInvertedPrefab;
                if(_battlePass.Rewards[i].RewardType == REWARD_TYPE.NONE)
                {
                    prefab = _cardRewardDefault;
                }

                GameObject _inst = await prefab.InstantiateAsync(_starProgressContent.GetComponent<RectTransform>()).Task;
                RewardCard card = _inst.GetComponent<RewardCard>();
                _rewardCards.Add(card);
                card.SetRewardCard(item, item.XPRequired);

                if (!_battlePass.Rewards[i].IsUsed)
                {
                    card.InteractionButton.onClick.AddListener(delegate { RewardPlayer(item, card); _seasonPassHeader.GetUpdateInfo(); });
                    card.SetRewardState(true);
                }
                else
                    card.SetRewardState(false);

                card.RewardScalableElement.GetComponent<RectTransform>().localScale = Vector3.zero;
                _side++;
            }
        }

        public async void ShowProgressPath()
        {
            if (_rewardCards == null)
            {
                _chefLoadingLogo.SetActive(true);
                await LoadAssets();
            }

            foreach (RewardCard _item in _rewardCards)
            {
                if (_item == null)
                    break;

                _item.SetRewardLockState(!(_playerContainer.PlayerBattlePassXp.BattlePassXp >= _item.LinkedItem.XPRequired));
                AnimateCard(_item);
            }

            _isOpen = true;
            _chefLoadingLogo.SetActive(false);
            ShowProgressBar();
            Debug.Log(_maxXPCard);
            Debug.Log(_maxDifferenceXPSelectedCard);
            ScrollViewFocusFunctions.FocusOnItem(_starProgressScrollRect, _maxDifferenceXPSelectedCard.GetComponent<RectTransform>());
        }

        public void ShowProgressBar()
        {
            List<RewardCard> _sortedCards = _rewardCards.OrderBy(p => p.LinkedItem.XPRequired).ToList();
            for (int i = 0; i < _sortedCards.Count; ++i)
            {
                if ((_playerContainer.PlayerBattlePassXp.BattlePassXp >= _sortedCards[i].LinkedItem.XPRequired))
                {
                    _maxXPCard = _sortedCards[i];
                    _maxDifferenceXPSelectedCard = (i == _sortedCards.Count - 1) ? _sortedCards[i] : _sortedCards[i + 1];
                }
                /*else if(_playerContainer.PlayerBattlePassXP == 0)
                {
                    _maxXPCard = _sortedCards[0];
                    _maxDifferenceXPSelectedCard = _sortedCards[1];
                }*/
            }
        }

        private void Update()
        {
            if(_isOpen)
            {
                if(_maxXPCard != null)
                {
                    if (_maxDifferenceXPSelectedCard.transform.position.y == _maxXPCard.transform.position.y)
                    {
                        _progressBar.position = new Vector3(_progressBar.position.x, _maxDifferenceXPSelectedCard.transform.position.y, _progressBar.position.z);
                    }
                    else
                    {
                        float offsetY = MathCalculation.Remap(_playerContainer.PlayerBattlePassXp.BattlePassXp,
                            _maxXPCard.LinkedItem.XPRequired, _maxDifferenceXPSelectedCard.LinkedItem.XPRequired,
                            _maxXPCard.transform.position.y, _maxDifferenceXPSelectedCard.transform.position.y);
                        _progressBar.position = new Vector3(_progressBar.position.x, offsetY, _progressBar.position.z);
                    }
                }
                
            }
        }
        public void HideProgressPath()
        {
            foreach (RewardCard _card in _rewardCards)
            {
                _card.DOKill();
                _card.RewardScalableElement.GetComponent<RectTransform>().localScale = Vector3.zero;
            }
            _isOpen = false;
            gameObject.SetActive(false);

        }

        void CompleteReward(RewardCard _card, RewardItem _item)
        {
            int index = _battlePass.Rewards.IndexOf(_item);
            _battlePass.Rewards[index].Complete();
            DataLoader.SavePlayerRewardsState(_battlePass.Rewards.Select(p => p.IsUsed).ToArray());
            _card.SetRewardState(false);
        }

        void RewardPlayer(RewardItem _item, RewardCard _card)
        {
            switch (_item.RewardType)
            {
                case REWARD_TYPE.COINS:
                    _playerContainer.Currencies.AddBalance(Enums.ECurrencyType.SoftCurrency, _item.CoinsReward);
                    //_rewardPopUpManager.ShowRewardItemPopup(_item);
                    break;
                case REWARD_TYPE.SKIN:
                    _playerContainer.PlayerSkinsInventory.AddSkin(_item.SkinReward);
                    break;
                default:
                    break;
            }
            CompleteReward(_card, _item);
        }

        void AnimateCard(RewardCard _card)
        {
            _card.DOKill();
            _card.RewardScalableElement.GetComponent<RectTransform>().DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
    }
}
