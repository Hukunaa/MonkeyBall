using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Runtime.UI.UIUtility;
using DG.Tweening;
using Enums;
using Gameplay.Player;
using UnityEngine.UI;
using ScriptableObjects.DataContainer;
using SceneManagementSystem.Scripts;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utilities;
using TMPro;
using UI.GameplayUI;
using System.Threading.Tasks;

public class PlayerCustomizationUI : MonoBehaviour
{
    [SerializeField]
    private AnimateElementToScreen _canvasHeader;
    [SerializeField]
    private RectTransform _canvas;
    [SerializeField]
    private RectTransform _canvasContent;
    [SerializeField]
    private bool _isOpen;
    [SerializeField]
    private Button _exitButton;
    [SerializeField]
    private Button _equipButton;
    [SerializeField]
    private RectTransform _player3D;
    [SerializeField]
    private PlayerSkinsLoader _skinLoader;
    [SerializeField]
    private PlayerSkinHandler _skinHandler;
    [SerializeField]
    private AssetReference _itemAssetReference;
    [SerializeField]
    private PreviewPlayerCamera _playerPreviewCam;
    [SerializeField]
    private PlayerAnimator _playerAnimator;
    [SerializeField]
    private Transform _playerBallParent;
    [SerializeField]
    private Transform _playerGliderParent;
    [SerializeField]
    private float _playerBallPreviewRotationSpeed;
    [SerializeField]
    private Color _equipButtonTextColor;
    [SerializeField]
    private Color _equipButtonTextColorDisabled;

    private GameObject _itemPrefabGO;
    private AsyncOperationHandle<GameObject> _itemPrefabHandle;

    private Vector2 _canvasStartPos;

    private List<PlayerSkinItem> _headItems;
    private List<PlayerSkinItem> _bodyItems;
    private List<PlayerSkinItem> _gliderItems;
    private List<PlayerSkinItem> _ballItems;

    private List<string> _playerCurrentSkins;
    private KeyValuePair<string, int> _tmpSkin;

    private int _categoryShown;
    private KeyValuePair<PlayerSkinItem, ESKIN_ITEM_UI_STATE> _tmpItemSelected;

    private void Start()
    {
        _categoryShown = 0;
        _headItems = new List<PlayerSkinItem>();
        _bodyItems = new List<PlayerSkinItem>();
        _gliderItems = new List<PlayerSkinItem>();
        _ballItems = new List<PlayerSkinItem>();
        _canvasStartPos = _canvas.anchoredPosition;
        _canvas.anchoredPosition -= new Vector2(0, 1000);

        if (_skinLoader == null)
            _skinLoader = FindObjectOfType<PlayerSkinsLoader>();

        if (_skinHandler == null)
            _skinHandler = FindObjectOfType<PlayerSkinHandler>();

        if (_playerAnimator == null)
            _playerAnimator = FindObjectOfType<PlayerAnimator>();

        if (_playerPreviewCam == null)
            _playerPreviewCam = FindObjectOfType<PreviewPlayerCamera>();

        _skinHandler.OnBallSkinUpdated += SetBallTextureLayer;
        _skinHandler.OnGliderSkinUpdated += SetGliderTextureLayer;
        DisableEquipButton();
        StartCoroutine(WaitForData());
    }

    private void Update()
    {
        _playerBallParent.Rotate(new Vector3(_playerBallPreviewRotationSpeed, _playerBallPreviewRotationSpeed, _playerBallPreviewRotationSpeed) * Time.deltaTime);
    }
    private IEnumerator WaitForData()
    {
        while (GameManager.Instance == null || GameManager.Instance.DataLoaded == false)
        {
            yield return null;
        }

        GameManager.Instance.PlayerDataContainer.PlayerSkinsInventory._onSkinAdded += CreateSkinItem;
        GameManager.Instance.PlayerDataContainer.PlayerSkinsInventory._onSkinRemoved += DestroySkinItem;
        
        LoadAvailableSkins();
    }

    private void OnDestroy()
    {
        Addressables.Release(_itemPrefabHandle);
        _skinHandler.OnBallSkinUpdated -= SetBallTextureLayer;
        _skinHandler.OnGliderSkinUpdated -= SetGliderTextureLayer;
        GameManager.Instance.PlayerDataContainer.PlayerSkinsInventory._onSkinAdded -= CreateSkinItem;
        GameManager.Instance.PlayerDataContainer.PlayerSkinsInventory._onSkinRemoved -= DestroySkinItem;
    }

    public void ToggleUI(bool _value)
    {
        if (_isOpen != _value)
        {
            if (_value)
                ShowUI();
            else
                HideUI();
        }

        _isOpen = _value;
    }

    public void ToggleUI()
    {
        if (!_isOpen)
            ShowUI();
        else
            HideUI();

        _isOpen = !_isOpen;
    }

    void ShowUI()
    {
        _canvasHeader.AnimateElement(0.25f, 0, Ease.InBack);
        _canvas.DOKill();
        _canvas.DOAnchorPos(_canvasStartPos, 0.25f).SetEase(Ease.OutBack);
        _player3D.DOKill();
        _player3D.DOAnchorPos(new Vector2(_player3D.anchoredPosition.x, 251), 0.25f).SetEase(Ease.OutBack);
        _exitButton.gameObject.SetActive(true);
        FilterItemSelection();
        ShowCategory();
    }

    void HideUI()
    {
        _canvasHeader.AnimateElement(0.25f, 0, Ease.OutBack);
        _canvas.DOKill();
        _canvas.DOAnchorPos(_canvasStartPos - new Vector2(0, 1000), 0.25f).SetEase(Ease.InBack);
        _player3D.DOKill();
        _player3D.DOAnchorPos(new Vector2(_player3D.anchoredPosition.x, 0), 0.25f).SetEase(Ease.OutBack);
        _exitButton.gameObject.SetActive(false);
        _playerPreviewCam.MoveToBody();
        _playerAnimator.CloseBag();
        _playerAnimator.ChangeAnimationState(GeneralScriptableObjects.EventChannels.ANIMATION_STATE.IDLE);
        UpdatePlayerSkinVisuals();
    }
    private void SetLayerByRecursion(GameObject _go, int _layer)
    {
        _go.layer = _layer;
        foreach (Transform child in _go.transform)
        {
            child.gameObject.layer = _layer;

            Transform _hasChildren = child.GetComponentInChildren<Transform>();
            if (_hasChildren != null)
                SetLayerByRecursion(child.gameObject, _layer);

        }
    }

    void SetBallTextureLayer()
    {
        SetLayerByRecursion(_playerBallParent.gameObject, LayerMask.NameToLayer("RenderTexture"));
    }

    void SetGliderTextureLayer()
    {
        SetLayerByRecursion(_playerGliderParent.gameObject, LayerMask.NameToLayer("RenderTexture"));
    }

    void FilterItemSelection()
    {
        _playerCurrentSkins = DataLoader.LoadPlayerCurrentSkins();
        for(int i = 0; i < _headItems.Count; ++i)
        {
            if (_playerCurrentSkins.Any(o => o == _headItems[i].Key))
                _headItems[i].ChangePickState(ESKIN_ITEM_UI_STATE.PICKED);
            else
                _headItems[i].ChangePickState(ESKIN_ITEM_UI_STATE.IDLE);
        }

        for (int i = 0; i < _bodyItems.Count; ++i)
        {
            if (_playerCurrentSkins.Any(o => o == _bodyItems[i].Key))
                _bodyItems[i].ChangePickState(ESKIN_ITEM_UI_STATE.PICKED);
            else
                _bodyItems[i].ChangePickState(ESKIN_ITEM_UI_STATE.IDLE);
        }

        for (int i = 0; i < _ballItems.Count; ++i)
        {
            if (_playerCurrentSkins.Any(o => o == _ballItems[i].Key))
                _ballItems[i].ChangePickState(ESKIN_ITEM_UI_STATE.PICKED);
            else
                _ballItems[i].ChangePickState(ESKIN_ITEM_UI_STATE.IDLE);
        }

        for (int i = 0; i < _gliderItems.Count; ++i)
        {
            if (_playerCurrentSkins.Any(o => o == _gliderItems[i].Key))
                _gliderItems[i].ChangePickState(ESKIN_ITEM_UI_STATE.PICKED);
            else
                _gliderItems[i].ChangePickState(ESKIN_ITEM_UI_STATE.IDLE);
        }
    }

    public void SetCategoryShown(int _id)
    {
        _categoryShown = _id;
        ShowCategory();
    }

    void ShowCategory()
    {
        switch(_categoryShown)
        {
            case 0:
                _headItems.ForEach(p => p.gameObject.SetActive(true));
                _bodyItems.ForEach(p => p.gameObject.SetActive(false));
                _ballItems.ForEach(p => p.gameObject.SetActive(false));
                _gliderItems.ForEach(p => p.gameObject.SetActive(false));
                _playerPreviewCam.MoveToHead();
                _playerAnimator.ChangeAnimationState(GeneralScriptableObjects.EventChannels.ANIMATION_STATE.IDLE);
                _playerAnimator.CloseBag();
                break;
            case 1:
                _bodyItems.ForEach(p => p.gameObject.SetActive(true));
                _headItems.ForEach(p => p.gameObject.SetActive(false));
                _ballItems.ForEach(p => p.gameObject.SetActive(false));
                _gliderItems.ForEach(p => p.gameObject.SetActive(false));
                _playerPreviewCam.MoveToBody();
                _playerAnimator.ChangeAnimationState(GeneralScriptableObjects.EventChannels.ANIMATION_STATE.IDLE);
                _playerAnimator.CloseBag();
                break;
            case 2:
                _ballItems.ForEach(p => p.gameObject.SetActive(true));
                _bodyItems.ForEach(p => p.gameObject.SetActive(false));
                _headItems.ForEach(p => p.gameObject.SetActive(false));
                _gliderItems.ForEach(p => p.gameObject.SetActive(false));
                _playerPreviewCam.MoveToBall();
                _playerAnimator.ChangeAnimationState(GeneralScriptableObjects.EventChannels.ANIMATION_STATE.ROLLING);
                _playerAnimator.CloseBag();
                break;
            case 3:
                _gliderItems.ForEach(p => p.gameObject.SetActive(true));
                _ballItems.ForEach(p => p.gameObject.SetActive(false));
                _bodyItems.ForEach(p => p.gameObject.SetActive(false));
                _headItems.ForEach(p => p.gameObject.SetActive(false));
                _playerPreviewCam.MoveToGlider();
                _playerAnimator.ChangeAnimationState(GeneralScriptableObjects.EventChannels.ANIMATION_STATE.IDLE);
                _playerAnimator.OpenBag();
                break;
        }
    }
    async void LoadAvailableSkins()
    {
        if (!_itemPrefabHandle.IsValid())
        {
            _itemPrefabHandle = _itemAssetReference.LoadAssetAsync<GameObject>();
            await _itemPrefabHandle.Task;
            _itemPrefabGO = _itemPrefabHandle.Result;
        }
        
        List<SkinData> headSkins = GameManager.Instance.PlayerDataContainer.PlayerSkinsInventory.HeadSkins;
        List<SkinData> bodySkins = GameManager.Instance.PlayerDataContainer.PlayerSkinsInventory.BodySkins;
        List<SkinData> ballSkins = GameManager.Instance.PlayerDataContainer.PlayerSkinsInventory.BallSkins;
        List<SkinData> gliderSkins = GameManager.Instance.PlayerDataContainer.PlayerSkinsInventory.GliderSkins;

        foreach(SkinData headSkin in headSkins)
        {
            CreateSkinItem(headSkin);
            
        }
        foreach (SkinData bodySkin in bodySkins)
        {
           CreateSkinItem(bodySkin);
        }

        foreach (SkinData ballSkin in ballSkins)
        {
            CreateSkinItem(ballSkin);
        }

        foreach (SkinData gliderSkin in gliderSkins)
        {
            CreateSkinItem(gliderSkin);
        }
    }
    

    private void CreateSkinItem(SkinData _skin)
    {
        PlayerSkinItem _instance = Instantiate(_itemPrefabGO, _canvasContent).GetComponent<PlayerSkinItem>();
        _instance.Initialize(_skin, this);
        switch (_skin.SkinType)
        {
            case ESkinType.HEAD:
                _headItems.Add(_instance);
                break;
            case ESkinType.BODY:
                _bodyItems.Add(_instance);
                break;
            case ESkinType.BALL:
                _ballItems.Add(_instance);
                break;
            case ESkinType.GLIDER:
                _gliderItems.Add(_instance);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void DestroySkinItem(SkinData _skin)
    {
        var item = FindPlayerSkinItem(_skin);
        
        if (item == null)
        {
            Debug.Log($"Cannot find PlayerSkinItem of name {_skin.SkinName}. Cannot destroy it.");
            return;
        }

        switch (_skin.SkinType)
        {
            case ESkinType.HEAD:
                _headItems.Remove(item);
                break;
            case ESkinType.BODY:
                _bodyItems.Remove(item);
                break;
            case ESkinType.BALL:
                _ballItems.Remove(item);
                break;
            case ESkinType.GLIDER:
                _gliderItems.Remove(item);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Destroy(item.gameObject);
    }

    private PlayerSkinItem FindPlayerSkinItem(SkinData _skin)
    {
        PlayerSkinItem output;
        switch (_skin.SkinType)
        {
            case ESkinType.HEAD:
                output = _headItems.FirstOrDefault(x => x.name == _skin.SkinName);
                break;
            case ESkinType.BODY:
                output = _bodyItems.FirstOrDefault(x => x.name == _skin.SkinName);
                break;
            case ESkinType.BALL:
                output = _ballItems.FirstOrDefault(x => x.name == _skin.SkinName);
                break;
            case ESkinType.GLIDER:
                output = _gliderItems.FirstOrDefault(x => x.name == _skin.SkinName);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        if (output == null)
        {
            Debug.Log($"Cannot find PlayerSkinItem of name {_skin.SkinName}. Cannot destroy it.");
        }

        return output;
    }

    void EnableEquipButton()
    {
        _equipButton.interactable = true;
        TMP_Text _text = _equipButton.GetComponentInChildren<TMP_Text>();
        _text.color = _equipButtonTextColor;
        _text.text = "Equip";
    }

    void DisableEquipButton()
    {
        _equipButton.interactable = false;
        TMP_Text _text = _equipButton.GetComponentInChildren<TMP_Text>();
        _text.color = _equipButtonTextColorDisabled;
        _text.text = "Equipped";
    }
    public async void SetSkin(PlayerSkinItem _item)
    {
        if (_tmpItemSelected.Key != null)
            _tmpItemSelected.Key.ChangePickState(_tmpItemSelected.Value);

        _tmpItemSelected = new KeyValuePair<PlayerSkinItem, ESKIN_ITEM_UI_STATE>(_item, _item.PickState);
        _tmpItemSelected.Key.ChangePickState(ESKIN_ITEM_UI_STATE.SELECTED);

        _tmpSkin = new KeyValuePair<string, int>(_item.Key, _item.Category);
        _skinLoader.LoadPreviewSkin(_tmpSkin);
        CheckForEquipButton(_item.Key);

        if (_item.Category == 3)
        {
            await Task.Delay(250);
            _playerAnimator.OpenBag();
        }
    }

    void CheckForEquipButton(string _key)
    {
        _playerCurrentSkins = DataLoader.LoadPlayerCurrentSkins();

        if (_playerCurrentSkins.Any(p => p == _key))
            DisableEquipButton();
        else
            EnableEquipButton();
    }

    public void UpdatePlayerSkinVisuals()
    {
        _skinLoader.LoadSkinFromCurrentData();
        DisableEquipButton();
    }

    public void ValidateSkin()
    {
        _playerCurrentSkins[_tmpSkin.Value] = _tmpSkin.Key;
        _tmpItemSelected = new KeyValuePair<PlayerSkinItem, ESKIN_ITEM_UI_STATE>(_tmpItemSelected.Key, ESKIN_ITEM_UI_STATE.PICKED);
        DataLoader.SavePlayerCurrentSkins(_playerCurrentSkins);
        DisableEquipButton();
        UpdatePlayerSkinVisuals();
        FilterItemSelection();
    }
}
