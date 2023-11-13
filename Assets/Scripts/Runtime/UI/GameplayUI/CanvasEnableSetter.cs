using GeneralScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.Events;

namespace UI.GameplayUI
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasEnableSetter : MonoBehaviour
    {
        [SerializeField] 
        private bool _startEnable;
        
        [SerializeField] 
        private BoolEventChannel _onChangeCanvasEnableEventChannel;

        [SerializeField] 
        private UnityEvent _onCanvasEnabled;
        
        [SerializeField] 
        private UnityEvent _onCanvasDisabled;
        private Canvas _canvas;
        
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            if (_onChangeCanvasEnableEventChannel == null) return;
            _onChangeCanvasEnableEventChannel.onEventRaised += ChangeVisibility;
        }

        private void Start()
        {
            ChangeVisibility(_startEnable);
        }

        private void OnDestroy()
        {
            if (_onChangeCanvasEnableEventChannel == null) return;
            _onChangeCanvasEnableEventChannel.onEventRaised -= ChangeVisibility;
        }

        public void ChangeVisibility(bool _visible)
        {
            _canvas.enabled = _visible;
            
            if (_visible)
            {
                Debug.Log($"{gameObject.name} visible");
                _onCanvasEnabled?.Invoke();
            }

            else
            {
                Debug.Log($"{gameObject.name} hidden");
                _onCanvasDisabled?.Invoke();
            }
        }
    }
}
