using GeneralScriptableObjects.EventChannels;
using Runtime.UI.UIUtility;
using UnityEngine;

namespace Runtime.UI.MainMenuUI
{
    public class TabManager : MonoBehaviour
    {
        [SerializeField] private MainMenuTab _marketTab;
        [SerializeField] private MainMenuTab _playerTab;
        [SerializeField] private MainMenuTab _homeTab;
        [SerializeField] private MainMenuTab _eventTab;
        [SerializeField] private MainMenuTab _socialTab;

        [SerializeField] private SwipeMenuLayout _swipeMenu;

        [Header("Listening to")]
        [SerializeField] private VoidEventChannel[] _tabInitializedEventChannel;

        private MainMenuTab _currentTab;

        public void TabsInitialized()
        {
            ShowHomeTab();
        }

        public void ShowMarketTab()
        {
            SwitchTab(_marketTab);
        }

        public void ShowPlayerTab()
        {
            SwitchTab(_playerTab);
        }

        public void ShowHomeTab()
        {
            SwitchTab(_homeTab);
        }

        public void ShowEventTab()
        {
            SwitchTab(_eventTab);
        }

        public void ShowSocialTab()
        {
            SwitchTab(_socialTab);
        }

        public void SwitchTab(MainMenuTab _newTab)
        {
            _currentTab = _newTab;
            _swipeMenu.ShowSelectedTab(_currentTab.GetComponent<RectTransform>());
        }

        public void ShowCurrentTab()
        {
            if (_currentTab == null) 
                return;

            _swipeMenu.ShowSelectedTab(_currentTab.GetComponent<RectTransform>());
        }

        public MainMenuTab MarketTab { get => _marketTab; }
        public MainMenuTab PlayerTab { get => _playerTab; }
        public MainMenuTab HomeTab { get => _homeTab; }
        public MainMenuTab EventTab { get => _eventTab; }
        public MainMenuTab SocialTab { get => _socialTab; }

    }
}