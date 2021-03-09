using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;
public class VipController : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel, _vipButton, _unlockedVip, _vipPanel, _infoPanel;
    PanelManager _panelManager;
    RewardManager _rewardManager;
    EconomyManager _economyManager;
    [SerializeField]
    Material _vipMaterial;
    [SerializeField]
    GameObject _vipParticles;
    TimeSpan _diff;
    UpgradesManager _upgradesManager;
    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
        _economyManager = FindObjectOfType<EconomyManager>();
        _upgradesManager = FindObjectOfType<UpgradesManager>();
        if (UserDataController.IsVipUser())
        {
            _vipMaterial.SetFloat("Bright", 1f);
            _vipParticles.SetActive(true);
            _unlockedVip.SetActive(true);
            _infoPanel.SetActive(true);
            _vipPanel.SetActive(false);
        }
        else
        {
            _vipMaterial.SetFloat("Bright", 0f);
            _vipParticles.SetActive(false);
            _unlockedVip.SetActive(false);
            _infoPanel.SetActive(false);
            _vipPanel.SetActive(true);
            RequestShowVip();
        }
    }

    public void OpenVip()
    {
        _panelManager.RequestShowPanel(_mainPanel);
    }

    public void CloseVip()
    {
        _panelManager.ClosePanel();
    }
   
    public void VipPurchase()
    {
        if (_economyManager.SpendHardCoins(2000))
        {
            _rewardManager.EarnHardCoin(75);
            _rewardManager.EarnSpeedUp(400);
            _rewardManager.EarnLootBox(2, 1);
            UserDataController.ActiveVip();
            UserDataController.SetFreeSpinTries(2);
            _upgradesManager.CheckVip();
            _vipMaterial.SetFloat("Bright", 1f);
            _vipButton.SetActive(false);
            _vipParticles.SetActive(true);
            _unlockedVip.SetActive(true);
            _infoPanel.SetActive(true);
            _vipPanel.SetActive(false);
            CloseVip();
        }
    }

    public void VipRestore()
    {
        _rewardManager.EarnHardCoin(500);
        _rewardManager.EarnSpeedUp(400);
        _rewardManager.EarnLootBox(2, 1);
        UserDataController.ActiveVip();
        UserDataController.SetFreeSpinTries(2);
        _upgradesManager.CheckVip();
        _vipMaterial.SetFloat("Bright", 1f);
        _vipButton.SetActive(false);
        _vipParticles.SetActive(true);
        _unlockedVip.SetActive(true);
        _infoPanel.SetActive(true);
        _vipPanel.SetActive(false);
    }

    private void Update()
    {
        if (UserDataController.IsVipUser())
        {
            _diff = UserDataController.GetVipExpireDate().Subtract(DateTime.Now);
            if (UserDataController.GetVipExpireDate() < DateTime.Now)
            {
                UserDataController.VipExpire();
                _vipMaterial.SetFloat("Bright", 0f);
                _vipParticles.SetActive(false);
                _unlockedVip.SetActive(false);
                _vipButton.SetActive(true);
                _infoPanel.SetActive(false);
                _vipPanel.SetActive(true);
                CloseVip();
            }
        }
    }

    public string GetVipTime()
    {
        string fText = (_diff.Days.ToString("00") + "d|" + _diff.Hours.ToString("00") + "h|" + _diff.Minutes.ToString("00") + "m|" + _diff.Seconds.ToString("00") + "s");
        return fText;
    }

    public void RequestShowVip()
    {
        if (UserDataController.GetBiggestDino() >= 4)
        {
            StartCoroutine(ShowVip());
        }
    }

    IEnumerator ShowVip()
    {
        yield return new WaitForSeconds(0.01f);
        OpenVip();
    }
}
