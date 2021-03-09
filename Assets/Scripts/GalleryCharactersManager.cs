using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GalleryCharactersManager : MonoBehaviour
{
    [SerializeField] GalleryFace _leadGalleryFace;
    [SerializeField] GameObject _basicCardsContainer, _epicCardsContainer;
    [SerializeField] RectTransform _basicButton, _epicButton;
    [SerializeField] TextMeshProUGUI _basicTx, _epicTx;
    [SerializeField] GalleryManager _galleryManager;
    [SerializeField] GameObject _backButton, _nextButton;

    [SerializeField] GallerySinglePhotoInstance[] _basicPrevPhotos, _epicPrevPhotos;

    [SerializeField] GallerySingleSkinPhotoInstance[] _singleSkins;
    int _currentMode;
    int _currentChar;
    public void LoadCharacterConfig(int charIndex)
    {
        List<int> _characterSkins = new List<int>();
        for(int i=0; i< SpecialSkinsManager._specialSkins.Length; i++)
        {
            if (SpecialSkinsManager._specialSkins[i]._character == charIndex)
            {
                _characterSkins.Add(i);
            }
        }
        for(int i = 0; i< _singleSkins.Length; i++)
        {
            if (i<_characterSkins.Count)
            {
                _singleSkins[i].gameObject.SetActive(true);
                _singleSkins[i].Init(_characterSkins[i]);
            }
            else
            {
                _singleSkins[i].gameObject.SetActive(false);
            }
        }
        _currentChar = charIndex;
        _leadGalleryFace.Init(charIndex, false);
        SelectViewMode(_currentMode);
        if(charIndex == 0)
        {
            _backButton.SetActive(false);
        }
        else
        {
            _backButton.SetActive(true);
        }
        if(charIndex < UserDataController.GetBiggestCharacter())
        {
            _nextButton.SetActive(true);
        }
        else
        {
            _nextButton.SetActive(false);
        }
    }

    public void ChangeCharacter(int sum)
    {
        LoadCharacterConfig(_currentChar + sum);
    }

    public void SelectViewMode(int mode)
    {
        _currentMode = mode;
        if (mode == 0)
        {
            _basicCardsContainer.SetActive(true);
            _basicButton.anchoredPosition = new Vector2(-275, 30);
            _basicTx.alignment = TextAlignmentOptions.Midline;
            _basicTx.fontSizeMax = 60f;

            _epicCardsContainer.SetActive(false);
            _epicButton.anchoredPosition = new Vector2(0, 8);
            _epicTx.alignment = TextAlignmentOptions.Top;
            _epicTx.fontSizeMax = 50f;
        }
        else
        {
            if (mode == 1)
            {
                _basicCardsContainer.SetActive(false);
                _basicButton.anchoredPosition = new Vector2(-275, 8);
                _basicTx.alignment = TextAlignmentOptions.Top;
                _basicTx.fontSizeMax = 50f;
                _epicCardsContainer.SetActive(true);
                _epicButton.anchoredPosition = new Vector2(0, 30);
                _epicTx.alignment = TextAlignmentOptions.Midline;
                _epicTx.fontSizeMax = 60f;
            }
            else
            {
                GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("SKINSANDANIMATIONS"));
            }
        }

        for (int i = 0; i < _basicPrevPhotos.Length; i++)
        {
            _basicPrevPhotos[i].Init((4 * _currentChar) + i);
        }
    }

    public void OpenSinglePhotoImage(int index)
    {
        _galleryManager.ShowFullScreen((4 * _currentChar) + index);
    }

}
