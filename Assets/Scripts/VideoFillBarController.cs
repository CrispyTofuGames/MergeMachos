using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoFillBarController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _txInfo, _txReward1, _txReward2, _txReward3;
    [SerializeField]
    Image _fillMask;
    [SerializeField]
    EconomyManager _economyManager;
    public void RefreshInfo()
    {
        string info = string.Format(LocalizationController.GetValueByKey("NEXT_REWARD"), UserDataController.GetWatchedVideos());
        _txInfo.text = info;
        GameCurrency bGameC = _economyManager.GetTotalEarningsPerSecond();
        bGameC.MultiplyCurrency(7200);
        _txReward1.text = bGameC.GetCurrentMoney();
        bGameC.MultiplyCurrency(2);
        _txReward2.text = bGameC.GetCurrentMoney();
        bGameC.MultiplyCurrency(1.5f);
        _txReward3.text = bGameC.GetCurrentMoney();
        _fillMask.fillAmount = UserDataController.GetWatchedVideos() / 6f;
    }
}
