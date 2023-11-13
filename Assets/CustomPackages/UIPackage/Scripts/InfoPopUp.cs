using UnityEngine;

namespace UIPackage.Scripts
{
    [RequireComponent(typeof(Canvas))][RequireComponent(typeof(CanvasGroup))]
    public class InfoPopUp : MonoBehaviour
    {
        [Header("Components ref")]
        [SerializeField]
        private Canvas _popUpCanvas;
        
        [SerializeField] 
        private CanvasGroup _canvasGroup;

        protected virtual void Awake()
        {
            _popUpCanvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            HidePopUp();
        }

        protected virtual void ShowPopUp()
        {
            _popUpCanvas.enabled = true;
            _canvasGroup.interactable = true;
        }

        public virtual void HidePopUp()
        {
            _popUpCanvas.enabled = false;
            _canvasGroup.interactable = false;
        }
    }
}