using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TutorialSystem.Scripts.Runtime
{
    public class ClickBlocker : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] 
        private UnityEvent _onBlockerClicked;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _onBlockerClicked?.Invoke();
        }
    }
}