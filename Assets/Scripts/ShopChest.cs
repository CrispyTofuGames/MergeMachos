using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopChest : MonoBehaviour
{
    EconomyManager _economyManager;
    ShopManager _shopManager;
    RewardManager _rewardManager;
    int _offerIndex;
    int[] _chestPrices = new int[]{25,50,100,200,400,750};
    private void Start()
    {
        _economyManager = FindObjectOfType<EconomyManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
        _shopManager = FindObjectOfType<ShopManager>();
    }
    public void PurchaseChest()
    {
        switch (_offerIndex)
        {
            case 0:
                if (_economyManager.SpendHardCoins(_chestPrices[_offerIndex]))
                {
                    _rewardManager.EarnLootBox(0, 1);
                }
                break;

            case 1:
                if (_economyManager.SpendHardCoins(_chestPrices[_offerIndex]))
                {
                    _rewardManager.EarnLootBox(1, 1);
                }
                break;

            case 2:
                if (_economyManager.SpendHardCoins(_chestPrices[_offerIndex]))
                {
                    _rewardManager.EarnLootBox(2, 1);
                }
                break;

            case 3:
                if (_economyManager.SpendHardCoins(_chestPrices[_offerIndex]))
                {
                    _rewardManager.EarnLootBox(3, 1);
                }
                break;
            case 4:
                if (_economyManager.SpendHardCoins(_chestPrices[_offerIndex]))
                {
                    _rewardManager.EarnLootBox(2, 5);
                }
                break;

            case 5:
                if (_economyManager.SpendHardCoins(_chestPrices[_offerIndex]))
                {
                    _rewardManager.EarnLootBox(3, 5);
                }
                break;
        }      
    }

    public void CheckPurchase(int offerIndex)
    {
        _offerIndex = offerIndex;
        _shopManager.OpenConfirmPanel(_chestPrices[_offerIndex], this);
    }
}
