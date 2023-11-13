using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using DG.Tweening;

public class SkinColorSelectionPopup : MonoBehaviour
{
    [SerializeField]
    private Button _exitButton;
    [SerializeField]
    private Image _buttonIcon;

    private PlayerSkinsLoader _skinLoader;
    private List<string> _playerCurrentSkins;

    private void Start()
    {
        _skinLoader = FindObjectOfType<PlayerSkinsLoader>();
        FilterSelection();
        ExitPopup();
    }
    public void ShowPopup()
    {
        gameObject.SetActive(true);
        transform.DOKill();
        transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        _exitButton.gameObject.SetActive(true);
    }

    public void ExitPopup()
    {
        transform.DOKill();
        _exitButton.gameObject.SetActive(false);
        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void SetSkinColor(SkinColorItem _item)
    {
        _skinLoader.LoadPreviewSkin(new KeyValuePair<string, int>(_item.SkinColorMesh.name, 5));
        _playerCurrentSkins = DataLoader.LoadPlayerCurrentSkins();
        _playerCurrentSkins[5] = _item.SkinColorMesh.name;
        DataLoader.SavePlayerCurrentSkins(_playerCurrentSkins);
        FilterSelection();
    }

    void FilterSelection()
    {
        string key = DataLoader.LoadPlayerCurrentSkins()[5];
        List<SkinColorItem> _items = GetComponentsInChildren<SkinColorItem>().ToList();
        foreach (SkinColorItem _item in _items)
        {
            if (_item.SkinColorMesh.name == key)
            {
               // _buttonIcon.sprite = _item.Icon.sprite;
                _buttonIcon.color = _item.Icon.color;
                _item.ChangePickState(Enums.ESKIN_ITEM_UI_STATE.PICKED);
            }
            else
                _item.ChangePickState(Enums.ESKIN_ITEM_UI_STATE.IDLE);

        }
    }
}
