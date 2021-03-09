using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExtraUpgradeObserver : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI[] _amountTx;
    [SerializeField]
    GameObject[] _icons;
    [SerializeField]
    UpgradesManager _upgradesManager;
    [SerializeField]
    SpeedUpManager _speedUpManager;

    void Update()
    {
        float hireDiscount = _upgradesManager.GetDiscount();
        if (_icons[0].activeSelf)
        {
            if (hireDiscount == 0)
            {
                _icons[0].SetActive(false);
            }
        }
        else
        {
            if(hireDiscount > 0)
            {
                _icons[0].SetActive(true);
            }
        }
        _amountTx[0].text = "-" + hireDiscount + "%";


        float extraEarnings = _upgradesManager.GetExtraEarnings();
        if (_icons[1].activeSelf)
        {
            if (extraEarnings == 0)
            {
                _icons[1].SetActive(false);
            }
        }
        else
        {
            if (extraEarnings > 0)
            {
                _icons[1].SetActive(true);
            }
        }
        _amountTx[1].text = extraEarnings + 100 + "%";

        float extraSpeed = (100 + _upgradesManager.GetExtraTouristSpeed()) * _speedUpManager.GetHappyHourSpeed();
        if (_icons[2].activeSelf)
        {
            if (extraSpeed == 100)
            {
                _icons[2].SetActive(false);
            }
        }
        else
        {
            if (extraSpeed > 100)
            {
                _icons[2].SetActive(true);
            }
        }
        _amountTx[2].text = extraSpeed +  "%";



        float passiveEarnings = _upgradesManager.GetExtraPassiveEarnings();
        if (_icons[3].activeSelf)
        {
            if (passiveEarnings == 0)
            {
                _icons[3].SetActive(false);
            }
        }
        else
        {
            if (passiveEarnings > 0)
            {
                _icons[3].SetActive(true);
            }
        }
        _amountTx[3].text = passiveEarnings + 100 + "%";
    }
}
