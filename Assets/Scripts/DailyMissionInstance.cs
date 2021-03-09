using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyMissionInstance : MonoBehaviour
{
    [SerializeField]
    int _missionType;
    bool state = false;
    [SerializeField]
    TextMeshProUGUI _titleTx;
    [SerializeField]
    TextMeshProUGUI _progressTx;
    [SerializeField]
    Image _progressFill;
    [SerializeField]
    Image _rewardIcon;
    [SerializeField]
    TextMeshProUGUI _rewardAmountTx;
    [SerializeField]
    GameObject _claimButton;
    [SerializeField]
    RewardManager _rewardManager;
    [SerializeField]
    EconomyManager _economyManager;
    [SerializeField]
    MissionsManager _missionManager;

    int level = 0;

    void OnEnable()
    {
        Refresh();
    }

    public void ClaimMergeReward()
    {
        _rewardManager.EarnSoftCoin(100 * (level + 1));
        UserDataController.AddDailyMergeLevel();
        state = false;
        Refresh();
        _missionManager.CheckWarningState();
    }
    public void ClaimAdReward()
    {
        _rewardManager.EarnHardCoin(3*level);
        UserDataController.AddDailySkinLevel();
        state = false;
        Refresh();
        _missionManager.CheckWarningState();
    }
    public void ClaimPurchaseReward()
    {
        _rewardManager.EarnSoftCoin(200 * (level + 1));
        UserDataController.AddDailyPurchaseLevel();
        state = false;
        Refresh();
        _missionManager.CheckWarningState();
    }

    public bool GetState()
    {
        Refresh();
        return state;
    }

    public void Refresh()
    {
        string title = "";
        int currentProgress = 0;
        int target = 0;

        switch (_missionType)
        {
            case 0://merge
                level = UserDataController.GetDailyMergeLevel();
                target = 10 + (10 * level);
                title = string.Format(LocalizationController.GetValueByKey("DAILYMISSION_MERGE"), target);
                currentProgress = UserDataController.GetDailyMerges();
                GameCurrency baseRewardPSec;
                baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
                baseRewardPSec.MultiplyCurrency(100 * (level + 1));           
                _rewardAmountTx.text = "x " + baseRewardPSec.GetCurrentMoneyConvertedTo3Chars();
                break;
            case 1://AD
                level = UserDataController.GetDailySkinLevel();
                target = 5 + (5 * level);
                title = string.Format(LocalizationController.GetValueByKey("DAILYMISSION_MERGE"), target);
                currentProgress = UserDataController.GetUnlockedSkinsNumber();
                break;
            case 2://Purchase
                level = UserDataController.GetDailyPurchaseLevel();
                target = 10 + (10 * level);
                title = string.Format(LocalizationController.GetValueByKey("DAILYMISSION_PURCHASE"), target);
                currentProgress = UserDataController.GetDailyPurchases();
                GameCurrency bigBaseRewardPSec;
                bigBaseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
                bigBaseRewardPSec.MultiplyCurrency(200 * (level +1));
                _rewardAmountTx.text = "x "+ bigBaseRewardPSec.GetCurrentMoneyConvertedTo3Chars();
                break;
        }
        if (currentProgress >= target)
        {
            currentProgress = target;
            _claimButton.SetActive(true);
            state = true;
        }
        else
        {
            _claimButton.SetActive(false);
            state = false;
        }
        _titleTx.text = title;
        _progressFill.fillAmount = currentProgress / (float)target;
        _progressTx.text = currentProgress + "/" + target;
    }
}
