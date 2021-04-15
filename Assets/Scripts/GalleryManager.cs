using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GalleryManager : MonoBehaviour
{
    [SerializeField] GameObject _mainPanel;
    [SerializeField] GameObject _warningIcon;
    PanelManager _panelManager;
    [SerializeField] GameObject _galleryHorizontalContainer;
    [SerializeField] GameObject _galleryImage;
    [SerializeField] RectTransform _gridWithoutElements;
    [SerializeField] RectTransform _faceGalleryRT;
    [SerializeField] RectTransform takoskin1SoftCoinButton;
    [SerializeField] RectTransform takoskin3SoftCoinButton;
    [SerializeField] GameObject _backButton;
    [SerializeField] GameObject _hardCoinsButton;
    [SerializeField] GameObject _softCoinsButton;
    [SerializeField] GameObject _lockIcon;
    [SerializeField] GameObject _faceGallery, _characterGallery, _onePhotoGallery, _fullModeGallery, _fullGalleryBasicGrid, _fullGallerySkinGrid;
    [SerializeField] GameObject _leftButton, _rightButton;
    [SerializeField] Image _fullScreenImage;
    [SerializeField] FaceGallery _faceGalleryInitializer;
    [SerializeField] GalleryCharactersManager _galleryCharactersManager;
    [SerializeField] GallerySingleIllustrationManager _gallerySingleIllustrationManager;
    [SerializeField] ScrollRect _scrollController;
    [SerializeField] Button _girlsButton, _basicButton, _skinButton, _animationsButton;
    [SerializeField] GameObject _swapButtonsNull;
    [SerializeField] GalleryFullModeInitializer _galleryFullModeInitializer;
    List<GameCurrency> _softCoinPrizes;
    List<int> _hardCoinPrizes = new List<int>(){0, 0, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50};
    bool _canBack = false;
    bool _onePhotoOpened = false;
    bool _swapping = false;
    bool _fullMode = false;
    int _currentChar;
    //CAMBIAR MAS ADELANTE 
    private void Awake()
    {
        _softCoinPrizes = new List<GameCurrency>()
        {
            new GameCurrency(0),
            new GameCurrency(0),
            new GameCurrency(new int[] {500, 52}),
            new GameCurrency(new int[] {500, 112}),
            new GameCurrency(new int[] {500, 238}),
            new GameCurrency(new int[] {0, 471}),
            new GameCurrency(new int[] {0, 0, 1}),
            new GameCurrency(new int[] {0, 139, 2}),
            new GameCurrency(new int[] {0, 519, 4}),
            new GameCurrency(new int[] {0, 555, 9}),
            new GameCurrency(new int[] {0, 500, 19}),
            new GameCurrency(new int[] {0, 0, 42}),
            new GameCurrency(new int[] {0, 0, 90}),
            new GameCurrency(new int[] {0, 0, 189}),
            new GameCurrency(new int[] {0, 0, 402}),
            new GameCurrency(new int[] {0, 0, 850}),
            new GameCurrency(new int[] { 0, 0, 797, 1}),
            new GameCurrency(new int[] { 0, 0, 789,3}),
            new GameCurrency(new int[] { 0, 0, 28, 8}),
            new GameCurrency(new int[] {0, 0, 965, 16}),
            new GameCurrency(new int[] { 0, 0, 850, 44}),
            new GameCurrency(new int[] { 0, 0, 750, 75}),
            new GameCurrency(new int[] { 0, 0, 69, 160}),
            new GameCurrency(new int[] { 0, 0, 300, 338}),
            new GameCurrency(new int[] { 0, 0, 930, 714}),
            new GameCurrency(new int[] { 0, 0, 0, 500, 1}),
            new GameCurrency(new int[] { 0, 0, 0, 200, 3}),
            new GameCurrency(new int[] { 0, 0, 0, 750, 6}),
            new GameCurrency(new int[] { 0, 0, 0, 250, 14}),
            new GameCurrency(new int[] { 0, 0, 0, 750, 63}),
            new GameCurrency(new int[] { 0, 0, 0, 500, 133}),
            new GameCurrency(new int[] { 0, 0, 0, 500, 251}),
            new GameCurrency(new int[] { 0, 0, 0, 500, 547}),
            new GameCurrency(new int[] { 0, 0, 0, 500, 103, 1}),
            new GameCurrency(new int[] { 0, 0, 0, 500, 256, 3}),
            new GameCurrency(new int[] { 0, 0, 0, 500, 750, 6}),
            new GameCurrency(new int[] { 0, 0, 0, 500, 250, 14}),
            new GameCurrency(new int[] { 0, 0, 0, 500, 250, 36}),
            new GameCurrency(new int[] { 0, 0, 0, 500, 250, 75}),
            new GameCurrency(new int[] { 0, 0, 0, 500, 250, 164}),
            new GameCurrency(new int[] { 0, 0, 0, 500, 250, 341})
        };
        GameEvents.DinoUp.AddListener(UnlockDinoInGallery);
    }
    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
    }

    public void UnlockDinoInGallery(int dino)
    {
        int skinUnlocked = (dino * 2);
        UserDataController.UnlockSkin(skinUnlocked);
        RefreshWarningIcons();
    }

    public RectTransform GetTakoFaceButton()
    {
        return _faceGalleryRT.GetChild(0).GetChild(0).GetComponent<RectTransform>();
    }
    public GameObject GetCharacterGallery()
    {
        return _characterGallery;
    }
    public RectTransform GetTakoSoftCoinsButton(int index)
    {
        if (index == 0)
        {
            return takoskin1SoftCoinButton;
        }
        else
        {
            return takoskin3SoftCoinButton;
        }
    }
    public void OpenGallery()
    {
        if (!UserDataController.HasSeenGalleryTutorial())
        {
            FindObjectOfType<Tutorial>().PlayGalleryTutorial();
        }

        RefreshWarningIcons();

        _fullMode = false;
        //_galleryFullModeInitializer.BasicMode();

        _faceGallery.SetActive(true);
        _fullModeGallery.SetActive(false);
        _scrollController.content = _faceGallery.GetComponent<RectTransform>();

        _swapButtonsNull.SetActive(true);
        _girlsButton.interactable = false;
        _basicButton.interactable = true;
        _skinButton.interactable = true;
        _animationsButton.interactable = true;

        _onePhotoGallery.SetActive(false);
        _characterGallery.SetActive(false);
        _backButton.SetActive(false);
        _canBack = false;
        _panelManager.RequestShowPanel(_mainPanel);
        _faceGalleryInitializer.RefreshFaces();
    }
    public void CloseGallery()
    {
        _mainPanel.SetActive(false);
        _gallerySingleIllustrationManager.SetOnePhotoState(false);
        _canBack = false;
        _panelManager.ClosePanel();
    }

    public void OpenCharacterData(int character)
    {
        _swapButtonsNull.SetActive(false);
        _faceGallery.SetActive(false);
        _fullModeGallery.SetActive(false);
        _characterGallery.SetActive(true);
        _onePhotoGallery.SetActive(false);
        _gallerySingleIllustrationManager.SetOnePhotoState(false);
        _backButton.SetActive(true);
        _galleryCharactersManager.LoadCharacterConfig(character);
        _canBack = true;
    }

    public void ShowFullScreen(int characterIndex)
    {
        _currentChar = characterIndex;

        if(characterIndex % 4 == 0)
        {
            _leftButton.SetActive(false);
        }
        else
        {
            _leftButton.SetActive(true);
        }
        if(characterIndex % 4 == 3)
        {
            _rightButton.SetActive(false);
        }
        else
        {
            _rightButton.SetActive(true);
        }
        _gallerySingleIllustrationManager.LoadConfig(characterIndex);

        if (UserDataController.IsSkinUnlocked(characterIndex))
        {
            _gallerySingleIllustrationManager.SetOnePhotoState(true);
        }
        else
        {
            _gallerySingleIllustrationManager.SetOnePhotoState(false);
        }

        _swapButtonsNull.SetActive(false);
        _faceGallery.SetActive(false);
        _fullModeGallery.SetActive(false);
        _characterGallery.SetActive(false);
        _onePhotoGallery.SetActive(true);
        _backButton.SetActive(true);
        _onePhotoOpened = true;
        _canBack = true;
    }
    public void ShowSkinFullScreen(int skinIndex)
    {   
        _currentChar = SpecialSkinsManager._specialSkins[skinIndex]._character * 4;
        _leftButton.SetActive(false);
        _rightButton.SetActive(false);
        _gallerySingleIllustrationManager.LoadSkinConfig(skinIndex);
        //if (UserDataController.IsSkinUnlocked(characterIndex))
        //{
        //    _gallerySingleIllustrationManager.SetOnePhotoState(true);
        //}
        //else
        //{
        //    _gallerySingleIllustrationManager.SetOnePhotoState(false);
        //}
        _faceGallery.SetActive(false);
        _swapButtonsNull.SetActive(false);
        _girlsButton.interactable = true;
        _basicButton.interactable = true;
        _skinButton.interactable = false;
        _animationsButton.interactable = true;
        _fullModeGallery.SetActive(false);
        _characterGallery.SetActive(false);
        _onePhotoGallery.SetActive(true);
        _backButton.SetActive(true);
        _onePhotoOpened = true;
        _canBack = true;
    }
    public void SwapFullScreen(int amount) 
    {
        if ((_currentChar + amount) % 4 >= 0 && (_currentChar + amount) % 4 <= 3)
        {
            if (!_swapping)
            {
                StartCoroutine(SwapTransition(amount));
            }       
        }
    }

    IEnumerator SwapTransition(int amount)
    {
        _swapping = true;
        float dur = 0.25f;
        Color currentColor = _fullScreenImage.color;
        currentColor.a = 1;
        Color currentColorTransparent = currentColor;
        currentColorTransparent.a = 0;

        for(float i = 0; i< dur; i+= Time.deltaTime)
        {
            _fullScreenImage.color = Color.Lerp(currentColor, currentColorTransparent, i / dur);
            yield return null;
        }
        _fullScreenImage.color = currentColorTransparent;

        ShowFullScreen(_currentChar + amount);

        currentColor = _fullScreenImage.color;
        currentColor.a = 1;
        currentColorTransparent = currentColor;
        currentColorTransparent.a = 0;

        for (float i = 0; i < dur; i += Time.deltaTime)
        {
            _fullScreenImage.color = Color.Lerp(currentColorTransparent, currentColor, i / dur);
            yield return null;
        }
        _fullScreenImage.color = currentColor;
        _swapping = false;
    }
    public void UnlockExtraSkin(int skin)
    {
        UserDataController.UnlockExtraSkin(skin);
        GameEvents.GetSkin.Invoke(new GameEvents.UnlockSkinEventData(skin, 1, true));
        _panelManager.ClosePanel();
    }

    public bool TryUnlockSkinSoftCoins(int skin, int purchaseType)
    {
        bool canUnlockSkin = false;
        EconomyManager economyManager = FindObjectOfType<EconomyManager>();
        if (economyManager.SpendSoftCoins(_softCoinPrizes[(skin-1)/2]))
        {
            canUnlockSkin = true;
            UserDataController.UnlockSkin(skin);
            GameEvents.GetSkin.Invoke(new GameEvents.UnlockSkinEventData(skin, purchaseType));
            _panelManager.ClosePanel();
        }
        RefreshWarningIcons();
        return canUnlockSkin;
    }
    public bool TryUnlockSkinHardCoins(int skin, int purchaseType)
    {
        bool canUnlockSkin = false;

        EconomyManager economyManager = FindObjectOfType<EconomyManager>();
        if (economyManager.SpendHardCoins(_hardCoinPrizes[(skin - 1) / 2])) //PRECIOS
        {
            UserDataController.UnlockSkin(skin);
            canUnlockSkin = true;
            GameEvents.GetSkin.Invoke(new GameEvents.UnlockSkinEventData(skin, purchaseType));
            _panelManager.ClosePanel();
        }
        RefreshWarningIcons();
        return canUnlockSkin;
    }

    public bool TryUnlockSpecialCard(int skin)
    {
        bool canUnlockSkin = false;
        EconomyManager economyManager = FindObjectOfType<EconomyManager>();
        if (economyManager.SpendHardCoins(50))
        {
            UserDataController.UnlockSpecialCard(skin);
            canUnlockSkin = true;
            //LLAMAR AL REWARD
        }
        return canUnlockSkin;
    }

    public string GetSoftCostByIndex(int index)
    {
        return _softCoinPrizes[(index - 1) / 2].GetCurrentMoneyConvertedTo3Chars();
    }
    public string GetHardCostByIndex(int index)
    {
        return _hardCoinPrizes[(index - 1) / 2].ToString();
    }

    public void RefreshWarningIcons()
    {
        _warningIcon.SetActive(false);
        for (int i = 0; i<UserDataController.GetGalleryImagesToOpen().Length; i++)
        {
            if (UserDataController.GetGalleryImagesToOpen()[i])
            {
                _warningIcon.SetActive(true);
            }
        }
    }
    public void Back()
    {   
        RefreshWarningIcons();

        if (_onePhotoOpened)
        {
            _swapButtonsNull.SetActive(true);
            if (_fullMode)
            {
                _galleryFullModeInitializer.BasicMode();
                _fullModeGallery.SetActive(true);
                _faceGallery.SetActive(false);
                _girlsButton.interactable = true;
                _basicButton.interactable = false;
                _skinButton.interactable = true;
                _animationsButton.interactable = true;
                _scrollController.content = _fullGalleryBasicGrid.GetComponent<RectTransform>();
                _characterGallery.SetActive(false);
                _faceGalleryInitializer.RefreshFaces();
                _onePhotoGallery.SetActive(false);
                _backButton.SetActive(false);
                _canBack = false;
            }
            else
            {
                _faceGallery.SetActive(false);
                _fullModeGallery.SetActive(false);
                _characterGallery.SetActive(true);
                _onePhotoGallery.SetActive(false);
                _onePhotoOpened = false;
                _gallerySingleIllustrationManager.SetOnePhotoState(false);
                OpenCharacterData(_currentChar / 4);
            }
        }
        else
        {
            _swapButtonsNull.SetActive(true);
            if (!_fullMode)
            {
                _faceGallery.SetActive(true);
                _fullModeGallery.SetActive(false);
                _girlsButton.interactable = false;
                _basicButton.interactable = true;
                _skinButton.interactable = true;
                _animationsButton.interactable = true;
                _scrollController.content = _faceGallery.GetComponent<RectTransform>();
            }
            else
            {
                _galleryFullModeInitializer.BasicMode();
                _fullModeGallery.SetActive(true);
                _faceGallery.SetActive(false);
                _girlsButton.interactable = true;
                _basicButton.interactable = false;
                _skinButton.interactable = true;
                _animationsButton.interactable = true;
                _scrollController.content = _fullGalleryBasicGrid.GetComponent<RectTransform>();
            }
            _characterGallery.SetActive(false);
            _faceGalleryInitializer.RefreshFaces();
            _onePhotoGallery.SetActive(false);
            _backButton.SetActive(false);
            _canBack = false;
        }
    }
   
    public bool IsFullScreen()
    {
        return _onePhotoOpened;
    }

    public bool CanBack()
    {
        return _canBack;
    }

    public void GirlsButton()
    {
        _fullMode = false;
        _faceGallery.SetActive(true);
        _fullModeGallery.SetActive(false);
        _girlsButton.interactable = false;
        _basicButton.interactable = true;
        _skinButton.interactable = true;
        _animationsButton.interactable = true;
        _scrollController.content = _faceGallery.GetComponent<RectTransform>();
    }

    public void BasicFullButton()
    {
        _fullMode = true;
        _faceGallery.SetActive(false);
        _fullModeGallery.SetActive(true);
        _girlsButton.interactable = true;
        _basicButton.interactable = false;
        _skinButton.interactable = true;
        _animationsButton.interactable = true;
        _galleryFullModeInitializer.BasicMode();
        _scrollController.content = _fullGalleryBasicGrid.GetComponent<RectTransform>();
    }

    public void SkinsFullButton()
    {
        _fullMode = true;
        _faceGallery.SetActive(false);
        _fullModeGallery.SetActive(true);
        _girlsButton.interactable = true;
        _basicButton.interactable = true;
        _skinButton.interactable = false;
        _animationsButton.interactable = true;
        _galleryFullModeInitializer.SkinMode();
        _scrollController.content = _fullGallerySkinGrid.GetComponent<RectTransform>();
    }

    public void AnimationFullButton()
    {
        _fullMode = true;
        _faceGallery.SetActive(false);
        _fullModeGallery.SetActive(true);
        _girlsButton.interactable = true;
        _basicButton.interactable = true;
        _skinButton.interactable = true;
        _animationsButton.interactable = false;
        _galleryFullModeInitializer.AnimationMode();
        _scrollController.content = _fullGallerySkinGrid.GetComponent<RectTransform>();
    }

    public void SwapGrid(bool isBasicMode)
    {
        if (isBasicMode)
        {
            _scrollController.content = _fullGalleryBasicGrid.GetComponent<RectTransform>();
        }
        else
        {
            _scrollController.content = _fullGallerySkinGrid.GetComponent<RectTransform>();
        }
    }

    public void LoadAnimatedScene(int charIndex)
    {
        PlayerPrefs.SetInt("AnimatedCharIndex", charIndex);
        GameEvents.LoadScene.Invoke("AnimatedScene");
    }
}
