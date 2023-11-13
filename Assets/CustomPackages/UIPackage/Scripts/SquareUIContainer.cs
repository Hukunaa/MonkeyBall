using System;
using UnityEngine;

namespace UIPackage.Scripts
{
    [RequireComponent(typeof(RectTransform))]
    public class SquareUIContainer : MonoBehaviour
    {
        [SerializeField] 
        private EMatchOrientation _orientationToMatch;

        [SerializeField]
        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        private void Start()
        {
            UpdateContainer();
        }

        [ContextMenu("UpdateRectTransformSize")]
        public void UpdateContainer()
        {
            switch (_orientationToMatch)
            {
                case EMatchOrientation.Width:
                   _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _rect.rect.width);
                    break;
                case EMatchOrientation.Height:
                    _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _rect.rect.height);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private enum EMatchOrientation
        {
            Width,
            Height
        }
    }
}