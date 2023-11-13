using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Enums;
using ScriptableObjects.DataContainer;
using ScriptableObjects.Settings;

namespace Enums
{
    public enum ESKIN_ITEM_UI_STATE
    {
        IDLE,
        SELECTED,
        PICKED
    }
}

public class PlayerSkinItem : MonoBehaviour
{

    [SerializeField]
    private string _key;
    [SerializeField]
    private int _category;
    [SerializeField]
    private ERarityType _rarity;
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Image _skinBackground;
    [SerializeField]
    private Outline _backgroundOutline;

    [SerializeField]
    PlayerCustomizationUI _playerCustomizer;

    [SerializeField]
    private Color _pickedColor;
    [SerializeField]
    private Color _selectedColor;
    [SerializeField]
    private Color _deselectedColor;
    [SerializeField]
    private RarityColors _rarityColors;

    private ESKIN_ITEM_UI_STATE _pickState;

    [SerializeField]
    private UnityEvent<ESKIN_ITEM_UI_STATE> _onPickStateChanged;

    public void Initialize(SkinData _skin, PlayerCustomizationUI _ui)
    {
        _rarity = _skin.Rarity;
        _category = (int)_skin.SkinType;
        _key = _skin.name;
        _icon.sprite = _skin.SkinIcon;
        _playerCustomizer = _ui;
        _backgroundOutline = GetComponent<Outline>();
        _pickState = ESKIN_ITEM_UI_STATE.IDLE;

        switch (_rarity)
        {
            case ERarityType.COMMON:
                _skinBackground.color = _rarityColors.Colors[0];
                break;
            case ERarityType.UNCOMMON:
                _skinBackground.color = _rarityColors.Colors[1];
                break;
            case ERarityType.RARE:
                _skinBackground.color = _rarityColors.Colors[2];
                break;
            case ERarityType.EPIC:
                _skinBackground.color = _rarityColors.Colors[3];
                break;
            case ERarityType.LEGENDARY:
                _skinBackground.color = _rarityColors.Colors[4];
                break;
        }
    }


    public void ChangePickState(ESKIN_ITEM_UI_STATE _state)
    {
        _pickState = _state;
        _onPickStateChanged?.Invoke(_pickState);

        switch(_pickState)
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
        _playerCustomizer.SetSkin(this);
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

    public string Key { get => _key; }
    public ESKIN_ITEM_UI_STATE PickState { get => _pickState; }
    public int Category { get => _category; }
    public string Key1 { get => _key; }
}
