using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyRewardManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    Image _blackBackgroundImage;
    PanelManager _panelManager;
    [SerializeField]
    RectTransform[] _dailyRewards;
    RewardManager _rewardManager;
    [SerializeField]
    TextMeshProUGUI _rewardTx1, _rewardTx2, _rewardTx3;
    EconomyManager _economyManager;

    private void Awake()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        _economyManager = FindObjectOfType<EconomyManager>();
        GameEvents.RewardMergeUp.AddListener(DinoUpCallback);
        _rewardManager = FindObjectOfType<RewardManager>();
    }
    private void Start()
    {
        DateTime lastDay = UserDataController.GetLastPlayedDay();
        DateTime today = System.DateTime.Now;
        if(lastDay.Day != today.Day || lastDay.Month != today.Month)
        {
            OpenPanel();
            UserDataController.RestoreDailyMissions();
        }

    }
    public void DinoUpCallback(int dino)
    {
        //if (dino == 6)
        //{
        //    OpenPanel();
        //}
    }
    public void CloseDaily()
    {
        _panelManager.ClosePanel();
    }
    public void OpenPanel()
    {
        GameCurrency baseRewardPSec;
        baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
        baseRewardPSec.MultiplyCurrency(3600);
        _rewardTx1.text = baseRewardPSec.GetCurrentMoneyConvertedTo3Chars();
        baseRewardPSec.MultiplyCurrency(2);
        _rewardTx2.text = baseRewardPSec.GetCurrentMoneyConvertedTo3Chars();
        baseRewardPSec.MultiplyCurrency(2);
        _rewardTx3.text = baseRewardPSec.GetCurrentMoneyConvertedTo3Chars();

        int playedDays = UserDataController.GetPlayedDays();
        MarkButton(playedDays);
        _panelManager.RequestShowPanel(_mainPanel.gameObject);
    }
    public void ObtainReward(int rewardDay)
    {
        switch (rewardDay)
        {
            case 0:
                _rewardManager.EarnSpeedUp(200);
                _rewardManager.EarnLootBox(1, 1);
                break;
            case 1:
                _rewardManager.EarnSoftCoin(3600);
                _rewardManager.EarnLootBox(2, 1);
                break;
            case 2:
                _rewardManager.EarnSpeedUp(400);
                _rewardManager.EarnLootBox(1, 1);
                break;
            case 3:
                _rewardManager.EarnSoftCoin(7200);
                _rewardManager.EarnLootBox(1, 1);
                break;
            case 4:
                _rewardManager.EarnSpeedUp(600);
                _rewardManager.EarnLootBox(1, 1);
                break;
            case 5:
                _rewardManager.EarnSoftCoin(14400);
                _rewardManager.EarnLootBox(1, 1);
                break;
            case 6:
                _rewardManager.EarnHardCoin(50);
                _rewardManager.EarnLootBox(3, 1);
                break;
        }
        UserDataController.AddPlayedDay();

        if (UserDataController.IsVipUser())
        {
            _rewardManager.EarnHardCoin(75);
            _rewardManager.EarnSpeedUp(400);
            _rewardManager.EarnLootBox(2, 1);
        }
        CloseDaily();
    }

    public void MarkButton(int buttonIndex)
    {
        for(int i = 0; i<_dailyRewards.Length; i++)
        {
            Button dailyButton = _dailyRewards[i].GetComponentInChildren<Button>();
            TextMeshProUGUI dailyText = _dailyRewards[i].GetComponentInChildren<TextMeshProUGUI>();
            string finalTx = string.Format(LocalizationController._localizedData["DAILY_REWARD_DAY"], (i+1).ToString());
            dailyText.text = finalTx;

            if (buttonIndex == i)
            {
                _dailyRewards[i].gameObject.GetComponent<DailyRewardInstance>().SelectedConfig();
                dailyButton.interactable = true;
            }
            else
            {
                if (i < buttonIndex)
                {
                    _dailyRewards[i].gameObject.GetComponent<DailyRewardInstance>().UsedConfig();
                    dailyButton.interactable = false;
                }
                else
                {
                    _dailyRewards[i].gameObject.GetComponent<DailyRewardInstance>().BasicConfig();
                    dailyButton.interactable = false;
                }
            }
        }
    }
}