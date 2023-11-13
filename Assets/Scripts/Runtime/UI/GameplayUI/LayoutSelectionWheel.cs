using System.Collections;
using System.Linq;
using GeneralScriptableObjects;
using ScriptableObjects.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace UI.GameplayUI
{
    public class LayoutSelectionWheel : MonoBehaviour
    {
        [SerializeField] 
        private Image _layoutImage;

        [SerializeField]
        private Image _layoutImageLeft;

        [SerializeField]
        private Image _layoutImageRight;

        [SerializeField] 
        private TMP_Text _layoutName;

        [SerializeField] 
        private float _wheelScrollingSpeed = .1f;

        [SerializeField] 
        private float _wheelDuration = 3;
        
        [SerializeField]
        private Image _blockCentre;

        [SerializeField]
        private Image _blockLayout;

        [SerializeField]
        private Image _blockTop;

        [SerializeField]
        private Image _blockBot;

        [SerializeField]
        private Image _blockLabel;

        [SerializeField]
        private Color _selectedColor;

        [SerializeField]
        private float _fillDuration;

        [SerializeField]
        private float _waitDuration;
       
        [SerializeField] 
        private bool _skip;
        
        [SerializeField] 
        private UnityEvent _onSelectionWheelStart;
        
        [SerializeField] 
        private UnityEvent _onWheelSelectionComplete;

        public void StartSelectionWheel(LayoutSO[] _availableLayouts, LayoutSO _selectedLayout)
        {
            if (_skip)
            {
                _onWheelSelectionComplete?.Invoke();
                return;
            }
            _blockLayout.color = Color.white;
            _layoutImageLeft.DOFade(1f, 0.2f);
            _layoutImageRight.DOFade(1f, 0.2f);
            _onSelectionWheelStart?.Invoke();
            StartCoroutine(TransitionInCoroutine());
            StartCoroutine(SelectionWheelCoroutine(_availableLayouts, _selectedLayout));
        }

        private IEnumerator SelectionWheelCoroutine(LayoutSO[] _availableLayouts, LayoutSO _selectedLayout)
        {
            float _currentWheelTime = 0;
            LayoutSO currentLayout = GetRandomLayout(_availableLayouts);

            while (_currentWheelTime < _wheelDuration)
            {
                _layoutImage.sprite = currentLayout.LayoutSprite;
                _layoutImageLeft.sprite = GetRandomLayout(_availableLayouts).LayoutSprite;
                _layoutImageRight.sprite = GetRandomLayout(_availableLayouts).LayoutSprite;

                yield return new WaitForSeconds(_wheelScrollingSpeed);
                _currentWheelTime += _wheelScrollingSpeed;
                currentLayout = GetRandomLayout(_availableLayouts.Where(x => x != currentLayout).ToArray());
            }
            
            _layoutImage.sprite = _selectedLayout.LayoutSprite;        
            _layoutName.SetText(_selectedLayout.LayoutName);
            _blockLabel.fillOrigin = (int)Image.OriginHorizontal.Left;
            _blockLabel.DOFillAmount(1f, _fillDuration);
            _blockLayout.color = _selectedColor;
            _layoutImageLeft.DOFade(0f,0.5f);
            _layoutImageRight.DOFade(0f, 0.5f);
            StartCoroutine(TransitionOutCoroutine());
        }
        private IEnumerator TransitionInCoroutine()
        {
            yield return new WaitForSeconds(_fillDuration);
            _blockCentre.fillOrigin = (int)Image.OriginHorizontal.Left;
            _blockCentre.DOFillAmount(1f, _fillDuration);

            yield return new WaitForSeconds(_fillDuration);
            _blockLayout.fillOrigin = (int)Image.OriginHorizontal.Right;
            _blockLayout.DOFillAmount(1f, _fillDuration);
            _blockTop.fillOrigin = (int)Image.OriginHorizontal.Right;
            _blockTop.DOFillAmount(1f, _fillDuration);
            _blockBot.fillOrigin = (int)Image.OriginHorizontal.Right;
            _blockBot.DOFillAmount(1f, _fillDuration);
        }

        private IEnumerator TransitionOutCoroutine()
        {
            yield return new WaitForSeconds(_waitDuration);
            _blockCentre.fillOrigin = (int)Image.OriginHorizontal.Right;
            _blockCentre.DOFillAmount(0f, _fillDuration);
            _blockLayout.fillOrigin = (int)Image.OriginHorizontal.Left;
            _blockLayout.DOFillAmount(0f, _fillDuration);
            _blockTop.fillOrigin = (int)Image.OriginHorizontal.Left;
            _blockTop.DOFillAmount(0f, _fillDuration);
            _blockBot.fillOrigin = (int)Image.OriginHorizontal.Left;
            _blockBot.DOFillAmount(0f, _fillDuration);
            _blockLabel.fillOrigin = (int)Image.OriginHorizontal.Right;
            _blockLabel.DOFillAmount(0f, _fillDuration);

            yield return new WaitForSeconds(_fillDuration);
            _onWheelSelectionComplete?.Invoke();
        }
        private LayoutSO GetRandomLayout(LayoutSO[] _availableLayouts)
        {
            if (_availableLayouts.Length == 1)
            {
                return _availableLayouts[0];
            }
            
            var randomIndex = Random.Range(0, _availableLayouts.Length);
            return _availableLayouts[randomIndex];
        }
    }
}