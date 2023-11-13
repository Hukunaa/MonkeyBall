using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Runtime.UI.MainMenuUI
{
    [RequireComponent(typeof(UIDocument))]
    public class HomeMenuUI : UIToolkitMonoBehavior
    {
        private Button _marketNavButton;
        private Button _cookNavButton;
        private Button _kitchenNavButton;
        
        private VisualElement _marketTab;
        private VisualElement _cookTab;
        private VisualElement _kitchenTab;

        private Button _cookButton;
        private Button _kitchenButton;
            
        [SerializeField] private UnityEvent _onMarketButtonClicked;
        [SerializeField] private UnityEvent _onCookButtonClicked;
        [SerializeField] private UnityEvent _onKitchenButtonClicked;

        private const string HiddenClassName = "hidden";
        private const string NavigationButtonDefaultClass = "navigation-button";
        private const string NavigationButtonActiveClass = "navigation-button-active";
        
        private enum EHomeMenuTabs
        {
            Market,
            Cook,
            Kitchen
        }
        
        protected override void FindVisualElements()
        {
            _marketNavButton = Root.Q<Button>("MarketNavButton");
            _cookNavButton = Root.Q<Button>("CookNavButton");
            _kitchenNavButton = Root.Q<Button>("CustomizeKitchenNavButton");

            _marketTab = Root.Q<VisualElement>("MarketTab");
            _cookTab = Root.Q<VisualElement>("CookTab");
            _kitchenTab = Root.Q<VisualElement>("KitchenTab");

            _cookButton = Root.Q<Button>("CookButton");
            _kitchenButton = Root.Q<Button>("KitchenButton");
        }

        private void Start()
        {
            ChangeTab(EHomeMenuTabs.Cook);
        }

        protected override void BindButtons()
        {
            _marketNavButton.clickable.clicked += () =>
            {
                ChangeTab(EHomeMenuTabs.Market);
            };
            _cookNavButton.clickable.clicked += () =>
            {
                ChangeTab(EHomeMenuTabs.Cook);

            };
            _kitchenNavButton.clickable.clicked += () =>
            {
                ChangeTab(EHomeMenuTabs.Kitchen);
            };

            _cookButton.clicked += () => _onCookButtonClicked?.Invoke();
            _kitchenButton.clicked += () => _onKitchenButtonClicked?.Invoke();
        }
        
        private void ChangeTab(EHomeMenuTabs _newTab)
        {
            switch (_newTab)
            {
                case EHomeMenuTabs.Market:
                    _marketTab.RemoveFromClassList(HiddenClassName);
                    _cookTab.AddToClassList(HiddenClassName);
                    _kitchenTab.AddToClassList(HiddenClassName);
                    
                    _marketNavButton.AddToClassList(NavigationButtonActiveClass);
                    _cookNavButton.RemoveFromClassList(NavigationButtonActiveClass);
                    _kitchenNavButton.RemoveFromClassList(NavigationButtonActiveClass);
                    break;
                case EHomeMenuTabs.Cook:
                    _marketTab.AddToClassList(HiddenClassName);
                    _cookTab.RemoveFromClassList(HiddenClassName);
                    _kitchenTab.AddToClassList(HiddenClassName);
                    
                    _marketNavButton.RemoveFromClassList(NavigationButtonActiveClass);
                    _cookNavButton.AddToClassList(NavigationButtonActiveClass);
                    _kitchenNavButton.RemoveFromClassList(NavigationButtonActiveClass);
                    break;
                case EHomeMenuTabs.Kitchen:
                    _marketTab.AddToClassList(HiddenClassName);
                    _cookTab.AddToClassList(HiddenClassName);
                    _kitchenTab.RemoveFromClassList(HiddenClassName);
                    
                    _marketNavButton.RemoveFromClassList(NavigationButtonActiveClass);
                    _cookNavButton.RemoveFromClassList(NavigationButtonActiveClass);
                    _kitchenNavButton.AddToClassList(NavigationButtonActiveClass);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_newTab), _newTab, null);
            }
        }
    }
}