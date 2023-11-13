using Enums;
using ScriptableObjects.DataContainer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkinColorItem : MonoBehaviour
{
    [SerializeField]
    private SkinData _skinColorMesh;
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private SkinColorSelectionPopup _popup;
    [SerializeField]
    private Outline _backgroundOutline;
    [SerializeField]
    private Color _pickedColor;
    [SerializeField]
    private Color _selectedColor;
    [SerializeField]
    private Color _deselectedColor;

    private ESKIN_ITEM_UI_STATE _pickState;
    [SerializeField]
    private UnityEvent<ESKIN_ITEM_UI_STATE> _onPickStateChanged;


    public void ChangePickState(ESKIN_ITEM_UI_STATE _state)
    {
        _pickState = _state;
        _onPickStateChanged?.Invoke(_pickState);

        switch (_pickState)
        {
            case ESKIN_ITEM_UI_STATE.IDLE:
                Deselect();
                break;
            case ESKIN_ITEM_UI_STATE.SELECTED:
                Select();
                break;
            case ESKIN_ITEM_UI_STATE.PICKED:
                Pick();
                break;
        }
    }
    public void OnClick()
    {
        _popup.SetSkinColor(this);
    }

    void Select()
    {
        _backgroundOutline.effectColor = _selectedColor;
    }

    void Pick()
    {
        _backgroundOutline.effectColor = _pickedColor;
    }

    void Deselect()
    {
        _backgroundOutline.effectColor = _deselectedColor;
    }
    public SkinData SkinColorMesh { get => _skinColorMesh; }
    public Image Icon { get => _icon; }
}
