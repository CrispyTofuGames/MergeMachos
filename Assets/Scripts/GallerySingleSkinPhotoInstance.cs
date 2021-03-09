using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GallerySingleSkinPhotoInstance : MonoBehaviour
{
    int _myIndex;
    CardConfigurator _cardConfigurator;
    [SerializeField] TextMeshProUGUI _progressText;
    [SerializeField] GameObject _progressBar;
    [SerializeField] GameObject _moreFragmentsButton;
    [SerializeField] GameObject _unlockSkinButton;
    [SerializeField] GameObject _fullScreenButton;
    GalleryManager _galleryManager;
    bool _canOpenFullScreen;
    private void Awake()
    {
        GameEvents.BuySkinFragments.AddListener(Refresh);
    }
    public void Refresh()
    {
        _galleryManager = FindObjectOfType<GalleryManager>();
        Button b = _fullScreenButton.GetComponent<Button>();
        if (!_canOpenFullScreen)
        {
            b.onClick.AddListener(() => OpenSinglePhotoSkin());
        }
        _moreFragmentsButton.GetComponent<Button>().onClick.AddListener(() => BuyFragments());
        if (UserDataController.IsExtraSkinUnlocked(_myIndex))
        {
            _progressBar.SetActive(false);
            _unlockSkinButton.SetActive(false);
            _moreFragmentsButton.SetActive(false);
            _fullScreenButton.SetActive(true);
        }
        else
        {
            _fullScreenButton.SetActive(false);
            if (UserDataController.GetExtraSkinFragments(_myIndex) >= GameData.specialSkinsFragmentCapsByRarity[SpecialSkinsManager._specialSkins[_myIndex]._rarity])
            {
                _moreFragmentsButton.SetActive(false);
                _unlockSkinButton.SetActive(true);
            }
            else
            {
                _unlockSkinButton.SetActive(false);
                _moreFragmentsButton.SetActive(true);
            }
            _moreFragmentsButton.SetActive(true);
            _progressBar.SetActive(true);
            _progressText.text = UserDataController.GetExtraSkinFragments(_myIndex) + "/" + GameData.specialSkinsFragmentCapsByRarity[SpecialSkinsManager._specialSkins[_myIndex]._rarity];
        }
        //_galleryManager = FindObjectOfType<GalleryManager>();
        _cardConfigurator = GetComponent<CardConfigurator>();
        _cardConfigurator.InitSkin(_myIndex);
        //SetZoomButtonState(false);
        _unlockSkinButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _unlockSkinButton.GetComponent<Button>().onClick.AddListener(() => UnlockSkin());
    }
    public void Init(int skin)
    {
        _canOpenFullScreen = true;
        _galleryManager = FindObjectOfType<GalleryManager>();
        Button b = _fullScreenButton.GetComponent<Button>();
        b.onClick.AddListener(() => OpenSinglePhotoSkin());
        _moreFragmentsButton.GetComponent<Button>().onClick.AddListener(()=>BuyFragments());
        if (UserDataController.IsExtraSkinUnlocked(skin))
        {
            _progressBar.SetActive(false);
            _unlockSkinButton.SetActive(false);
            _moreFragmentsButton.SetActive(false);
            _fullScreenButton.SetActive(true);
        }
        else
        {
            _fullScreenButton.SetActive(false);
            if (UserDataController.GetExtraSkinFragments(skin) >= GameData.specialSkinsFragmentCapsByRarity[SpecialSkinsManager._specialSkins[skin]._rarity])
            {
                _moreFragmentsButton.SetActive(false);
                _unlockSkinButton.SetActive(true);
            }
            else
            {
                _unlockSkinButton.SetActive(false);
                _moreFragmentsButton.SetActive(true);
            }
            _moreFragmentsButton.SetActive(true);
            _progressBar.SetActive(true);
            _progressText.text = UserDataController.GetExtraSkinFragments(skin)+ "/" + GameData.specialSkinsFragmentCapsByRarity[SpecialSkinsManager._specialSkins[skin]._rarity];
        }
        _myIndex = skin;
        //_galleryManager = FindObjectOfType<GalleryManager>();
        _cardConfigurator = GetComponent<CardConfigurator>();
        _cardConfigurator.InitSkin(skin);
        //SetZoomButtonState(false);
        _unlockSkinButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _unlockSkinButton.GetComponent<Button>().onClick.AddListener(()=>UnlockSkin());
    }
    public void InitWithoutButton(int skin)
    {
        _canOpenFullScreen = false;
        _galleryManager = FindObjectOfType<GalleryManager>();
        Button b = _fullScreenButton.GetComponent<Button>();
        //b.onClick.AddListener(() => OpenSinglePhotoSkin());
        _moreFragmentsButton.GetComponent<Button>().onClick.AddListener(() => BuyFragments());
        if (UserDataController.IsExtraSkinUnlocked(skin))
        {
            _progressBar.SetActive(false);
            _unlockSkinButton.SetActive(false);
            _moreFragmentsButton.SetActive(false);
            _fullScreenButton.SetActive(true);
        }
        else
        {
            _fullScreenButton.SetActive(false);
            if (UserDataController.GetExtraSkinFragments(skin) >= GameData.specialSkinsFragmentCapsByRarity[SpecialSkinsManager._specialSkins[skin]._rarity])
            {
                _moreFragmentsButton.SetActive(false);
                _unlockSkinButton.SetActive(true);
            }
            else
            {
                _unlockSkinButton.SetActive(false);
                _moreFragmentsButton.SetActive(true);
            }
            _moreFragmentsButton.SetActive(true);
            _progressBar.SetActive(true);
            _progressText.text = UserDataController.GetExtraSkinFragments(skin) + "/" + GameData.specialSkinsFragmentCapsByRarity[SpecialSkinsManager._specialSkins[skin]._rarity];
        }
        _myIndex = skin;
        //_galleryManager = FindObjectOfType<GalleryManager>();
        _cardConfigurator = GetComponent<CardConfigurator>();
        _cardConfigurator.InitSkin(skin);
        //SetZoomButtonState(false);
        _unlockSkinButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _unlockSkinButton.GetComponent<Button>().onClick.AddListener(() => UnlockSkin());
    }
    public void UnlockSkin()
    {
        _galleryManager.UnlockExtraSkin(_myIndex);
        Init(_myIndex);
    }
    public void BuyFragments()
    {
        FindObjectOfType<BuyFragmentsController>().ShowInfo(FragmentType.SkinFragments, _myIndex, UserDataController.GetRemainingSkinFragments(_myIndex));
    }
    public void OpenSinglePhotoSkin()
    {
        _galleryManager.ShowSkinFullScreen(_myIndex);
    }

}
