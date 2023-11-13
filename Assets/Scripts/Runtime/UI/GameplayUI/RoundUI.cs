using GeneralScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

namespace UI.GameplayUI
{
    public class RoundUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roundText;

        [SerializeField] private IntVariable _roundCount;

        [SerializeField] private Image _blockCentre;

        [SerializeField] private Image _blockCentreMask;

        [SerializeField] private Image _blockTop;

        [SerializeField] private Image _blockBot;

        [SerializeField] private float _fillDuration;

        [SerializeField] private float _waitDuration;

        [SerializeField] private CanvasEnableSetter _canvas;

        public void UpdateRoundText()
        {
            _roundText.text = $"Round {_roundCount.Value}";
            StartCoroutine(TransitionCoroutine());
        }

        private IEnumerator TransitionCoroutine()
        {
            yield return new WaitForSeconds(_fillDuration);
            _blockCentre.fillOrigin = (int)Image.OriginHorizontal.Left;
            _blockCentre.DOFillAmount(1f, _fillDuration);

            yield return new WaitForSeconds(_fillDuration);
            _blockCentreMask.fillOrigin = (int)Image.OriginHorizontal.Right;
            _blockCentreMask.DOFillAmount(1f, _fillDuration);
            _blockTop.fillOrigin = (int)Image.OriginHorizontal.Right;
            _blockTop.DOFillAmount(1f, _fillDuration);
            _blockBot.fillOrigin = (int)Image.OriginHorizontal.Right;
            _blockBot.DOFillAmount(1f, _fillDuration);

            yield return new WaitForSeconds(_waitDuration);
            _blockCentre.fillOrigin = (int)Image.OriginHorizontal.Right;
            _blockCentre.DOFillAmount(0f, _fillDuration);
            _blockCentreMask.fillOrigin = (int)Image.OriginHorizontal.Right;
            _blockCentreMask.DOFillAmount(0f, _fillDuration);
            _blockTop.fillOrigin = (int)Image.OriginHorizontal.Left;
            _blockTop.DOFillAmount(0f, _fillDuration);
            _blockBot.fillOrigin = (int)Image.OriginHorizontal.Left;
            _blockBot.DOFillAmount(0f, _fillDuration);

            yield return new WaitForSeconds(_fillDuration);
            _canvas.ChangeVisibility(false);
        }
    }
}