using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.UI.UIUtility
{
    [ExecuteInEditMode]
    public class ForceExpandToScreenSize : MonoBehaviour
    {
        private RectTransform _rect;
        [SerializeField]
        private Canvas _canvas;

        private void Update()
        {
            if (_rect == null)
                _rect = GetComponent<RectTransform>();

            if (_canvas != null)
                _rect.sizeDelta = new Vector2(_canvas.GetComponent<RectTransform>().sizeDelta.x, _rect.sizeDelta.y);
        }
    }
}
