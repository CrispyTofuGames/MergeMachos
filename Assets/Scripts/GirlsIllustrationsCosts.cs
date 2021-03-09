using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlsIllustrationsCosts : MonoBehaviour
{
    static List<GameCurrency> _illustrationCosts;

    private void Awake()
    {
        GameCurrency _mekuCost = new GameCurrency(new int[] { 0, 0, 1, 1 });
        GameCurrency _mekuCostNude = new GameCurrency(new int[] { 0, 0, 1, 2 });
        GameCurrency _eriCost = new GameCurrency(new int[] { 0, 0, 1, 3 });
        GameCurrency _eriCostNude = new GameCurrency(new int[] { 0, 0, 1, 4 });
        GameCurrency _pinkyCost = new GameCurrency(new int[] { 0, 0, 1, 6 });
        GameCurrency _pinkyCostNude = new GameCurrency(new int[] { 0, 0, 2, 0 });
        _illustrationCosts = new List<GameCurrency>(){ new GameCurrency(0), new GameCurrency(0), new GameCurrency(55000), new GameCurrency(115000), new GameCurrency(555000), new GameCurrency(1115000), new GameCurrency(5555000), new GameCurrency(11115000), new GameCurrency(55555000), new GameCurrency(111115000), new GameCurrency(211115000), new GameCurrency(311115000), new GameCurrency(411115000), new GameCurrency(511115000), new GameCurrency(_mekuCost.GetIntList()), new GameCurrency(_mekuCostNude.GetIntList()), new GameCurrency(_eriCost.GetIntList()), new GameCurrency(_eriCostNude.GetIntList()), new GameCurrency(_pinkyCost.GetIntList()), new GameCurrency(_pinkyCostNude.GetIntList()) };
    }
}
