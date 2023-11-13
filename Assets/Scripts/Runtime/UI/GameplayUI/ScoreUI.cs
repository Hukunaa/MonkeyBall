using System.Collections;
using DG.Tweening;
using Gameplay.Character;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI.GameplayUI
{
    public class ScoreUI : MonoBehaviour
    {
        private PlayerScore _playerScore;

        [SerializeField] 
        private RectTransform _distanceParent;

        [SerializeField] 
        private RectTransform _scoreParent;
        
        [SerializeField] 
        private TMP_Text _distanceText;

        [SerializeField] 
        private TMP_Text _multiplierText;

        [SerializeField] 
        private TMP_Text _finalScoreText;

        [SerializeField] 
        private float _displayDistanceDelay;
        
        [SerializeField] 
        private float _displayPointsDelay;

        [SerializeField] 
        private float _displayMultiplierDelay;

        [SerializeField] 
        private float _endDelay;
        
        [SerializeField] 
        private float _multiplierEffectDuration;

        [SerializeField] 
        private float _parentTranslationDuration;
        
        [SerializeField] 
        private Vector2 _distanceParentStartPos;
        
        [SerializeField] 
        private Vector2 _scoreParentStartPos;

        [SerializeField] 
        private UnityEvent _onPlayScoreAnimation;
        
        [SerializeField] 
        private UnityEvent _onScoreAnimationFinished;
        
        public void SetTarget(Transform transform)
        {
            _playerScore = transform.GetComponent<PlayerScore>();
            _playerScore.onScoreCalculated += PlayerScore_OnScoreCalculated;
        }

        [ContextMenu("Test")]
        private void Test()
        {
            PlayerScore_OnScoreCalculated(250, 250, 5, 1250);
        }

        private void PlayerScore_OnScoreCalculated(int _distance, int _basePoints, int _multiplier, int _finalScore)
        {
            _distanceParent.anchoredPosition = _distanceParentStartPos;
            _scoreParent.anchoredPosition = _scoreParentStartPos;
            _multiplierText.enabled = false;

            StartCoroutine(ScoreDisplayCoroutine(_distance, _basePoints, _multiplier, _finalScore));
        }

        private IEnumerator ScoreDisplayCoroutine(int _distance, int _basePoints, int _multiplier, int _finalScore)
        {
            _onPlayScoreAnimation?.Invoke();
            yield return new WaitForSeconds(_displayDistanceDelay);
            _distanceText.SetText(_distance.ToString());
            _distanceParent.DOAnchorPosX(0, _parentTranslationDuration, true);
            yield return new WaitForSeconds(_displayPointsDelay);
            _finalScoreText.SetText(_basePoints.ToString());
            _scoreParent.DOAnchorPosX(0, _parentTranslationDuration, true);
            yield return new WaitForSeconds(_displayMultiplierDelay);
            
            if (_multiplier > 1)
            {
                _multiplierText.SetText($"{_multiplier.ToString()}X");
                _multiplierText.rectTransform.localScale = Vector3.zero;
                _multiplierText.enabled = true;
                yield return _multiplierText.rectTransform.DOScale(Vector3.one, .5f).WaitForCompletion();
                yield return StartCoroutine(MultiplierEffect(_basePoints, _finalScore));
            }
            
            yield return new WaitForSeconds(_endDelay);
            _onScoreAnimationFinished?.Invoke();
        }

        private IEnumerator MultiplierEffect(int _startScore, int _endScore)
        {
            float currentTime = 0;
            while (currentTime < _multiplierEffectDuration)
            {
                currentTime += Time.deltaTime;
                var t = Mathf.Clamp01(currentTime / _multiplierEffectDuration);
                var interpolatedScore = Mathf.RoundToInt(Mathf.Lerp(_startScore, _endScore, t));
                _finalScoreText.SetText(interpolatedScore.ToString());
                yield return null;
            }
        }
    }
}
