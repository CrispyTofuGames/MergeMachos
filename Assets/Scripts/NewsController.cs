using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewsController : MonoBehaviour
{
    [SerializeField]
    RewardManager _rewardManager;
    [SerializeField]
    GameObject _mainPanel, _vipPanel, _passivePanel, _framesPanel;
    [SerializeField]
    Button[] _compensationButtons;
    [SerializeField]
    PanelManager _panelManager;
    [SerializeField]
    VipController _vipController;
    int _currentRewardIndex = 0;
    int _unlockedFrames = 0;
    bool _waitingVip, _waitingPassive;

    private void Start()
    {
        StartCoroutine(WaitForCheck());
    }

    public void OpenNews()
    {
        _panelManager.RequestShowPanel(_mainPanel);
    }

    public void CloseNews()
    {
        _panelManager.ClosePanel();
    }

    public void ClaimGems()
    {
        _rewardManager.EarnHardCoin(100);
        CloseNews();
    }

    public void OpenCompensation(GameObject panel)
    {
        _panelManager.RequestShowPanel(panel);
        foreach(Button b in _compensationButtons)
        {
            b.interactable = false;
        }
        StartCoroutine(WaitForClaim());
    }

    IEnumerator WaitForClaim()
    {
        yield return new WaitForSeconds(4f);
        foreach (Button b in _compensationButtons)
        {
            b.interactable = true;
        }
    }

    IEnumerator WaitForCheck()
    {
        yield return new WaitForSeconds(0.5f);
        if (UserDataController.IsVipUser())
        {
            //UserDataController._currentUserData._compensationVip = false;
            if (!UserDataController.GetCompensationVipState())
            {
                _waitingVip = true;
                OpenCompensation(_vipPanel);
            }
        }
        else
        {
            //if (!UserDataController.GetCompensationVipState())
            //{
            //    bool hasEverySkin = false;
            //    for (int i =0; i < UserDataController.GetSkinNumber();i++)
            //    {

            //    }
            //    if (finded)
            //    {
            //        int countOfSpecials = 0;
            //        for (int i = maxSpecial - 1; i >= 0; i--)
            //        {
            //            if (UserDataController.IsSkinUnlocked(i))
            //            {
            //                countOfSpecials++;
            //            }
            //        }
            //        if (countOfSpecials == maxSpecial)
            //        {
            //            if (countOfSpecials >= 8)
            //            {
            //                OpenCompensation(_vipPanel);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    UserDataController.ClaimCompensationVip();
            //    
            //}
            _waitingVip = false;
        }
        while (_waitingVip)
        {
            yield return null;
        }
        if (UserDataController.GetBiggestDino() > 4 && !UserDataController.GetCompensationPassiveEarningsState())
        {
            OpenCompensation(_passivePanel);
            _waitingPassive = true;
        }
        else
        {
            UserDataController.ClaimCompensationPassiveEarnings();
            _waitingPassive = false;
        }
        while (_waitingPassive)
        {
            yield return null;
        }
        if (!UserDataController.GetCompensationFramesState())
        {
            for (int i = 1; i < UserDataController.GetFramesSkins().Length; i++)
            {
                if (UserDataController.GetFramesSkins()[i])
                {
                    _unlockedFrames++;
                }
            }
            if (_unlockedFrames > 0)
            {
                OpenCompensation(_framesPanel);
            }
            else
            {
                UserDataController.ClaimCompensationFrames();
            }
        }
    }

    public void ClaimReward(int rewardIndex)
    {
        if(rewardIndex == 0)
        {
            _vipController.VipRestore();
            UserDataController.ClaimCompensationVip();
            _waitingVip = false;
        }
        if (rewardIndex == 1)
        {
            _rewardManager.EarnSoftCoin(86400);
            UserDataController.ClaimCompensationPassiveEarnings();
            _waitingPassive = false;
        }
        if (rewardIndex == 2)
        {
            _rewardManager.EarnHardCoin(75 * _unlockedFrames);
            UserDataController.ClaimCompensationFrames();
        }
        _panelManager.ClosePanel();
    }
}
