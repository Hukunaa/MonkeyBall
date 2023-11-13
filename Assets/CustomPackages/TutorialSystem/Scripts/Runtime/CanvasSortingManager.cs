using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace TutorialSystem.Scripts.Runtime
{
    public class CanvasSortingManager : MonoBehaviour
    {
        private Canvas _canvas;
        
        [SerializeField]
        private bool _defaultOverrideSorting = false;
        
        [SerializeField]
        private int _defaultSortingOrder = 0;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();

            if (_canvas == null)
            {
                CreateCanvas();
            }
        }

        public void OverrideSorting(int _sortingOrder)
        {
            _canvas.overrideSorting = true;
            _canvas.sortingOrder = _sortingOrder;
        }

        public void StopOverride()
        {
            _canvas.overrideSorting = _defaultOverrideSorting;
            _canvas.sortingOrder = _defaultSortingOrder;
        }

        private void CreateCanvas()
        {
            _canvas = transform.AddComponent<Canvas>();
            if (transform.GetComponent<GraphicRaycaster>() != null) return;
            transform.AddComponent<GraphicRaycaster>();
        }
    }
}