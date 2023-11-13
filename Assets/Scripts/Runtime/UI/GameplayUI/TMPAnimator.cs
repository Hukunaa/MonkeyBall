using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.GameplayUI
{
    public class TMPAnimator : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _tmpt;
        
        public IEnumerator ShowTextForDuration(string _text, float _duration)
        {
            _tmpt.text = _text;
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);

            Sequence _textSequence = DOTween.Sequence();
            _textSequence.Append(transform.DOScale(1, .5f)).AppendInterval(_duration)
                .Append(transform.DOScale(0, .5f));
            yield return _textSequence.WaitForCompletion();
            
            gameObject.SetActive(false);
        }
    }
}