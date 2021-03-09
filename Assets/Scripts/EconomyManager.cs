using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    List<DinosaurInstance> _dinosInGame;
    List<GameCurrency> _earningsByType = new List<GameCurrency>() { new GameCurrency(2), new GameCurrency(4), new GameCurrency(10), new GameCurrency(20), 
        new GameCurrency(42), new GameCurrency(90), new GameCurrency(190), new GameCurrency(403), new GameCurrency(851), new GameCurrency(1756), 
        new GameCurrency(3245), new GameCurrency(8007), new GameCurrency(16432), new GameCurrency(35411), new GameCurrency(75687), 
        new GameCurrency(159543), new GameCurrency(314514), new GameCurrency(674848), new GameCurrency(1426677), new GameCurrency(3013439), 
        new GameCurrency(6370677), new GameCurrency(new int[]{354,455,13}), new GameCurrency(new int[] { 354, 741, 28}), new GameCurrency(new int[] { 183, 88, 60}), 
        new GameCurrency(new int[] { 649, 281, 126}), new GameCurrency(new int[] { 613, 332, 268}), new GameCurrency(new int[] { 649, 281, 567}),
        new GameCurrency(new int[] { 835, 312, 198, 1}), new GameCurrency(new int[] { 312, 677, 532, 2}), new GameCurrency(new int[] { 912, 281, 352, 5}),
        new GameCurrency(new int[] { 166, 307, 310, 11}),  new GameCurrency(new int[] { 321, 433, 853, 23}), new GameCurrency(new int[] { 523, 153, 639, 48}), new GameCurrency(new int[] { 689, 553, 123, 97})};

    List<int> _initialCostList; 
    float _initialCostIncrementRatio = 3.092f;
    float _firstDinoIncrementRatio = 1.07f;
    float _dinoIncrementRatio = 1.157f;
    int initialCost0 = 100;
    int initialCost1 = 1500;
    [SerializeField]
    GoShopController _goShopController;
    UpgradesManager _upgradesManager;
    private void Awake()
    {
        _initialCostList = new List<int>() {100, 1500};
        _upgradesManager = FindObjectOfType<UpgradesManager>();
        float firstDinoCost = _initialCostList[1];
        for (int i = 0; i < 10; i++)
        {
            firstDinoCost *= _initialCostIncrementRatio;
            _initialCostList.Add((int)firstDinoCost);
        }
    }
    public void EarnSoftCoins(GameCurrency softCoins)
    {
        UserDataController.AddSoftCoins(softCoins);
    }

    public void SetDinosInGame(List<DinosaurInstance> dinosInGame)
    {
        _dinosInGame = dinosInGame;
    }

    public void EarnHardCoins(int amount)
    {
        UserDataController.AddHardCoins(amount);
    }

    public bool SpendHardCoins(int amount)
    {
        bool canSpend = false;
        if(UserDataController.GetHardCoins() >= amount)
        {
            canSpend = true;
            UserDataController.SpendHardCoins(amount);
        }
        else
        {
            GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("ADVICE_NOHARDCOINS", ""));
            _goShopController.OpenShopQuest();
        }
        return canSpend;
    }

    public bool SpendSoftCoins(GameCurrency amount)
    {
        bool canSpend = false;
        GameCurrency g = UserDataController.GetSoftCoins();
        if (g.SubstractCurrency(amount))
        {
            UserDataController.SpendSoftCoins(amount);
            canSpend = true;
        }
        else
        {
            GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("ADVICE_NOSOFTCOINS"));
        }
        return canSpend;
    }

    public string GetEarningsPerSecond()
    {
        GameCurrency earningsPerSecond = new GameCurrency();
        foreach(DinosaurInstance d in _dinosInGame)
        {
            if (d.IsWorking())
            {
                earningsPerSecond.AddCurrency(_earningsByType[d.GetDinosaur()]);
            }
        }
        earningsPerSecond.MultiplyCurrency(1 + _upgradesManager.GetExtraTouristSpeed()/100f);

        if (_upgradesManager.GetExtraEarnings() > 0)
        {
            earningsPerSecond.MultiplyCurrency(1f + (_upgradesManager.GetExtraEarnings() / 100f));
        }
        return earningsPerSecond.GetCurrentMoney() + "/sec";
    }
    public GameCurrency GetTotalEarningsPerSecond()
    {
        GameCurrency earningsPerSecond = new GameCurrency();
        foreach (DinosaurInstance d in _dinosInGame)
        {
            earningsPerSecond.AddCurrency(_earningsByType[d.GetDinosaur()]);       
        }
        return earningsPerSecond;
    }

    public GameCurrency GetEarningsByType(int dinoType)
    {
        GameCurrency g = new GameCurrency(_earningsByType[dinoType].GetIntList());
        if (_upgradesManager.GetExtraEarnings() > 0)
        {
            g.MultiplyCurrency(1f + (_upgradesManager.GetExtraEarnings() / 100f));
        }

        return g;
    }

    public GameCurrency GetDinoCost(int dinoType)
    {
        int ownedDinos = UserDataController.GetOwnedDinosByDinoType(dinoType);
        GameCurrency dinoCurrency = new GameCurrency();
        if(dinoType == 0)
        {
            dinoCurrency = GetInitialCost(dinoType);
            dinoCurrency.MultiplyCurrency(Mathf.Pow(_firstDinoIncrementRatio, ownedDinos));
        }
        else
        {
            if(dinoType >= 1)
            {
                dinoCurrency = GetInitialCost(dinoType);
                dinoCurrency.MultiplyCurrency(Mathf.Pow(_dinoIncrementRatio, ownedDinos));
            }
        }
        if(_upgradesManager.GetDiscount() > 0)
        {
            dinoCurrency.MultiplyCurrency(1f - (_upgradesManager.GetDiscount() / 100f));
        }
        return dinoCurrency;
    }

    public GameCurrency GetInitialCost(int dinoType)
    {
        if(dinoType == 0)
        {
            return new GameCurrency(initialCost0);
        }
        else
        {
            if (dinoType == 1)
            {
                return new GameCurrency(initialCost1);
            }
            else
            {
                GameCurrency finalInitialCost = GetInitialCost(dinoType - 1);
                finalInitialCost.MultiplyCurrency(_initialCostIncrementRatio);
                return finalInitialCost;
            }
        }
    }
}
