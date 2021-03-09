using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomizeController : MonoBehaviour
{
    [SerializeField]
    RectTransform _container;
    [SerializeField]
    TextMeshProUGUI _categoryText, _priceText;
    [SerializeField]
    GameObject _mainPanel, _bgSelector;
    PanelManager _panelManager;
    [SerializeField]
    GameObject _productPrefab;
    [SerializeField]
    GameObject _fullProductPanel;
    [SerializeField]
    Image _cellCategory, _expositorCategory, _groundCategory, _frameCategory;
    [SerializeField]
    GameObject _onePhotoCustomize, _frameCustomize, _selectButton, _unlockButton, _moreFragmentsButton;
    [SerializeField]
    CustomizeBigPhotoInstance _customizeBigPhoto, _customizeFrame;
    List<CustomizeProductInstance> _customizeProductInstances;
    [SerializeField]
    GameObject _backButton;
    CellManager _cellManager;
    EconomyManager _economyManager;
    RewardManager _rewardManager;
    List<int> _cellsCost = new List<int>() {0, 50, 50, 50, 50, 100, 100, 100, 100};
    List<int> _expositorsCost = new List<int>() {0, 50, 50, 50, 100, 100, 100, 100};
    List<int> _groundsCost = new List<int>() {0, 50, 50, 50, 100, 100, 100, 100};
    List<int> _framesCost = new List<int>() {0, 50, 50, 50, 100 };
    List<List<int>> _costList;

    bool _canBack = false;
    CustomizeElementType _currentType;

    int _productsAmount = 0;
    int _userSelectedProduct = 0;
    int _currentProduct = 0;
    int _currentCategory = 0;
    [SerializeField] CustomizeRewardManager _customizeRewardManager;
    public void OpenCustomize()
    {
        _panelManager.RequestShowPanel(_mainPanel);
        _fullProductPanel.SetActive(true);
        _bgSelector.SetActive(false);
        _selectButton.SetActive(false);
        _backButton.SetActive(false);
        _onePhotoCustomize.SetActive(false);
        _frameCustomize.SetActive(false);
        _canBack = false;
        RefreshInitialIcons();
    }
    private void Awake()
    {
        GameEvents.BuyCustomizeSkinFragments.AddListener(RefreshSelectedInfo);
    }
    public void CloseCustomize()
    {
        _mainPanel.SetActive(false);
    }

    public void RefreshInitialIcons()
    {
        _cellCategory.sprite = Resources.Load<Sprite>("Sprites/Cells/" + UserDataController.GetCurrentCell());
        _cellCategory.overrideSprite = Resources.Load<Sprite>("Sprites/Cells/" + UserDataController.GetCurrentCell());
        _expositorCategory.sprite = Resources.Load<Sprite>("Sprites/Expositors/" + UserDataController.GetCurrentExpositor());
        _expositorCategory.overrideSprite = Resources.Load<Sprite>("Sprites/Expositors/" + UserDataController.GetCurrentExpositor());
        _groundCategory.sprite = Resources.Load<Sprite>("Sprites/Grounds/" + UserDataController.GetCurrentGround());
        _groundCategory.overrideSprite = Resources.Load<Sprite>("Sprites/Grounds/" + UserDataController.GetCurrentGround());
        _frameCategory.sprite = Resources.Load<Sprite>("Sprites/Frames/" + UserDataController.GetCurrentFrame());
        _frameCategory.overrideSprite = Resources.Load<Sprite>("Sprites/Frames/" + UserDataController.GetCurrentFrame());
    }
    private void Start()
    {
        _cellManager = FindObjectOfType<CellManager>();
        _panelManager = FindObjectOfType<PanelManager>();
        _economyManager = FindObjectOfType<EconomyManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
        _customizeProductInstances = new List<CustomizeProductInstance>();
        _costList = new List<List<int>>();
        _costList.Add(_cellsCost);
        _costList.Add(_expositorsCost);
        _costList.Add(_groundsCost);
        _costList.Add(_framesCost);
    }

    public int GetHardCoinsProductCost(int categoryIndex, int productIndex)
    {
        return _costList[categoryIndex][productIndex];
    }

    public List<List<int>> GetAllCostList()
    {
        return _costList;
    }

    
    public void UnlockProduct()
    {
        switch (_currentCategory)
        {
            case 0:
                UserDataController.UnlockCellSkin(_currentProduct);
                break;
            case 1:
                UserDataController.UnlockExpositorSkin(_currentProduct);
                break;
            case 2:
                UserDataController.UnlockGroundSkin(_currentProduct);
                break;
            case 3:
                UserDataController.UnlockFrameSkin(_currentProduct);
                break;
        }
        _rewardManager.EarnCustomizeItem(_currentProduct, _currentCategory);
        _unlockButton.SetActive(false);
        _selectButton.SetActive(true);
        _customizeProductInstances[_currentProduct].Refresh();
    }

    public void GetMoreFragments()
    {
        switch (_currentCategory)
        {
            case 0:
                FindObjectOfType<BuyFragmentsController>().ShowInfo(FragmentType.CellFragments,_currentProduct,UserDataController.GetRemainingCellFragments(_currentProduct));
                break;
            case 1:
                FindObjectOfType<BuyFragmentsController>().ShowInfo(FragmentType.ExpositorFragments,_currentProduct, UserDataController.GetRemainingExpositorFragments(_currentProduct));
                break;
            case 2:
                FindObjectOfType<BuyFragmentsController>().ShowInfo(FragmentType.GroundFragments,_currentProduct, UserDataController.GetRemainingGroundFragments(_currentProduct));
                break;
        }
        
    }

    public void Unlock()
    {

    }

    public void SetProduct(int category, int productIndex)
    {
        switch (category)
        {
            case 0:
                UserDataController.SetCurrentCell(productIndex);
                break;
            case 1:
                UserDataController.SetCurrentExpositor(productIndex);
                break;
            case 2:
                UserDataController.SetCurrentGround(productIndex);
                break;
            case 3:
                UserDataController.SetCurrentFrame(productIndex);
                break;
        }
        _userSelectedProduct = productIndex;
        _cellManager.RefreshSprites();
    }

    public void OpenBigPanel(int categoryIndex)
    {
        string category = "";
        List<bool> unlockedItems = new List<bool>();
        _currentCategory = categoryIndex;
        _bgSelector.SetActive(true);
        _canBack = true;

        foreach (Transform t in _container)
        {
            _customizeProductInstances.Clear();
            Destroy(t.gameObject);
        }
        if (categoryIndex == 0)
        {
            _productsAmount = UserDataController.GetCellsSkins().Length;
            _userSelectedProduct = UserDataController.GetCurrentCell();
            unlockedItems = new List<bool>(UserDataController.GetCellsSkins());
            category = "Cells";
            _categoryText.text = LocalizationController.GetValueByKey("SEATS");
        }
        else if (categoryIndex == 1)
        {
            _productsAmount = UserDataController.GetExpositorsSkins().Length;
            _userSelectedProduct = UserDataController.GetCurrentExpositor();
            unlockedItems = new List<bool>(UserDataController.GetExpositorsSkins());
            category = "Expositors";
            _categoryText.text = LocalizationController.GetValueByKey("TABLES");
        }
        else if (categoryIndex == 2)
        {
            _productsAmount = UserDataController.GetGroundSkins().Length;
            _userSelectedProduct = UserDataController.GetCurrentGround();
            unlockedItems = new List<bool>(UserDataController.GetGroundSkins());
            category = "Grounds";
            _categoryText.text = LocalizationController.GetValueByKey("GROUNDS");
        }
        else if (categoryIndex == 3)
        {
            _customizeFrame.LoadConfig(3, UserDataController.GetCurrentFrame());
            _productsAmount = UserDataController.GetFramesSkins().Length;
            unlockedItems = new List<bool>(UserDataController.GetFramesSkins());
            _userSelectedProduct = UserDataController.GetCurrentFrame();
            category = "Frames";
            _categoryText.text = LocalizationController.GetValueByKey("FRAMES");
        }

        _customizeBigPhoto.LoadConfig(0, UserDataController.GetCurrentCell());
        _customizeBigPhoto.LoadConfig(1, UserDataController.GetCurrentExpositor());
        _customizeBigPhoto.LoadConfig(2, UserDataController.GetCurrentGround());
        CustomizeElementType cet = CustomizeElementType.Cell;
        switch (category)
        {
            case "Cells":
                cet = CustomizeElementType.Cell;
                break;
            case "Expositors":
                cet = CustomizeElementType.Expositor;
                break;

            case "Grounds":
                cet = CustomizeElementType.Ground;
                break;
        }
        for (int i = 0; i < _productsAmount; i++)
        {
            GameObject product = Instantiate(_productPrefab, _container);
            CustomizeProductInstance productCustomize = product.GetComponent<CustomizeProductInstance>();


            
            productCustomize.Init(cet,i, this, Resources.Load<Sprite>("Sprites/" + category + "/" + i));
            productCustomize.PreviewFeedback(false);
            productCustomize.SelectedFeedBack(false);
            _customizeProductInstances.Add(productCustomize);
        }
        _customizeProductInstances[_userSelectedProduct].PreviewFeedback(true);
        _customizeProductInstances[_userSelectedProduct].SelectedFeedBack(true);
        _container.sizeDelta = new Vector2((_container.childCount * 400f) + 100f, 310f);
        _fullProductPanel.SetActive(false);
        _backButton.SetActive(true);

        if (categoryIndex != 3) //Preguntamos si abre el de los marcos o el de los objetos customizables
        {
            _onePhotoCustomize.SetActive(true);
            _frameCustomize.SetActive(false);
            _customizeBigPhoto.LoadConfig(categoryIndex, _userSelectedProduct);
        }
        else
        {
            _frameCustomize.SetActive(true);
            _onePhotoCustomize.SetActive(false);
            _customizeFrame.LoadConfig(categoryIndex, _userSelectedProduct);
        }
        FastSelection();
        SelectProduct(0, cet);
    }

    public void Back()
    {
        _fullProductPanel.SetActive(true);
        _onePhotoCustomize.SetActive(false);
        _frameCustomize.SetActive(false);
        _backButton.SetActive(false);
        _bgSelector.SetActive(false);
        _canBack = false;
        RefreshInitialIcons();
    }

    public void SelectProduct(int productIndex, CustomizeElementType type)
    {
        _currentType = type;
        for(int i = 0; i<_customizeProductInstances.Count; i++)
        {
            _customizeProductInstances[i].PreviewFeedback(false);
        }
        _customizeProductInstances[productIndex].PreviewFeedback(true);

        if (_currentProduct != productIndex)
        {
            if(_currentCategory != 3)
            {
                _customizeBigPhoto.LoadConfig(_currentCategory, productIndex);
            }
            else
            {
                _customizeFrame.LoadConfig(_currentCategory, productIndex);
            }
            _currentProduct = productIndex;
            RectTransform parentRect = _container.parent.GetComponent<RectTransform>();
            StartCoroutine(MovePanel(_container.anchoredPosition, new Vector2(((parentRect.sizeDelta.x / 2f) - 240) - (productIndex * 360f), 0)));
        }   

        if(_currentProduct == _userSelectedProduct)
        {
            _selectButton.SetActive(false);
            _unlockButton.SetActive(false);
            _moreFragmentsButton.SetActive(false);

        }
        else
        {
            //Si el producto está desbloqueado, mostrar botón de seleccionar
            if (IsProductUnlocked(_currentCategory,_currentProduct))
            {
                _selectButton.SetActive(true);
                _unlockButton.SetActive(false);
                _moreFragmentsButton.SetActive(false);

            }
            else
            {
                //Si el producto no está desbloqueado, depende del número de fragmentos disponibles
                int currentFragments = 0;
                int maxFragments = 0;
                switch (type)
                {
                    case CustomizeElementType.Cell:
                        currentFragments = UserDataController.GetCellFragments(productIndex);
                        maxFragments = GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._cellSkins[productIndex]._rarity];
                        break;

                    case CustomizeElementType.Expositor:
                        currentFragments = UserDataController.GetExpositorFragments(productIndex);
                        maxFragments = GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._expositorSkins[productIndex]._rarity];
                        break;

                    case CustomizeElementType.Ground:
                        currentFragments = UserDataController.GetGroundFragments(productIndex);
                        maxFragments = GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._groundSkins[productIndex]._rarity];
                        break;
                }
                if (currentFragments>=maxFragments)
                {
                    //Si tiene suficientes fragmentos para desbloquear el producto muestro un botón para desbloquearlos
                    _moreFragmentsButton.SetActive(false);
                    _unlockButton.SetActive(true);
                }
                else
                {
                    //Si no los tiene,  
                    _unlockButton.SetActive(false);
                    _moreFragmentsButton.SetActive(true);
                }
                _selectButton.SetActive(false);
                //_priceText.text = GetHardCoinsProductCost(_currentCategory, _currentProduct).ToString();
            } 
        }
    }
    public void RefreshSelectedInfo()
    {
        if (_currentProduct == _userSelectedProduct)
        {
            _selectButton.SetActive(false);
            _unlockButton.SetActive(false);
            _moreFragmentsButton.SetActive(false);

        }
        else
        {
            //Si el producto está desbloqueado, mostrar botón de seleccionar
            if (IsProductUnlocked(_currentCategory, _currentProduct))
            {
                _selectButton.SetActive(true);
                _unlockButton.SetActive(false);
                _moreFragmentsButton.SetActive(false);

            }
            else
            {
                //Si el producto no está desbloqueado, depende del número de fragmentos disponibles
                int currentFragments = 0;
                int maxFragments = 0;
                switch (_currentType)
                {
                    case CustomizeElementType.Cell:
                        currentFragments = UserDataController.GetCellFragments(_currentProduct);
                        maxFragments = GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._cellSkins[_currentProduct]._rarity];
                        break;

                    case CustomizeElementType.Expositor:
                        currentFragments = UserDataController.GetExpositorFragments(_currentProduct);
                        maxFragments = GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._expositorSkins[_currentProduct]._rarity];
                        break;

                    case CustomizeElementType.Ground:
                        currentFragments = UserDataController.GetGroundFragments(_currentProduct);
                        maxFragments = GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._groundSkins[_currentProduct]._rarity];
                        break;
                }
                if (currentFragments >= maxFragments)
                {
                    //Si tiene suficientes fragmentos para desbloquear el producto muestro un botón para desbloquearlos
                    _moreFragmentsButton.SetActive(false);
                    _unlockButton.SetActive(true);
                }
                else
                {
                    //Si no los tiene,  
                    _unlockButton.SetActive(false);
                    _moreFragmentsButton.SetActive(true);
                }
                _selectButton.SetActive(false);
                //_priceText.text = GetHardCoinsProductCost(_currentCategory, _currentProduct).ToString();
            }
        }
    }
    IEnumerator MovePanel(Vector2 initPos, Vector2 targetPos)
    {    
        float duration = Mathf.Abs((targetPos - initPos).magnitude) * 0.25f / 360f;

        for(float i = 0; i<duration; i+= Time.deltaTime)
        {
            _container.anchoredPosition = Vector2.Lerp(initPos, targetPos, i/duration);
            yield return null;
        }
        _container.anchoredPosition = targetPos;
    }

    public void FastSelection()
    {
        RectTransform parentRect = _container.parent.GetComponent<RectTransform>();
        _container.anchoredPosition = new Vector2(((parentRect.sizeDelta.x / 2f) - 240) - (_userSelectedProduct * 360f), 0);
    }

    public void UseProduct()
    {
        _selectButton.SetActive(false);
        SetProduct(_currentCategory, _currentProduct);
        for(int i = 0; i<_customizeProductInstances.Count; i++)
        {
            _customizeProductInstances[i].SelectedFeedBack(false);
        }
        _customizeProductInstances[_currentProduct].SelectedFeedBack(true);
    }

    public bool IsProductUnlocked(int category, int productIndex)
    {
        bool unlocked = false;

        switch (category)
        {
            case 0:
                if (UserDataController.GetCellsSkins()[productIndex])
                    unlocked = true;
                break;
            case 1:
                if (UserDataController.GetExpositorsSkins()[productIndex])
                    unlocked = true;
                break;
            case 2:
                if (UserDataController.GetGroundSkins()[productIndex])
                    unlocked = true;
                break;
            case 3:
                if (UserDataController.GetFramesSkins()[productIndex])
                    unlocked = true;
                break;
        }
        return unlocked;
    }

    public bool CanBack()
    {
        return _canBack;
    }
}
