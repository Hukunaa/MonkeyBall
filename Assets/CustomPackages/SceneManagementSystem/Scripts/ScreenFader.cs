using System.Collections;
using DG.Tweening;
using GeneralScriptableObjects.EventChannels;
using UnityEngine;

namespace SceneManagementSystem.Scripts
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] private FadeChannel _fadeChannel;
		
        [SerializeField] private CanvasGroup _faderCanvasGroup;

        [SerializeField] private GameObject _loadingEffect;
        
        private void OnEnable()
        {
            _fadeChannel.onEventRaised += InitiateFade;
        }

        private void OnDisable()
        {
            _fadeChannel.onEventRaised -= InitiateFade;
        }

        private IEnumerator Fade(bool _fadeIn, float _fadeDuration)
        {
            var finalAlpha = _fadeIn ? 0 : 1;
            
            if (_fadeIn)
            {
                _loadingEffect.SetActive(false);
            }
            else
            {
                _faderCanvasGroup.blocksRaycasts = true;
            }

            _faderCanvasGroup.blocksRaycasts = true;
            yield return _faderCanvasGroup.DOFade(finalAlpha, _fadeDuration);

            if (!_fadeIn)
            {
                _loadingEffect.SetActive(true);
            }
            else
            {
                _faderCanvasGroup.blocksRaycasts = false;
            }
        }
        
        private void InitiateFade(bool _fadeIn, float _duration)
        {
            StartCoroutine(Fade(_fadeIn, _duration));
        }
    }
}