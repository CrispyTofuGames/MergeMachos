using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopChest : MonoBehaviour
{
    EconomyManager _economyManager;
    RewardManager _rewardManager;
    [SerializeField] int offer;
    private void Start()
    {
        _economyManager = FindObjectOfType<EconomyManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
    }
    public void PurchaseChest()
    {
        switch (offer)
        {
            case 0:

                if (_economyManager.SpendHardCoins(25))
                {
                    _rewardManager.EarnLootBox(0, 1);
                }
                
                break;

            case 1:
                if (_economyManager.SpendHardCoins(50))
                {
                    _rewardManager.EarnLootBox(1, 1);
                }
                break;

            case 2:
                if (_economyManager.SpendHardCoins(100))
                {
                    _rewardManager.EarnLootBox(2, 1);
                }
                break;

            case 3:
                if (_economyManager.SpendHardCoins(200))
                {
                    _rewardManager.EarnLootBox(3, 1);
                }
                break;
            case 4:
                if (_economyManager.SpendHardCoins(400))
                {
                    _rewardManager.EarnLootBox(2, 5);
                }
                break;

            case 5:
                if (_economyManager.SpendHardCoins(750))
                {
                    _rewardManager.EarnLootBox(3, 5);
                }
                break;
        }
        
    }
}
