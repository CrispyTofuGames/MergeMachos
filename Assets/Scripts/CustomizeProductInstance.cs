using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CustomizeProductInstance : MonoBehaviour
{
    int _productIndex = 0;
    CustomizeController _customizeController;
    [SerializeField]
    Image _image, _background;
    [SerializeField]
    GameObject _inUse, _selected;
    [SerializeField]
    Sprite _unlocked, _locked;
    [SerializeField] GameObject _progressBar;
    [SerializeField] TextMeshProUGUI _progressText;
    CustomizeElementType _customizeElementType;
    private void Awake()
    {
        GameEvents.BuyCustomizeSkinFragments.AddListener(Refresh);
    }
    public void Init(CustomizeElementType customizeElementType,  int productIndex, CustomizeController customize, Sprite objectSprite)
    {
        bool unlocked = false;
        switch (customizeElementType)
        {
            case CustomizeElementType.Cell:
                unlocked = UserDataController.GetCellsSkins()[productIndex];
                break;

            case CustomizeElementType.Expositor:
                unlocked = UserDataController.GetExpositorsSkins()[productIndex];
                break;

            case CustomizeElementType.Ground:
                unlocked = UserDataController.GetGroundSkins()[productIndex];
                break;
        }

        _customizeElementType = customizeElementType;
        _productIndex = productIndex;
        _customizeController = customize;
        _image.sprite = objectSprite;
        _inUse.SetActive(false);
        _selected.SetActive(false);
        if (unlocked)
        {
            _progressBar.SetActive(false);
            _background.sprite = _unlocked;
            _image.color = Color.white;
        }
        else
        {
            _progressBar.SetActive(true);
            switch (customizeElementType)
            {
                case CustomizeElementType.Cell:
                    _progressText.text = UserDataController.GetCellFragments(_productIndex) + "/" + GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._cellSkins[productIndex]._rarity];
                    break;

                case CustomizeElementType.Expositor:
                    _progressText.text = UserDataController.GetExpositorFragments(_productIndex) + "/" + GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._expositorSkins[productIndex]._rarity];
                    break;

                case CustomizeElementType.Ground:
                    _progressText.text = UserDataController.GetGroundFragments(_productIndex) + "/" + GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._groundSkins[productIndex]._rarity];
                    break;
            }
            _background.sprite = _locked;
            _image.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        }
    }

    public void SelectProduct()
    {
        _customizeController.SelectProduct(_productIndex, _customizeElementType);
    }

    public void PreviewFeedback(bool inPreview)
    {
        _inUse.SetActive(inPreview);
    }
    public void SelectedFeedBack(bool selected)
    {
        _selected.SetActive(selected);
    }
    public void Refresh()
    {
        bool unlocked = false;
        switch (_customizeElementType)
        {
            case CustomizeElementType.Cell:
                unlocked = UserDataController.GetCellsSkins()[_productIndex];
                break;

            case CustomizeElementType.Expositor:
                unlocked = UserDataController.GetExpositorsSkins()[_productIndex];
                break;

            case CustomizeElementType.Ground:
                unlocked = UserDataController.GetGroundSkins()[_productIndex];
                break;
        }
        if (unlocked)
        {
            _progressBar.SetActive(false);
            _background.sprite = _unlocked;
            _image.color = Color.white;
        }
        else
        {
            _progressBar.SetActive(true);
            switch (_customizeElementType)
            {
                case CustomizeElementType.Cell:
                    _progressText.text = UserDataController.GetCellFragments(_productIndex) + "/" + GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._cellSkins[_productIndex]._rarity];
                    break;

                case CustomizeElementType.Expositor:
                    _progressText.text = UserDataController.GetExpositorFragments(_productIndex) + "/" + GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._expositorSkins[_productIndex]._rarity];
                    break;

                case CustomizeElementType.Ground:
                    _progressText.text = UserDataController.GetGroundFragments(_productIndex) + "/" + GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._groundSkins[_productIndex]._rarity];
                    break;
            }
            _background.sprite = _locked;
            _image.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        }
    }
    public void Unlock()
    {
        _background.sprite = _unlocked;
        _image.color = Color.white;
    }
}
