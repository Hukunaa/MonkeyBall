using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public class TabsButtonsHandler : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _navButtons;
    List<bool> _navIsUp;

    [SerializeField]
    private Color32 _baseImage;
    [SerializeField]
    private Color32 _selectedImage;

    private Vector3 _anchorPos;

    [SerializeField]
    private float _slideOffset;
    [SerializeField]
    private float _slideDuration;

    private void Start()
    {
        if(_navButtons.Count < 5)
        {
            Debug.LogError("Tabs buttons are missing!");
        }
        _anchorPos = _navButtons[0].GetComponent<RectTransform>().anchoredPosition;

        _navIsUp = Enumerable.Repeat(false, _navButtons.Count).ToList();

        OnSelectButton(_navButtons[2]);
    }

    public void OnSelectButton(GameObject _button)
    {
        _navButtons.Where(p => p != _button).ToList().ForEach(p => p.GetComponent<Button>().targetGraphic.color = _baseImage);
        for(int i = 0; i < _navButtons.Count; ++i)
        {
            if(_navIsUp[i] == true)
            {
                RectTransform r = _navButtons[i].GetComponent<RectTransform>();
                r.DOAnchorPosY(_anchorPos.y, _slideDuration);
                _navIsUp[i] = false;
            }
        }
        _button.GetComponent<Button>().targetGraphic.color = _selectedImage;
        RectTransform p = _button.GetComponent<RectTransform>();
        p.DOAnchorPosY(_anchorPos.y + _slideOffset, _slideDuration);
        _navIsUp[_navButtons.IndexOf(_button)] = true;
    }
}
