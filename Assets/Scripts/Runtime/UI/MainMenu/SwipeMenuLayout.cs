using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using Runtime.UI.MainMenuUI;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Runtime.UI.UIUtility
{
    public class SwipeMenuLayout : MonoBehaviour
    {
        private RectTransform _rect;
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private TabManager _tabManager;
        [SerializeField]
        private RectTransform _referenceRect;
        [SerializeField]
        private float _decelerationSpeed;
        [SerializeField]
        private List<Button> _tabButtons;

        private List<MainMenuTab> _tabTransforms;
        private float _inertia;
        private float _width;
        [SerializeField]
        bool _isDragged;
        [SerializeField]
        bool _needsAnimation;

        public UnityEvent OnSwipeMenuReady;

        private void Start()
        {
            _tabTransforms = new List<MainMenuTab>();
            _tabTransforms.Add(_tabManager.MarketTab.GetComponent<MainMenuTab>());
            _tabTransforms.Add(_tabManager.PlayerTab.GetComponent<MainMenuTab>());
            _tabTransforms.Add(_tabManager.HomeTab.GetComponent<MainMenuTab>());
            _tabTransforms.Add(_tabManager.EventTab.GetComponent<MainMenuTab>());
            _tabTransforms.Add(_tabManager.SocialTab.GetComponent<MainMenuTab>());
            _rect = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            ShowLayout();
        }

        public void ShowLayout()
        {
            _width = _referenceRect.sizeDelta.x;
            _rect.sizeDelta = new Vector2(_width * (_rect.transform.childCount - 1), _rect.sizeDelta.y);
            _rect.anchoredPosition = new Vector2(0, 0);
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(1, 0.25f).OnComplete(() => { OnSwipeMenuReady?.Invoke(); });
        }
        public void HideLayout()
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(0, 0.25f);
        }

        private void Update()
        {
            /*if(!_isDragged)
            {
                _rect.anchoredPosition += new Vector2(_inertia, 0);
                if (_inertia > 0)
                    _inertia -= Time.deltaTime * _decelerationSpeed;
                if (_inertia < 0)
                    _inertia += Time.deltaTime * _decelerationSpeed;

                if (Mathf.Abs(_inertia) <= 5 && _needsAnimation)
                {
                    _needsAnimation = false;
                    _inertia = 0;
                    ShowClosestPanel();
                }
            }*/

            float a = -((_width * _rect.transform.childCount / 2));
            float b = (_width * _rect.transform.childCount / 2);
            if (_rect.anchoredPosition.x < a || _rect.anchoredPosition.x > b)
                _inertia = 0;

            _rect.anchoredPosition = new Vector2(Mathf.Clamp(_rect.anchoredPosition.x, a, b), _rect.anchoredPosition.y);

        }

        /*void ShowClosestPanel()
        {
            MainMenuTab _tab = null;
            foreach(MainMenuTab t in _tabTransforms)
            {
                if (_tab == null)
                    _tab = t;

                RectTransform a = t.GetComponent<RectTransform>();
                RectTransform b = _tab.GetComponent<RectTransform>();
                float x = a.transform.TransformPoint(a.rect.center).x;
                float y = b.transform.TransformPoint(b.rect.center).x;
                if (Mathf.Abs(x) < Mathf.Abs(y))
                    _tab = t;

            }
            _tabButtons[_tabTransforms.IndexOf(_tab)].onClick.Invoke();
        }*/

        public void ShowSelectedTab(RectTransform _tab)
        {
            _rect.DOKill();
            Vector2 itemCenterPoint = _rect.InverseTransformPoint(_tab.transform.TransformPoint(_tab.rect.center));
            _rect.DOAnchorPos(-itemCenterPoint, 0.25f).SetEase(Ease.OutExpo);
        }

        /*public void OnPointerDown(PointerEventData eventData)
        {
            _isDragged = true;
        }*/

        /*public void OnDrag(PointerEventData eventData)
        {
            float value = eventData.delta.x / _canvas.scaleFactor;
            _inertia = value;
            _rect.anchoredPosition += new Vector2(value, 0);
        }*/

        /*public void OnPointerUp(PointerEventData eventData)
        {
            _isDragged = false;
            _needsAnimation = true;
        }*/
    }
}
