using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementInstance : MonoBehaviour
{
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
    RewardManager _rewardManager;
    MissionsManager _missionManager;

    int hardCoinsAmount = 0;
    int dinoLevel = 0;
    int _targetProgress;
    int achievementID = 0;

    private void Start()
    {
        _rewardManager = FindObjectOfType<RewardManager>();
    }
    public void SetMissionInstance(int id, string title, int currentProgress, int targetProgress, Sprite rewardIcon, int rewardAmount, int dinoLvl, MissionsManager m)
    {
        _missionManager = m;
        Refresh();
        _rewardAmountTx.text = "x" + rewardAmount;
        _titleTx.text = title;
        _progressTx.text = currentProgress + "/" + targetProgress;
        _progressFill.fillAmount = currentProgress / (float)targetProgress;
        _rewardIcon.sprite = rewardIcon;
        _rewardIcon.overrideSprite = rewardIcon;
        hardCoinsAmount = rewardAmount;
        _targetProgress = targetProgress;
        dinoLevel = dinoLvl;
        achievementID = id;
    }

    private void OnEnable()
    {
        Refresh();
    }
    public void Refresh()
    {
        int currentProgress = UserDataController.GetObtainedDinosByDinotype(dinoLevel);
        if(_targetProgress > 0)
        {
            if (!UserDataController.GetClaimedAchievement(achievementID))
            {
                if (currentProgress >= _targetProgress)
                {
                    currentProgress = _targetProgress;
                    _claimButton.SetActive(true);
                    UserDataController.SetchievementToClaim(achievementID, true);
                }
                else
                {
                    _claimButton.SetActive(false);
                }
            }
        }

        _progressTx.text = currentProgress + "/" + _targetProgress;
        _progressFill.fillAmount = currentProgress / (float)_targetProgress;
    }

    public void ClaimReward()
    {
        _rewardManager.EarnHardCoin(hardCoinsAmount);
        UserDataController.ClaimAchievement(achievementID);
        _missionManager.CheckWarningState();
        gameObject.SetActive(false);
    }
}
