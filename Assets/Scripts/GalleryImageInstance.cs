using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GalleryImageInstance : MonoBehaviour
{
    [SerializeField]
    Image _chibiImage;
    [SerializeField]
    GameObject _hardCoinsButton;
    [SerializeField]
    public GameObject _softCoinsButton;
    [SerializeField]
    GameObject _openFullScreenButton;
    [SerializeField]
    GameObject _lockIcon;
    [SerializeField]
    Image _frameImage;
    [SerializeField]
    TextMeshProUGUI _softCoinsText;
    [SerializeField]
    TextMeshProUGUI _hardCoinsText;
    int _characterIndex;
    GalleryManager _galleryManager;
    [SerializeField]
    GameObject _warningIcon;
    CardConfigurator _cardConfigurator;

    public void Init(int characterIndex)
    {
        _galleryManager = FindObjectOfType<GalleryManager>();
        _cardConfigurator = GetComponent<CardConfigurator>();
        _cardConfigurator.Init(characterIndex);
        Button _softCoins = _softCoinsButton.GetComponent<Button>();
        Button _hardCoins = _hardCoinsButton.GetComponent<Button>();
        _softCoins.onClick.RemoveAllListeners();
        _hardCoins.onClick.RemoveAllListeners();
        _softCoins.onClick.AddListener(() => _galleryManager.TryUnlockSkinSoftCoins(characterIndex, 1));
        _hardCoins.onClick.AddListener(() => _galleryManager.TryUnlockSkinHardCoins(characterIndex, 1));
        _characterIndex = characterIndex;

        int biggestDino = UserDataController.GetBiggestDino();
        _softCoinsText.text = _galleryManager.GetSoftCostByIndex(characterIndex);
        _hardCoinsText.text = _galleryManager.GetHardCostByIndex(characterIndex);

        if (characterIndex <= (biggestDino*2)+1)
        {
            if (UserDataController.IsSkinUnlocked(characterIndex))
            {
                _chibiImage.color = Color.white;
                _hardCoinsButton.SetActive(false);
                _softCoinsButton.SetActive(false);
                _openFullScreenButton.SetActive(true);
            }
            else
            {
                _hardCoinsButton.SetActive(true);
                _softCoinsButton.SetActive(true);
                _openFullScreenButton.SetActive(false);
                _chibiImage.color = Color.black;
            }
            _lockIcon.SetActive(false);
        }
        else
        {
            _openFullScreenButton.SetActive(false);
            _hardCoinsButton.SetActive(false);
            _softCoinsButton.SetActive(false);
            _lockIcon.SetActive(true);              
        }
        RefreshIconInstance();
    }
    public void OpenFullScreen()
    {
        _galleryManager.ShowFullScreen(_characterIndex);
    }
    public void RefreshIconInstance()
    {
        if (UserDataController.GetGalleryImagesToOpen()[_characterIndex])
        {
            _warningIcon.SetActive(true);
        }
        else
        {
            _warningIcon.SetActive(false);
        }
    }
}
