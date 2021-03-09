using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SpeedUpManager : MonoBehaviour
{
    [SerializeField]
    GameObject _speedUpMain;
    [SerializeField]
    TextMeshProUGUI _remainingTimeTx;
    [SerializeField]
    RectTransform _speedUpButton;
    [SerializeField]
    Button _hardCoinsPurchaseButton;
    [SerializeField]
    Transform _adButton;
    [SerializeField]
    RectTransform _activeFeedbackPanel;
    [SerializeField]
    TextMeshProUGUI _activeTime;
    [SerializeField]
    AnimationCurve animationCurve;
    [SerializeField]
    VFXFireworksPool _VFXFireworksPool;
    [SerializeField]
    TextMeshProUGUI _hardCoinsPurchaseText;
    [SerializeField]
    float _speedUpTime = 0;
    bool _speedingUp = false;
    bool _panelIsOpen = false;
    [SerializeField]
    Image _timeFillBar;
    PanelManager _panelManager;
    int _speedHardCoinsCost = 3;
    EconomyManager _economyManager;
    RewardManager _rewardManager;
    float _happyHourSpeed = 1;

    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
        DateTime _lastSpeedUp = UserDataController.GetLastSpeedUpTime();
        int remainingSecs = (int)System.DateTime.Now.Subtract(_lastSpeedUp).TotalSeconds;
        _economyManager = FindObjectOfType<EconomyManager>();
        int finalTime = UserDataController.GetSpeedUpRemainingSecs() - remainingSecs;

        if (finalTime > 0)
        {
            SpeedUpCallback(finalTime);
        }
    }
    public void SetSpeedHardCoinsCost(int newCost)
    {
        _speedHardCoinsCost = newCost;
        _hardCoinsPurchaseText.text = newCost.ToString();
    }
    public void OpenSpeedUpPanel()
    {
        _panelManager.RequestShowPanel(_speedUpMain);
        _panelIsOpen = true;
    }
    public Vector3 GetHardCoinsButtonPosition()
    {
        return _hardCoinsPurchaseButton.transform.position;
    }
    public void CloseSpeedUpPanel()
    {
        _panelManager.ClosePanel();
        _panelIsOpen = false;
    }

    public void SpeedUpCallback(int time)
    {
        _speedingUp = true;
        _speedUpTime += time;
        if(_speedUpTime > 1800)
        {
            _speedUpTime = 1800;
        }
        _happyHourSpeed = 2;
        _VFXFireworksPool.StartTheParty();
        UserDataController.UpdateSpeedUpData((int)_speedUpTime);
    }

    public void HardcoinsPurchase()
    {
        if(_economyManager.SpendHardCoins(_speedHardCoinsCost))
        {
            //CAMBIAAAR
            _rewardManager.EarnSpeedUp(200);
        }
    }
    private void Update()
    {
        if (_panelIsOpen)
        {
            _remainingTimeTx.text = GetRemainingTime();
            _timeFillBar.fillAmount = _speedUpTime / 1800f;
        }
        if (_speedingUp)
        {
            _activeFeedbackPanel.gameObject.SetActive(true);
            _activeTime.text = GetRemainingTime();
            _speedUpTime -= Time.deltaTime;
            if (_speedUpTime < 0)
            {
                _speedingUp = false;
                _speedUpTime = 0;
                _VFXFireworksPool.StopTheParty();
                _happyHourSpeed = 1;
                _activeFeedbackPanel.gameObject.SetActive(false);
            }
        }
        else
        {
            _activeFeedbackPanel.gameObject.SetActive(false);
        }

    }
    public string GetRemainingTime()
    {
        int initialUnits = Mathf.FloorToInt(_speedUpTime);
        int minutes = initialUnits / 60;
        int seconds = initialUnits % 60;
        string time = minutes.ToString("00") + ":" + seconds.ToString("00");
        return time;
    }

    public float GetHappyHourSpeed()
    {
        return _happyHourSpeed;
    }

    public Vector3 GetSpeedUpPosition()
    {
        return _speedUpButton.position;
    }
    public Vector3 GetAdButtPosition()
    {
        return _adButton.position;
    }

    public bool IsPanelOpen()
    {
        return _panelIsOpen;
    }
}
