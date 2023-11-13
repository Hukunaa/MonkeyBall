using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ForceUpdateChildWidth : MonoBehaviour
{
    private RectTransform _rectTransform, _parentRectTransform;
    private VerticalLayoutGroup verticalLayoutGroup;

    void OnEnable()
    {
        UpdateWidth();
    }

    private void OnRectTransformDimensionsChange()
    {
        UpdateWidth();
    }



    private void UpdateWidth()
    {
        if (verticalLayoutGroup == null || _rectTransform == null || _parentRectTransform == null)
        {
            verticalLayoutGroup = GetComponentInParent<VerticalLayoutGroup>();
            if (verticalLayoutGroup != null)
            {
                _parentRectTransform = verticalLayoutGroup.GetComponent<RectTransform>();
                _rectTransform = GetComponent<RectTransform>();
                _rectTransform.pivot = new Vector2(0, 1);
                //_rectTransform.sizeDelta = new Vector2(_parentRectTransform.rect.size.x - (verticalLayoutGroup.padding.left + verticalLayoutGroup.padding.right), _rectTransform.sizeDelta.y);
                //_rectTransform.rect.Set(_rectTransform.rect.x, _rectTransform.rect.y, _parentRectTransform.rect.width, _rectTransform.rect.height);
                _rectTransform.offsetMin = new Vector2(0, 0);
                _rectTransform.offsetMax = new Vector2(0, 0);
            }
        }
        else
        {
            _rectTransform.offsetMin = new Vector2(0, 0);
            _rectTransform.offsetMax = new Vector2(0, 0);
            //_rectTransform.rect.Set(_rectTransform.rect.x, _rectTransform.rect.y, _parentRectTransform.rect.width, _rectTransform.rect.height);
            //_rectTransform.sizeDelta = new Vector2(_parentRectTransform.rect.size.x - (verticalLayoutGroup.padding.left + verticalLayoutGroup.padding.right), _rectTransform.sizeDelta.y);
        }
    }
}
