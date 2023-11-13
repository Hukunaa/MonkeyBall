using UnityEngine;
using UnityEngine.Events;

namespace TutorialSystem.Scripts.Runtime
{
    public class TutorialFaderMask : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onMaskFader;
        [SerializeField] private UnityEvent _onUnmaskFader;
        
        public void MaskFader()
        {
            _onMaskFader?.Invoke();
        }

        public void UnmaskFader()
        {
            _onUnmaskFader?.Invoke();
        }
    }
}