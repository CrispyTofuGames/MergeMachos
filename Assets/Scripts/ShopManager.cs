using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    PanelManager _panelManager;
    List<int> _gemRewards = new List<int>() { 40, 360, 840, 1920, 5100, 11400 };
    List<int> _realCosts = new List<int>() { 100, 449, 999, 1999, 4999, 9999 };
    List<int> _coinRewardSeconds = new List<int>() { 28800, 115200, 806400 };
    List<int> _coinGemCost = new List<int>() { 85, 399, 999 };
    [SerializeField]
    GameObject[] _gemProducts;
    [SerializeField]
    GameObject[] _coinProducts;
    EconomyManager _economyManager;
    RewardManager _rewardManager;
    [SerializeField]
    RectTransform _gridRectTransform;
    bool isOpen = false;
    [SerializeField]
    GameObject _confirmPurchase, _unlockedOfferPanel, _purchaseNull;
    NutakuPurchaseController _nutakuPurchaseController;
    int _specialOfferIndex = 1;

    public void OpenShop()
    {
        if (CurrentSceneManager._canOpenShop)
        {
            if (!_mainPanel.activeSelf)
            {
                _gridRectTransform.anchoredPosition = Vector3.zero;
                if (_panelManager.GetPanelState())
                {
                    _panelManager.ClosePanel();
                }
                _panelManager.RequestShowPanel(_mainPanel);
                RefreshCoinPanels();
                if (UserDataController.CheckSpecialOffer())
                {
                    _unlockedOfferPanel.SetActive(true);
                    _purchaseNull.SetActive(false);
                }
            }
        }
    }
    public void CloseShop()
    {
        _panelManager.ClosePanel();
    }

    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        _economyManager = FindObjectOfType<EconomyManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
        _nutakuPurchaseController = FindObjectOfType<NutakuPurchaseController>();

        for (int i = 0; i<_gemProducts.Length; i++)
        { 
            _gemProducts[i].GetComponent<ShopProductInstance>().Init(_gemRewards[i].ToString(), _realCosts[i], i, true, this);
        }
        RefreshCoinPanels();
    }
    public void Purchase(bool isGem, int index)
    {
        if (isGem)
        {
            _nutakuPurchaseController.RequestPayment(index, _realCosts[index], _gemRewards[index]);
        }
        else
        {
            if (_economyManager.SpendHardCoins(_coinGemCost[index]))
            {
                _rewardManager.EarnSoftCoin(_coinRewardSeconds[index]);
            }
        }
    }

    public void RefreshCoinPanels()
    {
        for(int i = 0; i<_coinProducts.Length; i++)
        {
            GameCurrency baseRewardPSec;
            baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
            baseRewardPSec.MultiplyCurrency(_coinRewardSeconds[i]);
            _coinProducts[i].GetComponent<ShopProductInstance>().Init(baseRewardPSec.GetCurrentMoney(), _coinGemCost[i], i, false, this);
        }
    }

    public void SpecialOffer()
    {
        FindObjectOfType<VipController>().OpenVip();
        CloseShop();
        //_confirmPurchase.SetActive(true);
    }

    public bool PackOffer(int packIndex)
    {
        bool canPurchase = false;
        if (_economyManager.SpendHardCoins(200))
        {
            UserDataController.UnlockPack(packIndex);
            _rewardManager.EarnCustomizeItem(packIndex, 0);
            _rewardManager.EarnCustomizeItem(packIndex, 1);
            _rewardManager.EarnCustomizeItem(packIndex, 2);
            canPurchase = true;
            if(packIndex == _specialOfferIndex) //Por si coincidiese que promocionamos un pack que tambien esta en la tienda y el jugador lo ha comprado
            {
                UserDataController.PurchaseSpecialOffer(_specialOfferIndex);
                _unlockedOfferPanel.SetActive(true);
                _purchaseNull.SetActive(false);
            }
        }
        return canPurchase;
    }

    public void ConfirmSpecialOffer()
    {
        if (_economyManager.SpendHardCoins(150))
        {
            UserDataController.PurchaseSpecialOffer(_specialOfferIndex); //Cambiamos estos 1 en funcion del paquete que esté en promoción
            _rewardManager.EarnCustomizeItem(_specialOfferIndex, 0);
            _rewardManager.EarnCustomizeItem(_specialOfferIndex, 1);
            _rewardManager.EarnCustomizeItem(_specialOfferIndex, 2);
            _unlockedOfferPanel.SetActive(true);
            _purchaseNull.SetActive(false);
            CloseConfirmPurchase();
        }
    }

    public void CloseConfirmPurchase()
    {
        _confirmPurchase.SetActive(false);
    }
}
