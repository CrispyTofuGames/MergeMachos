using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;
using TMPro;
public class SpinManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    GameObject _warningIcon;
    int _currentTries;
    bool spinning = false;
    [SerializeField]
    RectTransform _spin;
    BoxManager _boxManager;
    EconomyManager _economyManager;
    enum SpinRewards { Gems , BigSpeedTime, Money2H,  SmallSpeedTime, Money4H, Boxes };
    SpinRewards _obtainedReward;
    float _nextAdTime;
    int timeToNextAd = 3600;
    PanelManager _panelManager;
    RewardManager _rewardManager;
    [SerializeField]
    TextMeshProUGUI _price7200Label;
    [SerializeField]
    TextMeshProUGUI _price14400Label;
    [SerializeField]
    AnimationCurve _animationCurve;
    Quaternion _finalRotation;
    [SerializeField]
    Button _gemButton, _freeButton;
    bool _canClose;
    private void Start()
    {
        _boxManager = FindObjectOfType<BoxManager>();
        _economyManager = FindObjectOfType<EconomyManager>();
        _currentTries = UserDataController.GetSpinRemainingAds();
        _panelManager = FindObjectOfType<PanelManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
        if(UserDataController.GetFreeSpinTries() > 0)
        {
            _warningIcon.SetActive(true);
        }
        else
        {
            _warningIcon.SetActive(false);
        }

        int finalCount = (int)System.DateTime.Now.Subtract(UserDataController.GetSpinLastViewTime()).TotalSeconds;     
        int nAds = finalCount / timeToNextAd;
        int remainingTime = finalCount % timeToNextAd;
        _currentTries = Mathf.Min((_currentTries + nAds),3);
        if(_currentTries < 3)
        {
            _nextAdTime = timeToNextAd - remainingTime;
        }

    }
    public void Open()
    {
        _panelManager.RequestShowPanel(_mainPanel);
        GameCurrency baseRewardPSec;
        baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
        baseRewardPSec.MultiplyCurrency(7200);
        _price7200Label.text = baseRewardPSec.GetCurrentMoney();
        baseRewardPSec.MultiplyCurrency(2);
        _price14400Label.text = baseRewardPSec.GetCurrentMoney();
        _spin.rotation = Quaternion.identity;
        _canClose = true;
        if (UserDataController.GetFreeSpinTries() > 0)
        {
            _freeButton.gameObject.SetActive(true);
            _gemButton.gameObject.SetActive(false);
        }
        else
        {
            _freeButton.gameObject.SetActive(false);
            _gemButton.gameObject.SetActive(true);
        }
        
    }
    public void CloseSpin()
    {
        if (_canClose)
        {
            _panelManager.ClosePanel();
        }
        
    }

    public void SpinCallBack()
    {
        _currentTries--;
        if(_currentTries == 2)
        {
            _nextAdTime = (float)timeToNextAd;
        }
        Spin();
    }

    public void HardCoinSpin()
    {
        if (!spinning)
        {
            if (_economyManager.SpendHardCoins(3))
            {
                Spin();
            }
        }
    }

    public void Spin()
    {
        if (!spinning)
        {
            StartCoroutine(SpinRouleteCr());
        }
    }
    public void Update()
    {
        if (spinning)
        {
            _spin.Rotate(new Vector3(0,0, Time.deltaTime * 180));
        }
        if (_currentTries < 3)
        {
            _nextAdTime -= Time.deltaTime;
            if(_nextAdTime <= 0f)
            {
                _currentTries++;
                _nextAdTime = timeToNextAd;
                UserDataController.UpdateSpinData(_currentTries);
            }
        }
    }

    IEnumerator SpinRouleteCr()
    {
        _canClose = false;
        _freeButton.interactable = false;
        _gemButton.interactable = false;
        int nSections = Random.Range(30, 35);
        for(float i = 0; i <5f; i+=Time.deltaTime)
        {
            _spin.rotation = Quaternion.Euler(0,0,(60*nSections)*(_animationCurve.Evaluate(i/5f)));
            yield return null;
        }
        _spin.rotation = Quaternion.Euler(0, 0, 60 * nSections);
        yield return new WaitForSeconds(1f);
        spinning = false;
        _obtainedReward = (SpinRewards)(nSections%6);
        int tries = UserDataController.GetFreeSpinTries();
        if (tries > 0)
        {
            tries--;
        }
        UserDataController.SetFreeSpinTries(tries);

        switch (_obtainedReward)
        {
            case SpinRewards.Gems:
                _rewardManager.EarnHardCoin(5);
                break;
            case SpinRewards.BigSpeedTime:
                _rewardManager.EarnSpeedUp(400);
                break;

            case SpinRewards.Money2H:
                _rewardManager.EarnSoftCoin(7200);
                break;

            case SpinRewards.SmallSpeedTime:
                _rewardManager.EarnSpeedUp(200);
                break;

            case SpinRewards.Money4H:
                _rewardManager.EarnSoftCoin(14400);
                break;

            case SpinRewards.Boxes:
                _boxManager.RewardBox(4);
                _rewardManager.EarnGifts(4);
                break;
        }
        UserDataController.UpdateSpinData(_currentTries);
        //COMPROBAR 2 tiros

        if (UserDataController.GetFreeSpinTries()>0)
        {
            _warningIcon.SetActive(true);
        }
        else
        {
            _warningIcon.SetActive(false);
            _freeButton.gameObject.SetActive(false);
        }
        _freeButton.interactable = true;
        _gemButton.interactable = true;
        _canClose = true;
        CloseSpin();
    }
}