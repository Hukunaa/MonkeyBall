using Runtime.ScriptableObjects.DataContainers;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using ScriptableObjects.DataContainers;
using System.Collections.Generic;
using ScriptableObjects;

public class SeasonPassHeader : MonoBehaviour
{
    [SerializeField]
    private Image _light;
    [SerializeField]
    private TextMeshProUGUI _xpText;
    [SerializeField]
    private TextMeshProUGUI _rankText;
    [SerializeField]
    private GameObject _notification;
    [SerializeField]
    private PlayerDataContainer _playerDataContainer;
    [SerializeField]
    private BattlePassRewardManager _playerBattlePass;

    private void Awake()
    {        
        _playerDataContainer.OnPlayerDataLoaded += GetUpdateInfo;
        _playerBattlePass.Initialize();
    }

    void Start()
    {
        GetUpdateInfo();
    }

    public void GetUpdateInfo()
    {
        Debug.Log("Updating header");
        if (_playerBattlePass.Rewards.Any(p => _playerDataContainer.PlayerBattlePassXp.BattlePassXp >= p.XPRequired
        && !p.IsUsed))
            _notification.SetActive(true);
        else
            _notification.SetActive(false);

        List<RewardItem> _sortedItems = _playerBattlePass.Rewards.OrderBy(p => p.XPRequired).ToList();
        RewardItem _lastItemClaimed = null;
        int _lastItemXP = 0;
        int _nextItemToClaimXP = 0;
        for (int i = 0; i < _sortedItems.Count; ++i)
        {
            if ((_playerDataContainer.PlayerBattlePassXp.BattlePassXp >= _sortedItems[i].XPRequired))
            {
                _lastItemClaimed = _sortedItems[i];
                _lastItemXP = _sortedItems[i].XPRequired;
                _nextItemToClaimXP = (i >= _sortedItems.Count - 1) ? _sortedItems[i].XPRequired : _sortedItems[i + 1].XPRequired;
            }
            /*else if(_playerDataContainer.PlayerBattlePassXP == 0)
            {
                _lastItemXP = 0;
                _nextItemToClaimXP = _sortedItems[0].XPRequired;
            }*/
        }

        int _xp = _playerDataContainer.PlayerBattlePassXp.BattlePassXp;
        if (_light)
        {
            _light.fillAmount = (float)(_xp - _lastItemXP) / (float)(_nextItemToClaimXP - _lastItemXP);
        }

        if (_rankText)
        {
            if(_lastItemClaimed != null)
            {
                _rankText.text = (_sortedItems.IndexOf(_lastItemClaimed)).ToString();
            }
            else
            {
                _rankText.text = (1).ToString();
            }
        }

        if (_xpText)
            _xpText.text = _xp.ToString() + " / " + _nextItemToClaimXP.ToString();
    }

    void Update()
    {
    }

    private void OnDestroy()
    {
        _playerDataContainer.OnPlayerDataLoaded -= GetUpdateInfo;
    }
}
