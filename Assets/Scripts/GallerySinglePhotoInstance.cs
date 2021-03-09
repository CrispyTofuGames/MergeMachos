using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GallerySinglePhotoInstance : MonoBehaviour
{
    [SerializeField]
    Image _mainImage;
    [SerializeField]
    GameObject _hardCoinsButton;
    [SerializeField]
    GameObject _softCoinsButton;
    [SerializeField]
    GameObject _lockIcon;
    [SerializeField]
    TextMeshProUGUI _softCoinsText;
    [SerializeField]
    TextMeshProUGUI _hardCoinsText;
    GalleryManager _galleryManager;
    [SerializeField]
    Image _frameImage;
    [SerializeField]
    GameObject _zoomIcon, _foreGround;
    [SerializeField]
    Image _background;
    int _myIndex;
    CardConfigurator _cardConfigurator;

    public void InitSkin(int skinIndex)
    {
        //_myIndex = characterIndex;
        _galleryManager = FindObjectOfType<GalleryManager>();
        _cardConfigurator = GetComponent<CardConfigurator>();
        _cardConfigurator.InitSkin(skinIndex);
        _softCoinsButton.SetActive(false);
        _hardCoinsButton.SetActive(false);
        _lockIcon.SetActive(false);
        SetZoomButtonState(false);
    }
    public void Init(int characterIndex)
    {
        _myIndex = characterIndex;
        _galleryManager = FindObjectOfType<GalleryManager>();
        _cardConfigurator = GetComponent<CardConfigurator>();
        int biggestDino = UserDataController.GetBiggestDino() + 1;
        _cardConfigurator.Init(characterIndex);
        Button _softCoins = _softCoinsButton.GetComponent<Button>();
        Button _hardCoins = _hardCoinsButton.GetComponent<Button>();
        _softCoins.onClick.RemoveAllListeners();
        _hardCoins.onClick.RemoveAllListeners();
        _softCoins.onClick.AddListener(() =>UnlockSkinSoftCoins());
        _hardCoins.onClick.AddListener(() => UnlockSkinHardCoins());
        _softCoinsText.text = _galleryManager.GetSoftCostByIndex(characterIndex);
        _hardCoinsText.text = _galleryManager.GetHardCostByIndex(characterIndex);
        
        SetZoomButtonState(false);


        if (characterIndex <= (biggestDino * 2)-1)
        {
            if (UserDataController.IsSkinUnlocked(characterIndex))
            {
                UnlockAvailable();
                SetZoomButtonState(true);
            }
            else
            {
                UnlockUnavailable();
            }
        }
        else
        {
            Lock();
        }
    }
    public void UnlockSkinSoftCoins()
    {
        int purchaseType = 1;
        if (_galleryManager.IsFullScreen())
        {
            purchaseType = 2;
        }
        if (_galleryManager.TryUnlockSkinSoftCoins(_myIndex, purchaseType))
        {
            Init(_myIndex);
        }
    }
    public void UnlockSkinHardCoins()
    {
        int purchaseType = 1;
        if (_galleryManager.IsFullScreen())
        {
            purchaseType = 2;
        }
        if (_galleryManager.TryUnlockSkinHardCoins(_myIndex, purchaseType))
        {
            Init(_myIndex);
        }
    }
    public void EnableShinyMode()
    {
        _cardConfigurator.ActiveShinyMode();
    }

    public void SetZoomButtonState(bool state)
    {
        if(_zoomIcon != null)
        {
            _zoomIcon.SetActive(state);
        }
        _foreGround.SetActive(state);
    }

    public void Lock()
    {
        _hardCoinsButton.SetActive(false);
        _softCoinsButton.SetActive(false);
        _mainImage.color = Color.black;
        _lockIcon.SetActive(true);
    }

    public void UnlockAvailable()
    {
        _lockIcon.SetActive(false);
        _mainImage.color = Color.white;
        _hardCoinsButton.SetActive(false);
        _softCoinsButton.SetActive(false);
    }

    public void UnlockUnavailable()
    {
        _lockIcon.SetActive(false);
        _hardCoinsButton.SetActive(true);
        _softCoinsButton.SetActive(true);
        _mainImage.color = Color.black;
    }

    public void OpenBigView()
    {
        _galleryManager.ShowFullScreen(_myIndex);
    }
}
