using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    AnimationCurve animationCurve;
    [SerializeField]
    BoxManager _boxManager;
    [SerializeField]
    EconomyManager _economyManager;
    PanelManager _panelManager;
    RewardManager _rewardManager;

    private void Awake()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
    }
    public void OpenNest()
    {
        _panelManager.RequestShowPanel(_mainPanel);
    }
    public void CloseNest()
    {
        _panelManager.ClosePanel();
    }
    public void HardCoinPurchase()
    {
        if (_economyManager.SpendHardCoins(3))
        {
            _rewardManager.EarnGifts(4);
            //_boxManager.RewardBox(4);
        }
        CloseNest();
    }
}