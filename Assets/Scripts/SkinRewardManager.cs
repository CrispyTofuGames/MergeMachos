using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SkinRewardManager : MonoBehaviour
{
    [SerializeField] GameObject _mainPanel;
    [SerializeField] AnimationCurve animationCurve;
    GalleryManager _galleryManager;
    [SerializeField] GameObject _gallery;
    PanelManager _panelManager;
    [SerializeField] TextMeshProUGUI _skinNameText;
    [SerializeField] AnimationCurve _animationCurve;
    [SerializeField] RectTransform _mainImageElement;
    [SerializeField] RectTransform _continueButton;
    [SerializeField] GallerySingleIllustrationManager _gallerySingleIllustrationManager;
    int _purchaseType;
    int _skinIndex;
    [SerializeField] CardConfigurator _cardConfigurator;

    private void Awake()
    {
        GameEvents.GetSkin.AddListener(OpenSkinReward);
        _panelManager = FindObjectOfType<PanelManager>();
        _galleryManager = FindObjectOfType<GalleryManager>();
    }
    public void OpenSkinReward(GameEvents.UnlockSkinEventData unlockSkinData)
    {
        if (unlockSkinData._extraSkin)
        {
            StartCoroutine(AnimateSkinOpenPanel(unlockSkinData._skinIndex));
            _purchaseType = unlockSkinData._purchaseType;
            _skinIndex = SpecialSkinsManager._specialSkins[unlockSkinData._skinIndex]._character*4;
        }
        else
        {
            StartCoroutine(AnimateOpenPanel(unlockSkinData._skinIndex));
            _purchaseType = unlockSkinData._purchaseType;
            _skinIndex = unlockSkinData._skinIndex;
            if (unlockSkinData._skinIndex % 2 == 0)
            {
                UserDataController.SetGalleryImage(unlockSkinData._skinIndex, true);
            }
            _galleryManager.RefreshWarningIcons();
        }
    }
    IEnumerator AnimateOpenPanel(int skinIndex)
    {
        _panelManager.RequestShowPanel(_mainPanel);
        _cardConfigurator.Init(skinIndex);
        _skinNameText.text = _gallerySingleIllustrationManager.GetPhotoName(skinIndex);
        _skinNameText.rectTransform.localScale = Vector3.zero;
        _continueButton.localScale = Vector3.zero;
        _mainImageElement.localScale = Vector3.zero;

        while (!_mainPanel.activeSelf)
        {
            yield return null;
        }
        GameEvents.PlaySFX.Invoke("AlbumPhotoUnlocked");

        for (float i = 0; i< 0.25f; i += Time.deltaTime)
        {
            yield return null;
            _mainImageElement.localScale = Vector3.one* _animationCurve.Evaluate(i / 0.25f);
        }
        _mainImageElement.localScale = Vector3.one;
        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            yield return null;
            _skinNameText.rectTransform.localScale = Vector3.one * _animationCurve.Evaluate(i / 0.25f);
        }
        _skinNameText.rectTransform.localScale = Vector3.one;

        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            yield return null;
            _continueButton.localScale = Vector3.one * _animationCurve.Evaluate(i / 0.25f);
        }
        _continueButton.localScale = Vector3.one;
    }
    IEnumerator AnimateSkinOpenPanel(int skinIndex)
    {
        
        _panelManager.RequestShowPanel(_mainPanel);
        _cardConfigurator.InitSkin(skinIndex);
        int character = SpecialSkinsManager._specialSkins[skinIndex]._character;
        _skinNameText.text = SpecialSkinsManager._specialSkins[skinIndex]._name;
        _skinNameText.rectTransform.localScale = Vector3.zero;
        _continueButton.localScale = Vector3.zero;
        _mainImageElement.localScale = Vector3.zero;

        while (!_mainPanel.activeSelf)
        {
            yield return null;
        }
        GameEvents.PlaySFX.Invoke("AlbumPhotoUnlocked");

        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            yield return null;
            _mainImageElement.localScale = Vector3.one * _animationCurve.Evaluate(i / 0.25f);
        }
        _mainImageElement.localScale = Vector3.one;
        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            yield return null;
            _skinNameText.rectTransform.localScale = Vector3.one * _animationCurve.Evaluate(i / 0.25f);
        }
        _skinNameText.rectTransform.localScale = Vector3.one;

        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            yield return null;
            _continueButton.localScale = Vector3.one * _animationCurve.Evaluate(i / 0.25f);
        }
        _continueButton.localScale = Vector3.one;
    }
    public void CloseSkinReward()
    {
        
        if(_purchaseType == 0)
        {
            GameEvents.MergeDino.Invoke(_skinIndex/ 2);
            GameEvents.RewardMergeUp.Invoke(_skinIndex / 2);
            _panelManager.ClosePanel();
        }
        else
        {
            if(_purchaseType == 1)
            {
                _panelManager.ClosePanel();
                _galleryManager.OpenGallery();
                _galleryManager.OpenCharacterData(_skinIndex / 4);
            }
            else
            {
                _panelManager.ClosePanel();
                _galleryManager.OpenGallery();
                _galleryManager.ShowFullScreen(_skinIndex);
            }
        }
    }
}
